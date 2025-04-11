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
        PlayerData = new PlayerData();
        PlayerData.ItemSlot = new Dictionary<ItemType, Item>();

        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            PlayerData.ItemSlot.Add(type,default(Item));
        }
        Debug.Log(PlayerData.ItemSlot[ItemType.Head]);
    }

    public void EquipItem(Item item)
    {
        if (PlayerData.ItemSlot[item._Type] != null)
        {
            PlayerData.ItemSlot[item._Type]._bIsEquiped = false;
        }
        PlayerData.ItemSlot[item._Type] = item;
        item._bIsEquiped = true;
        onEquip.Invoke(item);
    }

    #region Inventory
    public void AddItem(Item item)
    {
        PlayerData._itemList.Add(item);
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

    //#region Stats
    //public void ChangeLevel(int amount)
    //{
    //    PlayerData._iLevel += amount;
    //    UpdateUI();
    //}
    //public void ChangeHp(int amount)
    //{
    //    PlayerData._iHp += amount;
    //    UpdateUI();
    //}
    //#endregion

    //public void UpdateUI()
    //{
    //    lvlUI.text = "lvl : " + PlayerData._iLevel;
    //    hpUI.text = "hp : " + PlayerData._iHp;
    //}

}
