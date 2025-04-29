using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    #region Structs
    [System.Serializable]
    public class Room
    {
        public string m_sRoomName;
        public GameObject m_goRoomPrefab;
        public List<GameObject> m_lEnemyPrefabs = new List<GameObject>();
    }
    #endregion

    #region Références
    [SerializeField] private SwipeDetection m_csSwipeDetection;
    [SerializeField] private Tools m_csTools;
    [SerializeField] private PlayerScript m_csPlayerScript;
    [SerializeField] private Item m_csItem;
    [SerializeField] private SaveLoadSystem m_csSaveLoadSystem;
    [SerializeField] private EnemyGenerator m_csEnemyGenerator;
    #endregion

    #region UI
    [SerializeField] private GameObject m_gPrefabItem;
    [SerializeField] private GameObject m_gContentItemObtain;

    [SerializeField] private Slider m_sSlider;
    [SerializeField] private Slider m_sBossSlider;

    [SerializeField]private GameObject m_gBossUI;
    #endregion

    #region Room Settings
    [Header("Paramètres des Rooms")]
    [SerializeField] private List<Room> m_lRooms = new List<Room>();
    [SerializeField] private int m_iCurrentRoomIndex = 0;
    [SerializeField] private float m_fRoomTransitionDelay = 2f;
    [SerializeField] private Transform m_tRoomSpawnPoint;
    #endregion

    #region Spawn Settings
    [Header("Paramètres de Spawn")]
    [SerializeField] private Transform m_tEnemySpawnPoint;
    [SerializeField] private float m_fEnemySpawnDelay = 1f;
    [Header("Boss Settings")]
    [SerializeField] private GameObject m_goBossPrefab;
    #endregion

    #region Events
    [Header("Événements")]
    public UnityEvent m_eOnDungeonCompleted;
    public UnityEvent<Room> m_eOnRoomChanged;
    public UnityEvent<GameObject> m_eOnEnemySpawned;
    public UnityEvent<GameObject> m_eOnEnemyDefeated;
    public UnityEvent m_eOnPlayerDead;
    public UnityEvent m_eEndDungeon;
    #endregion

    #region Private Variables
    private bool m_bIsDungeonCompleted = false;
    private GameObject m_goCurrentRoomInstance;
    private List<GameObject> m_lCurrentRoomEnemies = new List<GameObject>();
    private int m_iCurrentEnemyIndex = 0;
    private GameObject m_goCurrentEnemy;
    private bool m_bIsWaveActive = false;
    #endregion



    private void Awake()
    {
        if (m_tRoomSpawnPoint == null)
        {
            m_tRoomSpawnPoint = transform;
            Debug.LogWarning("Point de spawn des rooms non assigné, utilisation de la transform du DungeonManager.");
        }

        if (m_tEnemySpawnPoint == null)
        {
            m_tEnemySpawnPoint = transform;
            Debug.LogWarning("Point de spawn des ennemis non assigné, utilisation de la transform du DungeonManager.");
        }
    }

    private void Start()
    {
        StartDungeon();
    }

    private void Update()
    {
        if(m_csSwipeDetection.m_iPv <= 0)
        {
            m_eOnPlayerDead.Invoke();
        }
    }

    #region Gestion du Donjon
    public void StartDungeon()
    {
        m_bIsDungeonCompleted = false;
        m_iCurrentRoomIndex = 0;
        m_lRooms.Clear();

        CreateGeneratedRooms(); // ← auto-génération ici
        SpawnRoom(m_iCurrentRoomIndex); // et on lance la première
    }

    private void SpawnRoom(int iRoomIndex)
    {
        if (iRoomIndex >= m_lRooms.Count)
        {
            CompleteDungeon();
            return;
        }

        Room currentRoom = m_lRooms[iRoomIndex];

        if (m_goCurrentRoomInstance != null)
        {
            Destroy(m_goCurrentRoomInstance);
        }

        if (currentRoom.m_goRoomPrefab != null)
        {
            m_goCurrentRoomInstance = Instantiate(currentRoom.m_goRoomPrefab, m_tRoomSpawnPoint.position, m_tRoomSpawnPoint.rotation);
            m_goCurrentRoomInstance.name = "Room_" + currentRoom.m_sRoomName;
        }
        else
        {
            Debug.LogWarning($"Préfab de room manquant pour {currentRoom.m_sRoomName}");
        }

        //  Si c'est la dernière room et qu'il y a au moins un ennemi, on remplace le dernier par le boss
        if (iRoomIndex == m_lRooms.Count - 1 && m_goBossPrefab != null && currentRoom.m_lEnemyPrefabs.Count > 0)
        {
            currentRoom.m_lEnemyPrefabs[currentRoom.m_lEnemyPrefabs.Count - 1] = m_goBossPrefab;
            Debug.Log(" Boss assigné comme dernier ennemi de la dernière room");
        }

        m_eOnRoomChanged?.Invoke(currentRoom);
        StartWave(currentRoom.m_lEnemyPrefabs);
        Debug.Log($"Démarrage de la room {currentRoom.m_sRoomName} avec {currentRoom.m_lEnemyPrefabs.Count} ennemis");
    }


    private void MoveToNextRoom()
    {
        StartCoroutine(MoveToNextRoomAfterDelay());
    }

    private IEnumerator MoveToNextRoomAfterDelay()
    {
        yield return new WaitForSeconds(m_fRoomTransitionDelay);
        m_iCurrentRoomIndex++;
        SpawnRoom(m_iCurrentRoomIndex);
    }

    private void CompleteDungeon()
    {
        if (m_bIsDungeonCompleted)
            return;

        Item csItem = new Item();
        csItem._iLevel = 1;
        csItem._Rarity = ItemRarity.A;
        csItem._Type = ItemType.Weapon;
        csItem._Stats[(int)ItemStats.Strength] = 20;

        Item csItem2 = new Item();
        csItem2._iLevel = 1;
        csItem2._Rarity = ItemRarity.A;
        csItem2._Type = ItemType.Weapon;
        csItem2._Stats[(int)ItemStats.Dext] = 20;

        Item csItem1 = new Item();
        csItem1._iLevel = 1;
        csItem1._Rarity = ItemRarity.C;
        csItem1._Type = ItemType.Legging;
        csItem1._Stats[(int)ItemStats.Strength] = 5;
        csItem1._Stats[(int)ItemStats.Vita] = 50;

        //
        GameObject UIBtn = Instantiate(m_gPrefabItem, m_gContentItemObtain.transform);
        UIBtn.GetComponent<ItemUIScript>()._EquipItem = csItem;
        UIBtn.GetComponent<ItemUIScript>().Init();

        ReferenceManager.Player.AddItem(csItem);

        GameObject UIBtn1 = Instantiate(m_gPrefabItem, m_gContentItemObtain.transform);
        UIBtn1.GetComponent<ItemUIScript>()._EquipItem = csItem1;
        UIBtn1.GetComponent<ItemUIScript>().Init();

        ReferenceManager.Player.AddItem(csItem1);

        GameObject UIBtn2 = Instantiate(m_gPrefabItem, m_gContentItemObtain.transform);
        UIBtn2.GetComponent<ItemUIScript>()._EquipItem = csItem2;
        UIBtn2.GetComponent<ItemUIScript>().Init();

        ReferenceManager.Player.AddItem(csItem2);
        SaveLoadSystem.Save(ReferenceManager.Player);

        m_bIsDungeonCompleted = true;
        Debug.Log("Donjon terminé!");

        m_eOnDungeonCompleted?.Invoke();

        if (m_csTools != null)
        {
            //m_csTools.ChangeScene("Main");
        }
        else
        {
            Debug.LogError(" Tools est null ! Tu l'as pas assigné dans l’inspecteur ?");
        }

        SaveLoadSystem.Save(m_csPlayerScript);
    }

    public Room GetCurrentRoom() => (m_iCurrentRoomIndex < m_lRooms.Count) ? m_lRooms[m_iCurrentRoomIndex] : null;
    public int GetCurrentRoomIndex() => m_iCurrentRoomIndex;
    public int GetTotalRoomCount() => m_lRooms.Count;
    #endregion

    #region Gestion des Ennemis
    private void StartWave(List<GameObject> lEnemyPrefabs)
    {
        if (m_bIsWaveActive)
        {
            ClearCurrentWave();
        }

        m_bIsWaveActive = true;
        m_lCurrentRoomEnemies = new List<GameObject>(lEnemyPrefabs);
        m_iCurrentEnemyIndex = 0;

        StartCoroutine(SpawnNextEnemyAfterDelay());
    }

    private IEnumerator SpawnNextEnemyAfterDelay()
    {
        yield return new WaitForSeconds(m_fEnemySpawnDelay);

        if (m_iCurrentEnemyIndex < m_lCurrentRoomEnemies.Count)
        {
            SpawnEnemy(m_iCurrentEnemyIndex);
        }
        else
        {
            CompleteWave();
        }
    }

    private void SpawnEnemy(int iIndex)
    {
        if (iIndex < 0 || iIndex >= m_lCurrentRoomEnemies.Count)
            return;

        GameObject goEnemyInstance = Instantiate(m_lCurrentRoomEnemies[iIndex], m_tEnemySpawnPoint.transform);
        m_goCurrentEnemy = goEnemyInstance;
        //goEnemyInstance.GetComponent<Enemy>().m_sBossSlider = m_sBossSlider;
        Enemy csEnemy = goEnemyInstance.GetComponent<Enemy>();
        
        if (csEnemy.m_bIsBoss)
        {
            m_gBossUI.SetActive(true);
            csEnemy.m_sSlider = m_sBossSlider;
            csEnemy.m_sSlider.maxValue = csEnemy.GetMaxHealth();
            m_eEndDungeon.Invoke();
        }
        if (csEnemy != null)
        {
            csEnemy.m_eOnDeath.AddListener(() => OnEnemyDefeated(goEnemyInstance));

            if (m_csSwipeDetection != null)
            {
                m_csSwipeDetection.SetCurrentEnemy(csEnemy);
                csEnemy.StartAttacking(m_csSwipeDetection);
            }
        }
        else
        {
            Debug.LogWarning("Le préfab d'ennemi n'a pas de composant Enemy!");
        }

        m_eOnEnemySpawned?.Invoke(goEnemyInstance);
        Debug.Log($"Ennemi {iIndex + 1} sur {m_lCurrentRoomEnemies.Count} apparu");
    }

    private void OnEnemyDefeated(GameObject goEnemy)
    {
        m_eOnEnemyDefeated?.Invoke(goEnemy);
        m_iCurrentEnemyIndex++;
        StartCoroutine(SpawnNextEnemyAfterDelay());
    }

    private void CompleteWave()
    {
        m_bIsWaveActive = false;
        Debug.Log("Vague terminée!");
        m_sSlider.value++;
        MoveToNextRoom(); 
    }


    private void ClearCurrentWave()
    {
        if (m_goCurrentEnemy != null)
        {
            Destroy(m_goCurrentEnemy);
        }

        m_lCurrentRoomEnemies.Clear();
        m_iCurrentEnemyIndex = 0;
        m_bIsWaveActive = false;
    }

    private void CreateGeneratedRooms()
    {
        List<List<GameObject>> generated = m_csEnemyGenerator.GenerateAllRooms();
        for (int i = 0; i < generated.Count; i++)
        {
            Room r = new Room();
            r.m_sRoomName = $"Room_{i + 1}";
            r.m_lEnemyPrefabs = generated[i];
            m_lRooms.Add(r);
        }
    }


    public bool IsWaveActive() => m_bIsWaveActive;
    public GameObject GetCurrentEnemy() => m_goCurrentEnemy;
    public float GetWaveProgress() => m_lCurrentRoomEnemies.Count == 0 ? 0f : (float)m_iCurrentEnemyIndex / m_lCurrentRoomEnemies.Count;
    public int GetRemainingEnemyCount() => m_lCurrentRoomEnemies.Count - m_iCurrentEnemyIndex;
    #endregion
}
