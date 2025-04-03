using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int _iLevel;
    public int _iHp;

    public PlayerData(PlayerScript player) 
    {
        _iLevel = player._iLevel;
        _iHp = player._iHp;
    }
}

public class InventoryData
{

}
