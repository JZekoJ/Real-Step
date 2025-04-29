using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyGenerator : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] private List<GameObject> m_lEnemyPool;
    [SerializeField] private GameObject m_goBossPrefab;
    [SerializeField] private int m_iRoomCount = 2;
    [SerializeField] private int m_iMinEnemiesPerRoom = 2;
    [SerializeField] private int m_iMaxEnemiesPerRoom = 4;

    public List<List<GameObject>> GenerateAllRooms()
    {
        List<List<GameObject>> allRooms = new List<List<GameObject>>();

        if (m_lEnemyPool == null || m_lEnemyPool.Count == 0)
        {
            Debug.LogError("Enemy pool vide !");
            return allRooms;
        }

        for (int i = 0; i < m_iRoomCount; i++)
        {
            bool isLastRoom = (i == m_iRoomCount - 1);
            int enemyCount = Random.Range(m_iMinEnemiesPerRoom, m_iMaxEnemiesPerRoom + 1);
            allRooms.Add(GenerateRoomEnemies(enemyCount, isLastRoom));
        }

        return allRooms;
    }

    public List<GameObject> GenerateRoomEnemies(int count, bool isLastRoom)
    {
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject prefab = m_lEnemyPool[Random.Range(0, m_lEnemyPool.Count)];
            result.Add(prefab);
        }

        if (isLastRoom && m_goBossPrefab != null && result.Count > 0)
        {
            result[result.Count - 1] = m_goBossPrefab;
            Debug.Log(" Boss assigné à la dernière room");
        }

        return result;
    }

    public int GetRoomCount() => m_iRoomCount;
}
