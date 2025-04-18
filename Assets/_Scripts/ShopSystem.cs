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

    public MoneySystem moneySystem; // Référence au MoneySystem
    public ShopItem[] itemsForSale; // Liste des objets à vendre

    public void BuyItem(int index)
    {
        if (index < 0 || index >= itemsForSale.Length)
        {
            Debug.LogWarning("Index d'objet invalide.");
            return;
        }

        ShopItem item = itemsForSale[index];

        if (moneySystem.CurrentMoney >= item.price)
        {
            moneySystem.RemoveMoney(item.price);
            Debug.Log("Acheté : " + item.itemName + " pour " + item.price + " pièces.");
        }
        else
        {
            Debug.Log("Pas assez d'argent pour acheter : " + item.itemName);
        }
    }
}