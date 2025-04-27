using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemUtils
{
    public static Item GenerateRandomItem()
    {
        int playerLvl = ReferenceManager.Player.PlayerData._iLevel;
        Item newItem = new Item
        {
            _iLevel = UnityEngine.Random.Range(playerLvl - 10, playerLvl + 10),
            _Rarity = (ItemRarity)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ItemRarity)).Length),
            _Type = (ItemType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length),
            _bIsEquiped = false
        };
        if (newItem._iLevel <= 0)
        {
            newItem._iLevel = 1;
        }

        for (int i = 0; i < (int)newItem._Rarity; i++)
        {
            newItem._Stats[UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ItemStats)).Length - 1)] += UnityEngine.Random.Range(newItem._iLevel, newItem._iLevel + 10);
        }

        return newItem;
    }

    public static int CalculateBuyPrice(Item item)
    {
        int rarityMultiplier = item._Rarity switch
        {
            ItemRarity.E => 1,
            ItemRarity.D => 2,
            ItemRarity.C => 4,
            ItemRarity.B => 8,
            ItemRarity.A => 16,
            ItemRarity.S => 32,
            _ => 1
        };

        int levelValue = item._iLevel * 5;

        int statBonus = 0;
        foreach (int stat in item._Stats)
        {
            statBonus += stat;
        }

        int finalPrice = (levelValue + statBonus) * rarityMultiplier;
        return finalPrice;
    }
}
