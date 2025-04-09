using UnityEngine;
using System.IO;
using System;

public class SaveLoadSystem
{
    #region Save
    public static void Save(PlayerScript player = null)
    {
        if (player != null)
        {
            string playerDataPath = Application.persistentDataPath + "/playerData.json";
            string json = JsonUtility.ToJson(player.PlayerData);
            File.WriteAllText(playerDataPath, json);
        }
    }
    #endregion

    #region Load
    public static void Load(PlayerScript player = null)
    {
        string playerDataPath = Application.persistentDataPath + "/playerData.json";
        if (player != null)
        {
            if (File.Exists(playerDataPath))
            {
                string json = File.ReadAllText(playerDataPath);
                Debug.Log(playerDataPath);
                Debug.Log(json);
                player.PlayerData = JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                Debug.Log("No save file detected, create new one");
                Save(player);
            }
        }
    }
    #endregion
}
