using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager Instance;

    public MoneySystem moneySystem;
    public InventoryScript inventory;

    private void Awake()
    {

    }
}

