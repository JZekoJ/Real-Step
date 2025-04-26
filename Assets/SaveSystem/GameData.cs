using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int _iLevel;
    public int _iHp;

    public int _iMoney;

    public List<Item> _itemList;

    public List<Item> ItemSlot;

    public PlayerData()
    {
        _itemList = new List<Item>();
        ItemSlot = new List<Item> { null, null, null, null, null, null };
    }


    public int _iPlayerAttack(int strenght, int intel, int _char, int dext)
    {
        int iPlayerAttack = (strenght / 3) + (intel /2) + (_char / 3) + (dext / 2);
        return iPlayerAttack;
    }

    public int _iPlayerDefense(int streght, int spirit, int intel, int _char)
    {
        int iPlayerDefense = (streght / 3) + (spirit / 2) + (intel / 2) + (_char / 3);
        return iPlayerDefense;
    }

    public int _iPlayerHp(int strenght, int spirit, int vita, int _char)
    {
        int iPlayerHp = ((strenght/3) + (spirit/2) + vita + (_char /3))/10;
        return iPlayerHp;
    }

    

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

    public List<int> _Stats = new List<int> { 10, 0, 0, 0, 0, 0 };

}


#endregion
