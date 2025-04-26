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

    #region UI
    [SerializeField] private GameObject m_gStatPrefab;
    [SerializeField] private GameObject m_gStatParent;
    #endregion

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
        foreach (RectTransform rtStat in m_gStatParent.GetComponentsInChildren<RectTransform>())
        {
            if(rtStat != m_gStatParent.GetComponentsInChildren<RectTransform>()[0])
            {
                Destroy(rtStat.gameObject);
            }
            
        }

        for (int i = 0; i < item._Stats.Count; i++)
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
            GameObject textStat = Instantiate(m_gStatPrefab, m_gStatParent.transform);
            textStat.GetComponentsInChildren<TextMeshProUGUI>()[0].text = stat + item._Stats[i].ToString();
            int diff = item._Stats[i];
            if (_Slots[(int)item._Type].GetComponent<ItemUIScript>()._EquipItem != null)
            {
                //ReferenceManager.Player.PlayerData.ItemSlot[(int)item._Type]._bIsEquiped = false;
                Debug.Log("???");
                diff = item._Stats[i] - _Slots[(int)item._Type].GetComponent<ItemUIScript>()._EquipItem._Stats[i];// ReferenceManager.Player.PlayerData.ItemSlot[(int)item._Type]._Stats[i]
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
