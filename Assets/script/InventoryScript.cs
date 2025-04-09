using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public GameObject _PrefabUI;
    public GameObject _InventoryContent;


    private void Start()
    {
        Debug.Log("test2");
        ReferenceManager.SaveLoader.OnLoadSave.AddListener(InitInventory); 
    }
    public void InitInventory()
    {
        Debug.Log("test");
        foreach (Item obj in ReferenceManager.Player.PlayerData._itemList)
        {
            GameObject UIBtn = Instantiate(_PrefabUI, _InventoryContent.transform);
            UIBtn.GetComponent<ItemUIScript>().Init(obj);
        }
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
