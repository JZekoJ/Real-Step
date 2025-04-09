using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ItemUIScript : MonoBehaviour
{

    [SerializeField] private Sprite Weapon;
    [SerializeField] private Sprite Head;
    [SerializeField] private Sprite Chest;
    [SerializeField] private Sprite Boot;
    [SerializeField] private Sprite Legging;
    [SerializeField] private Sprite Gauntlet;
    public void Init(Item i)
    {
        switch (i._Rarity)
        {
            case ItemRarity.Commun:
                GetComponent<Outline>().effectColor = Color.green;
                break;
            case ItemRarity.Rare:
                GetComponent<Outline>().effectColor = Color.blue;
                break;
            case ItemRarity.Epic:
                GetComponent<Outline>().effectColor = Color.magenta;
                break;
            case ItemRarity.Legendary:
                GetComponent<Outline>().effectColor = Color.yellow;
                break;

            default:
                break;
        }

        switch (i._Type)
        {
            case ItemType.Weapon:
                GetComponent<Image>().sprite = Weapon;
                break;
            case ItemType.Head:
                GetComponent<Image>().sprite = Head;
                break;
            case ItemType.Chest:
                GetComponent<Image>().sprite = Chest;
                break;
            case ItemType.Boot:
                GetComponent<Image>().sprite = Boot;
                break;
            case ItemType.Legging:
                GetComponent<Image>().sprite = Legging;
                break;
            case ItemType.Gauntlet:
                GetComponent<Image>().sprite = Gauntlet;
                break;

            default:
                break;
        }
    }
}
