using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonManager : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public string roomName;
        public GameObject roomPrefab;
        public List<GameObject> enemyPrefabs = new List<GameObject>();
        
    }

    [SerializeField] private SwipeDetection swipeDetection;
    [SerializeField] private Tools tools;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private Item item;
    [SerializeField] private SaveLoadSystem saveLoadSystem;


    [Header("Paramètres des Rooms")]
    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private int currentRoomIndex = 0;
    [SerializeField] private float roomTransitionDelay = 2f;
    [SerializeField] private Transform roomSpawnPoint;

    [Header("Paramètres de Spawn")]
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private float enemySpawnDelay = 1f;

    [Header("Événements")]
    public UnityEvent onDungeonCompleted;
    public UnityEvent<Room> onRoomChanged;
    public UnityEvent<GameObject> onEnemySpawned;
    public UnityEvent<GameObject> onEnemyDefeated;

    // Note: Ces événements sont destinés aux systèmes externes (UI, effets, etc.)
    // et ne sont plus nécessaires pour la communication interne
    
    
    private bool isDungeonCompleted = false;
    private GameObject currentRoomInstance;
    



    private List<GameObject> currentRoomEnemies = new List<GameObject>();
    private int currentEnemyIndex = 0;
    private GameObject currentEnemy;
    private bool isWaveActive = false;

    private void Awake()
    {
        if (roomSpawnPoint == null)
        {
            roomSpawnPoint = transform;
            Debug.LogWarning("Point de spawn des rooms non assigné, utilisation de la transform du DungeonManager.");
        }

        if (enemySpawnPoint == null)
        {
            enemySpawnPoint = transform;
            Debug.LogWarning("Point de spawn des ennemis non assigné, utilisation de la transform du DungeonManager.");
        }
    }

    private void Start()
    {
        
        StartDungeon();
    }

    #region Gestion du Donjon et des Rooms

    public void StartDungeon()
    {
        isDungeonCompleted = false;
        currentRoomIndex = 0;
        
        SpawnRoom(currentRoomIndex);
    }

    private void SpawnRoom(int roomIndex)
    {
        if (roomIndex >= rooms.Count)
        {
            CompleteDungeon();
            return;
        }

        Room currentRoom = rooms[roomIndex];
        
        if (currentRoomInstance != null)
        {
            Destroy(currentRoomInstance);
        }
        
        if (currentRoom.roomPrefab != null)
        {
            currentRoomInstance = Instantiate(currentRoom.roomPrefab, roomSpawnPoint.position, roomSpawnPoint.rotation);
            currentRoomInstance.name = "Room_" + currentRoom.roomName;
        }
        else
        {
            Debug.LogWarning($"Préfab de room manquant pour {currentRoom.roomName}");
        }

        // Notifier les systèmes externes du changement de room (UI, etc.)
        onRoomChanged?.Invoke(currentRoom);

        // Démarrer la vague avec les ennemis de la room actuelle
        StartWave(currentRoom.enemyPrefabs);

        Debug.Log($"Démarrage de la room {currentRoom.roomName} avec {currentRoom.enemyPrefabs.Count} ennemis");
    }

    private void MoveToNextRoom()
    {
        // Passer à la room suivante après un délai
        StartCoroutine(MoveToNextRoomAfterDelay());
    }

    private IEnumerator MoveToNextRoomAfterDelay()
    {
        yield return new WaitForSeconds(roomTransitionDelay);

        // Passer à la room suivante
        currentRoomIndex++;
        SpawnRoom(currentRoomIndex);
    }

    private void CompleteDungeon()
    {
        if (isDungeonCompleted)
            return;

        Item i = new Item();
        i._iLevel = 1;
        i._Rarity = ItemRarity.Rare;
        i._Type = ItemType.Weapon;
        i._iStrength = 10;
        i._iSpirit = 5;
        i._iIntel = 0;
        i._iVita = 0;
        i._iChar = 0;
        i._iDext = 0;
        ReferenceManager.Player.AddItem(i);


        isDungeonCompleted = true;
        Debug.Log("Donjon terminé!");

        onDungeonCompleted?.Invoke();



        //  Changer de scène

        if (tools != null)
        {
            tools.ChangeScene("Map");
        }
        else
        {
            Debug.LogError(" Tools est null ! Tu l'as pas assigné dans l’inspecteur ?");
        }

        SaveLoadSystem.Save(playerScript);
    }

    // Méthode pour obtenir la room actuelle
    public Room GetCurrentRoom()
    {
        if (currentRoomIndex < rooms.Count)
            return rooms[currentRoomIndex];

        return null;
    }

    // Méthode pour obtenir l'index de la room actuelle
    public int GetCurrentRoomIndex()
    {
        return currentRoomIndex;
    }

    // Méthode pour obtenir le nombre total de rooms
    public int GetTotalRoomCount()
    {
        return rooms.Count;
    }

    #endregion

    #region Gestion des Ennemis
    
    private void StartWave(List<GameObject> enemyPrefabs)
    {
        if (isWaveActive)
        {
            ClearCurrentWave();
        }

        isWaveActive = true;
        currentRoomEnemies = new List<GameObject>(enemyPrefabs);
        currentEnemyIndex = 0;

        // Commencer à faire apparaître les ennemis
        StartCoroutine(SpawnNextEnemyAfterDelay());
    }
    
    private IEnumerator SpawnNextEnemyAfterDelay()
    {
        yield return new WaitForSeconds(enemySpawnDelay);

        if (currentEnemyIndex < currentRoomEnemies.Count)
        {
            SpawnEnemy(currentEnemyIndex);
        }
        else
        {
            CompleteWave();
        }
    }

    private void SpawnEnemy(int index)
    {
        if (index < 0 || index >= currentRoomEnemies.Count)
            return;

        currentEnemy = Instantiate(currentRoomEnemies[index], enemySpawnPoint.position, enemySpawnPoint.rotation);

       
        Enemy enemyComponent = currentEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.onDeath.AddListener(() => OnEnemyDefeated(currentEnemy));

            //  Set dans SwipeDetection
            if (swipeDetection != null)
            {
                swipeDetection.SetCurrentEnemy(enemyComponent);
                enemyComponent.StartAttacking(swipeDetection);
            }
        }
        else
        {
            Debug.LogWarning("Le préfab d'ennemi n'a pas de composant Enemy!");
        }

        onEnemySpawned?.Invoke(currentEnemy);

        Debug.Log($"Ennemi {index + 1} sur {currentRoomEnemies.Count} apparu");
    }

    private void OnEnemyDefeated(GameObject enemy)
    {
        onEnemyDefeated?.Invoke(enemy);

        // Passer à l'ennemi suivant
        currentEnemyIndex++;
        StartCoroutine(SpawnNextEnemyAfterDelay());
    }
    
    private void CompleteWave()
    {
        isWaveActive = false;
        Debug.Log("Vague terminée!");

        // Passer à la room suivante
        MoveToNextRoom();
    }
    
    private void ClearCurrentWave()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
        }
        
        currentRoomEnemies.Clear();
        currentEnemyIndex = 0;
        isWaveActive = false;
    }
    
    public bool IsWaveActive()
    {
        return isWaveActive;
    }
    
    public GameObject GetCurrentEnemy()
    {
        return currentEnemy;
    }
    
    public float GetWaveProgress()
    {
        if (currentRoomEnemies.Count == 0)
            return 0f;

        return (float)currentEnemyIndex / currentRoomEnemies.Count;
    }
    
    public int GetRemainingEnemyCount()
    {
        return currentRoomEnemies.Count - currentEnemyIndex;
    }

    #endregion
}