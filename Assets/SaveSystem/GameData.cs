using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int _iLevel;
    public int _iHp;
    public List<Item> _itemList = new List<Item>();

    public Dictionary<ItemType, Item> ItemSlot = new Dictionary<ItemType, Item>();
}

#region ItemClass
public enum ItemType
{
    Head,
    Chest,
    Weapon,
    Gauntlet,
    Boot,
    Legging
}
public enum ItemRarity
{
    Commun,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class Item
{
    public int _iLevel;
    public ItemRarity _Rarity;
    public ItemType _Type;
    public bool _bIsEquiped;

    public int _iStrength;
    public int _iSpirit;
    public int _iIntel;
    public int _iVita;
    public int _iChar;
    public int _iDext;
}
#endregion