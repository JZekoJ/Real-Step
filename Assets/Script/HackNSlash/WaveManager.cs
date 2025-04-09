using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    [Header("Paramètres de Spawn")]
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private float spawnDelay = 1f;
    
    [Header("Événements")]
    public UnityEvent onWaveCompleted;
    public UnityEvent<GameObject> onEnemySpawned;
    public UnityEvent<GameObject> onEnemyDefeated;
    
    private List<GameObject> currentWaveEnemies = new List<GameObject>();
    private int currentEnemyIndex = 0;
    private GameObject currentEnemy;
    private bool isWaveActive = false;

    private void Awake()
    {
        // Vérifier si le point de spawn est assigné
        if (enemySpawnPoint == null)
        {
            enemySpawnPoint = transform;
            Debug.LogWarning("Point de spawn des ennemis non assigné, utilisation de la transform du WaveManager.");
        }
    }

    // Démarrer une nouvelle vague avec les préfabs d'ennemis donnés
    public void StartWave(List<GameObject> enemyPrefabs)
    {
        if (isWaveActive)
        {
            // Si une vague est déjà active, la nettoyer d'abord
            ClearCurrentWave();
        }
        
        isWaveActive = true;
        currentWaveEnemies = new List<GameObject>(enemyPrefabs);
        currentEnemyIndex = 0;
        
        // Commencer à faire apparaître les ennemis
        StartCoroutine(SpawnNextEnemyAfterDelay());
    }
    
    // Faire apparaître le prochain ennemi dans la vague
    private IEnumerator SpawnNextEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        
        if (currentEnemyIndex < currentWaveEnemies.Count)
        {
            SpawnEnemy(currentEnemyIndex);
        }
        else
        {
            // Tous les ennemis ont été générés et vaincus
            CompleteWave();
        }
    }
    
    // Faire apparaître un ennemi spécifique par index
    private void SpawnEnemy(int index)
    {
        if (index < 0 || index >= currentWaveEnemies.Count)
            return;
            
        // Instancier l'ennemi au point de spawn
        currentEnemy = Instantiate(currentWaveEnemies[index], enemySpawnPoint.position, enemySpawnPoint.rotation);
        
        // S'abonner à l'événement de mort de l'ennemi
        Enemy enemyComponent = currentEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.onDeath.AddListener(() => OnEnemyDefeated(currentEnemy));
        }
        else
        {
            Debug.LogWarning("Le préfab d'ennemi n'a pas de composant Enemy!");
        }
        
        // Déclencher l'événement d'apparition d'ennemi
        onEnemySpawned?.Invoke(currentEnemy);
        
        Debug.Log($"Ennemi {index + 1} sur {currentWaveEnemies.Count} apparu");
    }
    
    // Appelé quand un ennemi est vaincu
    private void OnEnemyDefeated(GameObject enemy)
    {
        // Déclencher l'événement d'ennemi vaincu
        onEnemyDefeated?.Invoke(enemy);
        
        // Passer à l'ennemi suivant
        currentEnemyIndex++;
        StartCoroutine(SpawnNextEnemyAfterDelay());
    }
    
    // Terminer la vague actuelle
    private void CompleteWave()
    {
        isWaveActive = false;
        Debug.Log("Vague terminée!");
        
        // Déclencher l'événement de fin de vague
        onWaveCompleted?.Invoke();
    }
    
    // Nettoyer la vague actuelle (utilisé lors des transitions de rooms)
    private void ClearCurrentWave()
    {
        // Détruire l'ennemi actuel s'il existe
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
        }
        
        // Réinitialiser l'état de la vague
        currentWaveEnemies.Clear();
        currentEnemyIndex = 0;
        isWaveActive = false;
    }
    
    // Vérifier si la vague est actuellement active
    public bool IsWaveActive()
    {
        return isWaveActive;
    }
    
    // Obtenir l'ennemi actuel en combat
    public GameObject GetCurrentEnemy()
    {
        return currentEnemy;
    }
    
    // Obtenir la progression de la vague actuelle (0-1)
    public float GetWaveProgress()
    {
        if (currentWaveEnemies.Count == 0)
            return 0f;
            
        return (float)currentEnemyIndex / currentWaveEnemies.Count;
    }
    
    // Obtenir le nombre d'ennemis restants dans la vague
    public int GetRemainingEnemyCount()
    {
        return currentWaveEnemies.Count - currentEnemyIndex;
    }
}