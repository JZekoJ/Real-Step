using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyGenerator
{
    public List<GameObject> Generate(List<GameObject> enemyPrefabs, int count)
    {
        List<GameObject> result = new List<GameObject>();

        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("Liste de prefabs vide dans EnemyGenerator !");
            return result;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            result.Add(prefab);
        }

        return result;
    }
}
