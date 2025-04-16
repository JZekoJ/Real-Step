using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int _iLevel;
    public int _iHp;

    public List<Item> _itemList = new List<Item>();

    public List<Item> ItemSlot = new List<Item> { null, null, null, null, null, null };

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
    E,D,C,B,A,S
}

public enum ItemStats
{
    Strength, Spirit, Intel, Vita, Char, Dext
}

[System.Serializable]
public class Item
{
    public int _iLevel;
    public ItemRarity _Rarity;
    public ItemType _Type;
    public bool _bIsEquiped;

    public List<int> _Stats = new List<int> { 0, 0, 0, 0, 0, 0 };

}
#endregion
