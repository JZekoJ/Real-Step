using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToBuyUI : MonoBehaviour
{
    public Item _item;
    private int _price;
    public void Init()
    {
        ReferenceManager.Player.PlayerData._iMoney += 10000;
        _item = ItemUtils.GenerateRandomItem();
        _price = ItemUtils.CalculateBuyPrice(_item);

        GetComponentInChildren<ItemUIScript>()._EquipItem = _item;
        GetComponentInChildren<ItemUIScript>().Init();

        GetComponentInChildren<TextMeshProUGUI>().text = "" + _price;

        Debug.Log(GetComponentsInChildren<Button>()[0].name);
        GetComponentsInChildren<Button>()[1].onClick.AddListener(() =>
        {
            //Buy
            if (ReferenceManager.Player.PlayerData._iMoney < _price)
            {
                Debug.Log("pas assez d'argent");
                return;
            }
            ReferenceManager.Player.PlayerData._iMoney -= _price;
            ReferenceManager.Player.AddItem(_item);
            GetComponentInChildren<TextMeshProUGUI>().text = "Sold";
            GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();

        });
    }
}
