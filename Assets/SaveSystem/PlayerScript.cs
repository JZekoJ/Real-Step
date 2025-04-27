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


}
