using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int _iLevel;
    public int _iHp;
    public List<Item> _itemList = new List<Item>();
}

public enum ItemType
{
    Head,
    Chest,
    Weapon
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
}
