using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class Portal : MonoBehaviour
{
    private float maxClickDuration = 0.2f;

    private float mouseDownTime;



    #region Scriptable Settings
    [SerializeField] private List<ScriptablePortal> m_lScriptablePortals;
    private ScriptablePortal m_csSelectedPortal;
    #endregion

    #region UI
    [Header("UI")]
    public GameObject m_goPortalUIPrefab;
    private TextMeshProUGUI m_tmpDifficulty;
    private GameObject m_goPortalUIInstance;
    private Button m_btEnterDungeon;
    private Button m_btCloseWindow;
    #endregion

    #region Références
    private Tools m_csTools;
    private bool m_bCanEnter = false;
    #endregion

    //—------Unity Events—----
    private void Start()
    {
        m_csSelectedPortal = m_lScriptablePortals[Random.Range(0, m_lScriptablePortals.Count)];
        m_csTools = FindAnyObjectByType<Tools>();
        ExposedProperty m_MyProperty = "Color";

        GetComponent<VisualEffect>().SetVector4(m_MyProperty, m_csSelectedPortal.m_cColor);
        //GetComponent<MeshRenderer>().material = m_csSelectedPortal.m_mMaterial;

    }

    private void Update()
    {
        // Rien pour le moment
    }
    //—------------------

    //—------Interaction Souris—----
    private void OnMouseOver()
    {
        if (!m_csTools.IsPointerOverUIElement())
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDownTime = Time.time;
            }
            if (Input.GetMouseButtonUp(0))
            {
                float clickDuration = Time.time - mouseDownTime;
                if (clickDuration <= maxClickDuration)
                {
                    m_goPortalUIInstance = Instantiate(m_goPortalUIPrefab);
                    m_btEnterDungeon = m_goPortalUIInstance.GetComponentsInChildren<Button>()[0];
                    m_btCloseWindow = m_goPortalUIInstance.GetComponentsInChildren<Button>()[1];
                    m_tmpDifficulty = m_goPortalUIInstance.GetComponentInChildren<TextMeshProUGUI>();

                    m_btEnterDungeon.onClick.AddListener(() =>
                    {
                        PortalEnemies portalEnemies = GetComponent<PortalEnemies>();
                        if (portalEnemies != null)
                        {
                            DungeonTransfer.EnemiesToSpawn = portalEnemies.GetGeneratedEnemies();
                        }

                        m_csTools.ChangeScene("HackNSlash");
                    });

                    m_btCloseWindow.onClick.AddListener(CloseWindow);
                    m_tmpDifficulty.text = "Difficulty : " + m_csSelectedPortal.m_iDifficulty.ToString();
                }
            }
        }
    }

    public void CloseWindow()
    {
        Destroy(m_goPortalUIInstance);
    }
    //—------------------

    //—------Détection Trigger—----
    private void OnTriggerEnter(Collider cOther)
    {
        if (cOther.CompareTag("DetectionBox"))
        {
            m_bCanEnter = true;
        }
    }

    private void OnTriggerExit(Collider cOther)
    {
        if (cOther.CompareTag("DetectionBox"))
        {
            m_bCanEnter = false;
        }
    }
    //—------------------
}
