using System.Collections.Generic;
using UnityEngine;

public class PortalEnemies : MonoBehaviour
{
    [Header("Paramètres de génération")]
    [SerializeField] private List<GameObject> m_lAvailableEnemyPrefabs;
    [SerializeField] private int m_iEnemyCount = 3;

    private List<GameObject> m_lGeneratedEnemies = new List<GameObject>();

    public List<GameObject> GetGeneratedEnemies() => m_lGeneratedEnemies;

    private void Awake()
    {
        EnemyGenerator generator = new EnemyGenerator();
        m_lGeneratedEnemies = generator.Generate(m_lAvailableEnemyPrefabs, m_iEnemyCount);
    }
}
