using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemToBuyUI : MonoBehaviour
{
    public Item _item;
    private int _price;

    #region UI
    public GameObject m_gStatUIParent;
    public GameObject m_gStatUIPrefab;
    public GameObject m_gImageItem;
    public GameObject m_gItemInfo;
    #endregion
    public void Init()
    {
        ReferenceManager.Player.PlayerData._iMoney += 10000;
        _item = ItemUtils.GenerateRandomItem();
        _price = ItemUtils.CalculateBuyPrice(_item);

        GetComponentInChildren<ItemUIScript>()._EquipItem = _item;
        GetComponentInChildren<ItemUIScript>().Init();

        GetComponentsInChildren<TextMeshProUGUI>()[1].text = "" + _price;

        Debug.Log(GetComponentsInChildren<Button>()[0].name);

        GetComponentsInChildren<Button>()[0].onClick.AddListener(()=>ShowStat());
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

    public void ShowStat() {

        m_gImageItem.GetComponentInChildren<ItemUIScript>()._EquipItem = _item;
        m_gImageItem.GetComponentInChildren<ItemUIScript>().Init();
        m_gItemInfo.SetActive(true);
        foreach (RectTransform rtStat in m_gStatUIParent.GetComponentsInChildren<RectTransform>())
        {
            if (rtStat != m_gStatUIParent.GetComponentsInChildren<RectTransform>()[0])
            {
                Destroy(rtStat.gameObject);
            }
        }

        for (int i = 0; i < _item._Stats.Count; i++)
        {

            string stat = "";
            switch ((ItemStats)i)
            {
                case ItemStats.Strength:
                    stat = "Strenght : ";
                    break;
                case ItemStats.Spirit:
                    stat = "Spirit : ";
                    break;
                case ItemStats.Intel:
                    stat = "Int : ";
                    break;
                case ItemStats.Vita:
                    stat = "Health : ";
                    break;
                case ItemStats.Char:
                    stat = "Char : ";
                    break;
                case ItemStats.Dext:
                    stat = "Dext : ";
                    break;
            }

            Debug.Log(i);
            GameObject textStat = Instantiate(m_gStatUIPrefab, m_gStatUIParent.transform);
            textStat.GetComponentsInChildren<TextMeshProUGUI>()[0].text = stat + _item._Stats[i].ToString();
            int diff = _item._Stats[i];
            
            if (ReferenceManager.Player.PlayerData.ItemSlot[(int)_item._Type] != null)
            {
                //ReferenceManager.Player.PlayerData.ItemSlot[(int)item._Type]._bIsEquiped = false;
                Debug.Log("???");
                diff = _item._Stats[i] - ReferenceManager.Player.PlayerData.ItemSlot[(int)_item._Type]._Stats[i];// ReferenceManager.Player.PlayerData.ItemSlot[(int)item._Type]._Stats[i]
            }
            string moreless = "";
            if (diff > 0)
            {
                moreless = " + ";
                textStat.GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.green;
            }
            if (diff == 0)
            {
                moreless = "";
                textStat.GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.white;
            }
            if (diff < 0)
            {
                moreless = "";
                textStat.GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.red;
            }

            textStat.GetComponentsInChildren<TextMeshProUGUI>()[1].text = moreless + diff.ToString();
        }
    }
}
