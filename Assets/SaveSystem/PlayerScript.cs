using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    public PlayerData PlayerData;
    public UnityEvent<Item> onEquip;

    //[SerializeField] 
    //TextMeshProUGUI lvlUI;
    //[SerializeField]
    //TextMeshProUGUI hpUI;
    //[SerializeField]
    //TMP_InputField nameUI;

    private void Awake()
    {
        ReferenceManager.Player = this;
    }

    public void EquipItem(Item item)
    {
        if (PlayerData.ItemSlot[(int)item._Type] != null)
        {
            PlayerData.ItemSlot[(int)item._Type]._bIsEquiped = false;
        }
        PlayerData.ItemSlot[(int)item._Type] = item;
        item._bIsEquiped = true;
        SaveLoadSystem.Save(ReferenceManager.Player);
    }

    public void UnequipItem(Item item)
    {
        item._bIsEquiped = false;
        PlayerData.ItemSlot[(int)item._Type] = null;
        SaveLoadSystem.Save(ReferenceManager.Player);
    }

    #region Inventory
    public void AddItem(Item item)
    {
        PlayerData._itemList.Add(item);
        SaveLoadSystem.Save(ReferenceManager.Player);
    }
    public void AddItem(int iLevel, ItemRarity Rarity, ItemType Type)
    {
        Item i = new Item();
        i._iLevel = iLevel;
        i._Rarity = Rarity;
        i._Type = Type;
        PlayerData._itemList.Add(i);
    }
    public void SellItem(Item item)
    {
        PlayerData._itemList.Remove(item);
    }
    #endregion

    #region Stat
    public List<int> GetPlayerStat()
    {
        List<int> list = new List<int> { 0, 0, 0, 0, 0, 0 };
        foreach (Item item in PlayerData.ItemSlot)
        {
            if (item != null)
            {
                for (int i = 0; i < item._Stats.Count; i++)
                {
                    list[i] += item._Stats[i];
                }
            }
        }
        return list;
    }
    public int _iPlayerAttack()
    {
        int iPlayerAttack = (GetPlayerStat()[(int)ItemStats.Strength] / 3) + (  GetPlayerStat()[(int)ItemStats.Intel] / 2) + (GetPlayerStat()[(int)ItemStats.Char] / 3) + (GetPlayerStat()[(int)ItemStats.Dext] / 2)+10;
        return iPlayerAttack;
    }
    public int _iPlayerDefense()
    {
        int iPlayerDefense = (GetPlayerStat()[(int)ItemStats.Strength] / 3) + (GetPlayerStat()[(int)ItemStats.Intel] / 2) + (GetPlayerStat()[(int)ItemStats.Intel] / 2) + (GetPlayerStat()[(int)ItemStats.Char] / 3)+10;
        return iPlayerDefense;
    }

    public int _iPlayerHp()
    {
        int iPlayerHp = ((GetPlayerStat()[(int)ItemStats.Strength] / 3) + (GetPlayerStat()[(int)ItemStats.Intel] / 2) + GetPlayerStat()[(int)ItemStats.Vita] + (GetPlayerStat()[(int)ItemStats.Char] / 3)) / 10 + 100;
        return iPlayerHp;
    }
    #endregion

}
