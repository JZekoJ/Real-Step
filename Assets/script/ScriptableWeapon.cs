using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Weapon_Type
{
    HELMET,
    WEAPON,
    BOOTS
}

[CreateAssetMenu(menuName = "Scriptable Objects/Weapon")]
public class ScriptableWeapon : ScriptableObject
{
    [Header("Data")]

    [SerializeField] public int m_iIndex;

    public int m_iDamage;

    [SerializeField] private Sprite sprite;

    public Sprite m_sSprite { get { return sprite; } }

    public Weapon_Type type;
}
