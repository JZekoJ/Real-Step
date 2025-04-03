using UnityEngine;
using System.IO;
using System;



[System.Serializable]
public class SaveLoadSystem : MonoBehaviour
{
    #region Singleton
    
    private static SaveLoadSystem instance = null;
    public static SaveLoadSystem GetInstance() { return instance; }

    private void Awake()
    {
        playerDataPath = Application.persistentDataPath + "/playerData.json";
        inventoryDataPath = Application.persistentDataPath + "/inventoryData.json";
        Debug.Log(playerDataPath);
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    private static string playerDataPath;
    private static string inventoryDataPath;

    public static void Save(PlayerScript player = null)
    {
        if (player != null)
        {
            PlayerData data = new PlayerData(player);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(playerDataPath, json);
        }
    }

    public static void Load(PlayerScript player = null)
    {
        if (player != null)
        {
            if (File.Exists(playerDataPath))
            {
                string json = File.ReadAllText(playerDataPath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                player.SetData(data);
            }
            else
            {
                Debug.LogError("Failed to find file at path" + playerDataPath);
            }
        }

    }
}
