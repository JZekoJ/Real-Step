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
            for (int i = 0; i < item._Stats.Count; i++)
            {
                list[i] += item._Stats[i];
            }
        }
        return list;
    }
    //public int _iPlayerAttack(int strenght, int intel, int _char, int dext)
    //{
    //    int iPlayerAttack = (strenght / 3) + (intel / 2) + (_char / 3) + (dext / 2);
    //    return iPlayerAttack;
    //}

    //public int _iPlayerDefense(int streght, int spirit, int intel, int _char)
    //{
    //    int iPlayerDefense = (streght / 3) + (spirit / 2) + (intel / 2) + (_char / 3);
    //    return iPlayerDefense;
    //}

    //public int _iPlayerHp(int strenght, int spirit, int vita, int _char)
    //{
    //    int iPlayerHp = ((strenght / 3) + (spirit / 2) + vita + (_char / 3)) / 10;
    //    return iPlayerHp;
    //}
    #endregion

}
