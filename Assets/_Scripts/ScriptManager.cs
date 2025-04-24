using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager Instance;

    public ShopSystem shopSystem;
    public MoneySystem moneySystem;
    public InventoryScript inventory;

    private void Awake()
    {
        shopSystem.moneySystem = moneySystem.GetComponent<MoneySystem>();
        shopSystem.inventory = inventory.GetComponent<InventoryScript>();
    }
}

