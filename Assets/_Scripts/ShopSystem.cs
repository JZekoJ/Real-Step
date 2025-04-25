using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int price;
    }

    public MoneySystem moneySystem;
    public ShopItem[] itemsForSale;

    public GameObject itemUIPrefab;
    public List<Item> generatedItems = new List<Item>();

    public InventoryScript inventory;

    private void Start()
    {
        GenerateRandomItems();
    }
    private List<int> GenerateStats()
    {
        List<int> stats = new List<int>();
        for (int i = 0; i < System.Enum.GetValues(typeof(ItemStats)).Length; i++)
        {
            stats.Add(UnityEngine.Random.Range(1, 10));
        }
        return stats;
    }

    public void BuyItem(int index)
    {
        if (index < 0 || index >= generatedItems.Count)
        {
            Debug.LogWarning("Index d'objet invalide.");
            return;
        }

        Item item = generatedItems[index];
        int price = CalculatePrice(item);

        if (moneySystem.CurrentMoney >= price)
        {
            moneySystem.RemoveMoney(price);
            //inventory.InitInventory();
            Debug.Log("Acheté : " + item._Type + " niveau " + item._iLevel + " pour " + price + " pièces.");
        }
        else
        {
            Debug.Log("Pas assez d'argent pour acheter : " + item._Type);
        }
    }


    public void GenerateRandomItems()
    {
        generatedItems.Clear();

        Item newItem = new Item
            {
                _iLevel = UnityEngine.Random.Range(1, 20),
                _Rarity = (ItemRarity)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ItemRarity)).Length),
                _Type = (ItemType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length),
                _bIsEquiped = false,
                _Stats = GenerateStats()
            };

            GameObject uiObj = Instantiate(itemUIPrefab, this.transform);
            ItemUIScript itemUI = uiObj.GetComponent<ItemUIScript>();
            itemUI._EquipItem = newItem;
            itemUI.Init();

            generatedItems.Add(newItem);
    }

    public int CalculatePrice(Item item)
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