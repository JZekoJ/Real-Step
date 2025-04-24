using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class InventoryScript : MonoBehaviour
{
    public GameObject _PrefabUI;
    public GameObject _InventoryContent;
    public GameObject _SortedInventoryContent;
    public GameObject _ItemInfo;

    public List<CinemachineVirtualCamera> _VirtualCams;
    private CinemachineVirtualCamera _ActiveCam;

    public List<GameObject> _Slots;

    private ItemType _SeletedItemType;

    private void Awake()
    {
        //ReferenceManager.SaveLoader.OnLoadSave.AddListener(InitInventory);
    }

    #region PlayerInventory
    public void InitInventory()
    {
        foreach (Item obj in ReferenceManager.Player.PlayerData._itemList)
        {
            GameObject UIBtn = Instantiate(_PrefabUI, _InventoryContent.transform);
            UIBtn.GetComponent<ItemUIScript>()._EquipItem = obj;
            UIBtn.GetComponent<ItemUIScript>().Init();
            if (obj._bIsEquiped)
            {
                _Slots[(int)obj._Type].GetComponent<ItemUIScript>()._EquipItem = obj;
                _Slots[(int)obj._Type].GetComponent<ItemUIScript>().Init();
            }
        }
    }
    #endregion

    private void LoadSortInventory()
    {
        foreach (Transform child in _SortedInventoryContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Item obj in ReferenceManager.Player.PlayerData._itemList)
        {
            if (obj._Type == _SeletedItemType)
            {
                GameObject UIBtn = Instantiate(_PrefabUI, _SortedInventoryContent.transform);
                UIBtn.GetComponent<ItemUIScript>()._EquipItem = obj;
                UIBtn.GetComponent<ItemUIScript>().Init();
                UIBtn.GetComponent<Button>().onClick.AddListener(() => OpenItemInfo(obj));
            }
        }
    }

    private void OpenItemInfo(Item item)
    {
        if (_ItemInfo.activeSelf == false)
        {
            _ItemInfo.SetActive(true);
        }
        
        _ItemInfo.GetComponentInChildren<ItemUIScript>()._EquipItem = item;
        _ItemInfo.GetComponentInChildren<ItemUIScript>().Init();

        Button[] listButton = _ItemInfo.GetComponentsInChildren<Button>();
        foreach (Button button in listButton)
        {
            if (button.name == "Equip")
            {
                button.onClick.RemoveAllListeners();
                if (item._bIsEquiped)
                {
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";
                    button.onClick.AddListener(() => {
                        ReferenceManager.Player.UnequipItem(item);
                        _Slots[(int)item._Type].GetComponent<ItemUIScript>()._EquipItem = null;
                        _Slots[(int)item._Type].GetComponent<ItemUIScript>().Init();
                        OpenItemInfo(item);
                    });
                }
                else
                {
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
                    button.onClick.AddListener(() => {
                        ReferenceManager.Player.EquipItem(item);
                        _Slots[(int)item._Type].GetComponent<ItemUIScript>()._EquipItem = item;
                        _Slots[(int)item._Type].GetComponent<ItemUIScript>().Init();
                        OpenItemInfo(item);
                    });
                }
            }
        }
    }

    public void SetSort(int i)
    {
        _SeletedItemType = (ItemType)i;
        
        LoadSortInventory();

        _VirtualCams[i].gameObject.SetActive(true);
        if (_ActiveCam == null)
        {
            _ActiveCam = _VirtualCams[i];
            return;
        }
        _ActiveCam.gameObject.SetActive(false);
        _ActiveCam = _VirtualCams[i];
    }

}
