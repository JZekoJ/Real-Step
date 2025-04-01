using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Portals")]
public class ScriptablePortal : ScriptableObject
{
    [Header("Data")]

    [SerializeField] public int m_iIndex;

    [SerializeField] public int m_iDificulty;

    [SerializeField] public Material m_mMaterial;
}
