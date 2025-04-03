using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Portals")]
public class ScriptablePortal : ScriptableObject
{
    [Header("Data")]

    [SerializeField] public int m_iIndex;

    [SerializeField] public int m_iDificulty;//peut être fait en enum ?

    [SerializeField] public Material m_mMaterial;

    //type d'ennemie possible
    //récompense possible ?
}
