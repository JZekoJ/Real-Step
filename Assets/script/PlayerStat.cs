using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{

    private int m_iVitality{ get; set; }
    private int m_iStrenght = 10;
    private int m_iActualStrenght { get; set; }
    private int m_iMind { get; set; }
    private int m_iIntelligence { get; set; }
    private int m_iDexterity { get; set; }
    private int m_iCharisma { get; set; }
    private float m_fExperience { get; set; }
    private int m_iLevel { get; set; }
    // sa liste d'équipement ?
    [Header("UI")]
    public Image m_wCurrentWeapon;
    public Image m_wCurrentHelmet; 
    public Image m_wCurrentBoot;

    public TextMeshProUGUI m_tStrenght;

    [Header("Scriptable")]
    [SerializeField]private ScriptableWeapon m_PlayerWeapon;
    [SerializeField] private ScriptableWeapon m_PlayerHelmet;
    [SerializeField] private ScriptableWeapon m_PlayerBoots;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //add equipment (faire un truc classique)

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelUp()
    {

    }

    public void ActualStrenght()
    {
        //est ce que je fais strenght + arme strnght = Actual strength ?
    }

    public void Equip(ScriptableWeapon obj)
    {
        //déséquipez l'actuelle + affichez le truc actuel + enlevez de l'inventaire? 
        //Debug.Log("euh");
        switch (obj.type)
        {
            case Weapon_Type.HELMET:
                m_PlayerHelmet = obj;
                break;
            case Weapon_Type.WEAPON:
                m_PlayerWeapon = obj;
                break;
            case Weapon_Type.BOOTS:
                m_PlayerBoots = obj;
                break;

        }
        Actualise();

    }
    
    public void Actualise()
    {
        m_wCurrentWeapon.sprite = m_PlayerWeapon.m_sSprite;
        m_wCurrentHelmet.sprite = m_PlayerHelmet.m_sSprite;
        m_wCurrentBoot.sprite = m_PlayerBoots.m_sSprite;

        m_iActualStrenght = m_PlayerWeapon.m_iDamage + m_PlayerHelmet.m_iDamage+ m_PlayerBoots.m_iDamage + m_iStrenght;

        m_tStrenght.text = m_iActualStrenght.ToString();
    }
}
