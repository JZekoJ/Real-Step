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

                //player.AddItem(12, ItemRarity.S, ItemType.Gauntlet);
                player.AddItem(ItemUtils.GenerateRandomItem());
                Item csItem1 = new Item();
                csItem1._iLevel = 12;
                csItem1._Rarity = ItemRarity.S;
                csItem1._Type = ItemType.Weapon;
                csItem1._Stats[(int)ItemStats.Strength] = 6;
                csItem1._Stats[(int)ItemStats.Dext] = 4;
                csItem1._Stats[(int)ItemStats.Vita] = 2;
                player.AddItem(csItem1);
                ReferenceManager.Player.PlayerData._iMoney = 10000;
                Save(player);
            }
        }
    }
    #endregion
}
