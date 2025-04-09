using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public PlayerData PlayerData;

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

    public void Start()
    {
        PlayerData = new PlayerData();
        AddItem(12, ItemRarity.Legendary, ItemType.Weapon);
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
