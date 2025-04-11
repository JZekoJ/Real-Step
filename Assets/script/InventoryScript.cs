using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryScript : MonoBehaviour
{
    public GameObject _PrefabUI;
    public GameObject _InventoryContent;
    public GameObject _SortedInventoryContent;
    public GameObject _ItemInfo;

    public Dictionary<ItemType, GameObject> ItemSlotUI = new Dictionary<ItemType, GameObject>();

    private ItemType _SeletedItemType;

    private void Awake()
    {
        ReferenceManager.SaveLoader.OnLoadSave.AddListener(InitInventory);
    }

    #region PlayerInventory
    public void InitInventory()
    {
        foreach (Item obj in ReferenceManager.Player.PlayerData._itemList)
        {
            GameObject UIBtn = Instantiate(_PrefabUI, _InventoryContent.transform);
            UIBtn.GetComponent<ItemUIScript>().Init(obj);

        }
    }
    #endregion

    private void LoadSortInventory()
    {
        foreach (Transform child in _InventoryContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Item obj in ReferenceManager.Player.PlayerData._itemList)
        {
            if (obj._Type == _SeletedItemType)
            {
                GameObject UIBtn = Instantiate(_PrefabUI, _InventoryContent.transform);
                UIBtn.GetComponent<ItemUIScript>()._EquipItem = obj;
                UIBtn.GetComponent<ItemUIScript>().Init();
                UIBtn.GetComponent<Button>().onClick.AddListener(() => OpenItemInfo(obj));
            }
        }
    }

    private void OpenItemInfo(Item item)
    {
        _ItemInfo.SetActive(true);
        Button[] listButton = _ItemInfo.GetComponentsInChildren<Button>();
        _ItemInfo.GetComponentInChildren<ItemUIScript>()._EquipItem = item;
        _ItemInfo.GetComponentInChildren<ItemUIScript>().Init();
        foreach (Button button in listButton)
        {
            if (button.name == "Equip")
            {
                button.onClick.AddListener(() => ReferenceManager.Player.EquipItem(item));
            }
        }
    }

    public void SetSort(int i)
    {
        _SeletedItemType = (ItemType)i;
        LoadSortInventory();
    }


    //public void OpenInventory(int type)//pas récupérable sur un bouton
    //{
    //    for (int i = 0; i < listSelected.Count; i++)
    //    {
    //        Destroy(listSelected[i]);
    //    }
    //    listSelected.Clear();

    //    foreach (ScriptableWeapon obj in list)
    //    {
    //        if (obj.type == (Weapon_Type)type)
    //        {
    //            GameObject go = Instantiate(prefabUI, content.transform);
    //            go.GetComponent<Weapon>().m_weapon = obj;
    //            go.GetComponent<Weapon>().Init();
    //            go.GetComponent<Button>().onClick.AddListener(() => Player.Equip(go.GetComponent<Weapon>().m_weapon));
    //            listSelected.Add(go);
    //        }
    //    }
    //}


}
