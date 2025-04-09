using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public string roomName;
        public GameObject roomPrefab;
        public List<GameObject> enemyPrefabs = new List<GameObject>();
    }

    [Header("Paramètres des Rooms")]
    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private int currentRoomIndex = 0;
    [SerializeField] private float roomTransitionDelay = 2f;
    [SerializeField] private Transform roomSpawnPoint; // Position où les rooms seront instanciées
    
    [Header("Références")]
    [SerializeField] private WaveManager waveManager;
    
    [Header("Événements")]
    public UnityEvent onDungeonCompleted;
    public UnityEvent<Room> onRoomChanged;
    
    private bool isDungeonCompleted = false;
    private GameObject currentRoomInstance; // L'instance de la room actuelle dans la scène

    private void Awake()
    {
        // Rechercher WaveManager si non assigné
        if (waveManager == null)
        {
            waveManager = FindObjectOfType<WaveManager>();
            if (waveManager == null)
            {
                Debug.LogError("WaveManager non trouvé! Veuillez l'assigner dans l'inspecteur.");
            }
        }
        
        // Vérifier si le point de spawn est assigné
        if (roomSpawnPoint == null)
        {
            roomSpawnPoint = transform;
            Debug.LogWarning("Point de spawn des rooms non assigné, utilisation de la transform du LevelManager.");
        }
    }

    private void Start()
    {
        // S'abonner à l'événement de fin de vague
        waveManager.onWaveCompleted.AddListener(OnWaveCompleted);
        
        // Démarrer avec la première room
        StartDungeon();
    }

    public void StartDungeon()
    {
        isDungeonCompleted = false;
        currentRoomIndex = 0;
        
        // Démarrer avec la première room
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
        
        // Détruire l'ancienne room si elle existe
        if (currentRoomInstance != null)
        {
            Destroy(currentRoomInstance);
        }
        
        // Instancier la nouvelle room
        if (currentRoom.roomPrefab != null)
        {
            currentRoomInstance = Instantiate(currentRoom.roomPrefab, roomSpawnPoint.position, roomSpawnPoint.rotation);
            currentRoomInstance.name = "Room_" + currentRoom.roomName;
        }
        else
        {
            Debug.LogWarning($"Préfab de room manquant pour {currentRoom.roomName}");
        }
        
        // Notifier les abonnés du changement de room
        onRoomChanged?.Invoke(currentRoom);
        
        // Envoyer les ennemis au WaveManager
        waveManager.StartWave(currentRoom.enemyPrefabs);
        
        Debug.Log($"Démarrage de la room {currentRoom.roomName} avec {currentRoom.enemyPrefabs.Count} ennemis");
    }

    private void OnWaveCompleted()
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
            
        isDungeonCompleted = true;
        Debug.Log("Donjon terminé!");
        
        // Déclencher l'événement de fin de donjon
        onDungeonCompleted?.Invoke();
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
}   