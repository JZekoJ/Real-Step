using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class SaveLoadSystem
{
    #region Save
    public static void Save(PlayerScript player = null)
    {
        if (player != null)
        {
            string playerDataPath = Application.persistentDataPath + "/playerData.json";
            string json = JsonConvert.SerializeObject(player.PlayerData, Formatting.Indented);
            Debug.Log(playerDataPath);
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
                player.PlayerData = JsonConvert.DeserializeObject<PlayerData>(json, new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace });
            }
            else
            {
                Debug.Log("No save file detected, create new one");
                PlayerData data = new PlayerData();
                player.PlayerData = data;

                player.AddItem(12, ItemRarity.S, ItemType.Gauntlet);
                Save(player);
            }
        }
    }
    #endregion
}
