using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_goUIMenu;
    [SerializeField] private GameObject m_goUIInventory;
    [SerializeField] private GameObject m_goUIShop;
    // Start is called before the first frame update
    void Start()
    {
        if (Tools.m_bOpenInv)
        {
            m_goUIInventory.SetActive(true);
            m_goUIInventory.GetComponent<RectTransform>().localScale = Vector3.one;
            m_goUIMenu.SetActive(false);
        }

        if (Tools.m_bOpenShop)
        {
            m_goUIShop.SetActive(true);
            m_goUIShop.GetComponent<RectTransform>().localScale = Vector3.one;
            m_goUIMenu.SetActive(false);
        }
        Debug.Log(Tools.m_bOpenInv);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
