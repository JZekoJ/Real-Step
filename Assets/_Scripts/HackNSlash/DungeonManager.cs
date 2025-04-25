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
    #endregion

    #region UI
    [SerializeField] private Slider m_sSlider;
    [SerializeField] private Slider m_sBossSlider;
    #endregion

    #region UI
    [SerializeField] private Slider m_sSlider;
    [SerializeField] private Slider m_sBossSlider;
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
    #endregion

    #region Events
    [Header("Événements")]
    public UnityEvent m_eOnDungeonCompleted;
    public UnityEvent<Room> m_eOnRoomChanged;
    public UnityEvent<GameObject> m_eOnEnemySpawned;
    public UnityEvent<GameObject> m_eOnEnemyDefeated;
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

    #region Gestion du Donjon
    public void StartDungeon()
    {
        if (DungeonTransfer.EnemiesToSpawn != null && DungeonTransfer.EnemiesToSpawn.Count > 0 && m_lRooms.Count > 0)
        {
            m_lRooms[0].m_lEnemyPrefabs = new List<GameObject>(DungeonTransfer.EnemiesToSpawn);
            DungeonTransfer.EnemiesToSpawn.Clear(); 
        }

        m_bIsDungeonCompleted = false;
        m_iCurrentRoomIndex = 0;
        SpawnRoom(m_iCurrentRoomIndex);
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
        csItem._Rarity = ItemRarity.B;
        csItem._Type = ItemType.Weapon;
        csItem._Stats[(int)ItemStats.Strength] = 10;
        ReferenceManager.Player.AddItem(csItem);
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

    public bool IsWaveActive() => m_bIsWaveActive;
    public GameObject GetCurrentEnemy() => m_goCurrentEnemy;
    public float GetWaveProgress() => m_lCurrentRoomEnemies.Count == 0 ? 0f : (float)m_iCurrentEnemyIndex / m_lCurrentRoomEnemies.Count;
    public int GetRemainingEnemyCount() => m_lCurrentRoomEnemies.Count - m_iCurrentEnemyIndex;
    #endregion
}
