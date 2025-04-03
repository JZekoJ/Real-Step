using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int _iLevel = 1;
    public int _iHp = 1;
    public string _sName = "";

    [SerializeField] 
    TextMeshProUGUI lvlUI;
    [SerializeField]
    TextMeshProUGUI hpUI;
    [SerializeField]
    TMP_InputField nameUI;

    public void SetData(PlayerData data)
    {
        _iLevel = data._iLevel;
        _iHp = data._iHp;

        UpdateUI();
    }
    public void ChangeLevel(int amount)
    {
        _iLevel += amount;
        UpdateUI();
    }
    public void ChangeHp(int amount)
    {
        _iHp += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        lvlUI.text = "lvl : " + _iLevel;
        hpUI.text = "hp : " + _iHp;
    }

}
