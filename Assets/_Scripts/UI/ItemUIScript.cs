using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ItemUIScript : MonoBehaviour
{
    public Item _EquipItem;


    [SerializeField] private Sprite Empty;

    [SerializeField] private Sprite Rarity0;
    [SerializeField] private Sprite Rarity1;
    [SerializeField] private Sprite Rarity2;
    [SerializeField] private Sprite Rarity3;
    [SerializeField] private Sprite Rarity4;
    [SerializeField] private Sprite Rarity5;

    [SerializeField] private Sprite Weapon;
    [SerializeField] private Sprite Head;
    [SerializeField] private Sprite Chest;
    [SerializeField] private Sprite Boot;
    [SerializeField] private Sprite Legging;
    [SerializeField] private Sprite Gauntlet;

    public void Init()
    {
        if (_EquipItem == null)
        {
            GetComponent<Image>().sprite = Empty;
            GetComponentsInChildren<Image>()[1].sprite = null;
            GetComponentsInChildren<Image>()[1].color = Color.clear;
            return;
        }
        GetComponentsInChildren<Image>()[1].color = Color.white;
        switch (_EquipItem._Rarity)
        {
            case ItemRarity.E:
                GetComponent<Image>().sprite = Rarity0;
                break;
            case ItemRarity.D:
                GetComponent<Image>().sprite = Rarity1;
                break;
            case ItemRarity.C:
                GetComponent<Image>().sprite = Rarity2;
                break;
            case ItemRarity.B:
                GetComponent<Image>().sprite = Rarity3;
                break;
            case ItemRarity.A:
                GetComponent<Image>().sprite = Rarity4;
                break;
            case ItemRarity.S:
                GetComponent<Image>().sprite = Rarity5;
                break;

            default:
                break;
        }

        switch (_EquipItem._Type)
        {
            case ItemType.Weapon:
                GetComponentsInChildren<Image>()[1].sprite = Weapon;
                break;
            case ItemType.Head:
                GetComponentsInChildren<Image>()[1].sprite = Head;
                break;
            case ItemType.Chest:
                GetComponentsInChildren<Image>()[1].sprite = Chest;
                break;
            case ItemType.Boot:
                GetComponentsInChildren<Image>()[1].sprite = Boot;
                break;
            case ItemType.Legging:
                GetComponentsInChildren<Image>()[1].sprite = Legging;
                break;
            case ItemType.Gauntlet:
                GetComponentsInChildren<Image>()[1].sprite = Gauntlet;
                break;

            default:
                break;
        }

    }
}
