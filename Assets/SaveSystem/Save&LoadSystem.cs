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

    #region Save
    public static void Save(PlayerScript player = null)
    {
        if (player != null)
        {
            string json = JsonUtility.ToJson(player.PlayerData);
            File.WriteAllText(playerDataPath, json);
        }
    }
    #endregion

    public static void Load(PlayerScript player = null)
    {
        if (player != null)
        {
            if (File.Exists(playerDataPath))
            {
                string json = File.ReadAllText(playerDataPath);
                Debug.Log(json);
                player.PlayerData = JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                Debug.LogError("Failed to find file at path" + playerDataPath);
            }
        }
    }
}
