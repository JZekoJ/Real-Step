using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Portals")]
public class ScriptablePortal : ScriptableObject
{
    #region Données du Portail
    [Header("Données")]
    [SerializeField] public int m_iIndex;
    [SerializeField] public int m_iDifficulty; // TODO: Peut être transformé en enum
    //[SerializeField] public Material m_mMaterial;
    [SerializeField] private Color m_cColor;
    #endregion

    // TODO: Ajouter type d'ennemis possibles ?
    // TODO: Ajouter récompense possible ?
}