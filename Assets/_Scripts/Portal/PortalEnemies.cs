using System.Collections.Generic;
using UnityEngine;

public class PortalEnemies : MonoBehaviour
{
    [Header("Paramètres de génération")]
    [SerializeField] private EnemyGenerator m_csEnemyGenerator;
    [SerializeField] private int m_iEnemyCount = 3;

    private List<GameObject> m_lGeneratedEnemies = new List<GameObject>();

    public List<GameObject> GetGeneratedEnemies() => m_lGeneratedEnemies;

    private void Awake()
    {
       if (m_csEnemyGenerator == null)
        {
            //Debug.LogError("EnemyGenerator non assignéPortalEnemies ");
            return;
        }

        // Génère des ennemis sans boss ici
        m_lGeneratedEnemies = m_csEnemyGenerator.GenerateRoomEnemies(m_iEnemyCount, false);
    }
}
