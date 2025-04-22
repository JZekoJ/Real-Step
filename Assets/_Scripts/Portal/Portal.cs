using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Portal : MonoBehaviour
{
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
        m_csSelectedPortal = m_lScriptablePortals[Random.Range(0, 1)];
        m_csTools = FindAnyObjectByType<Tools>();
        //GetComponent<VisualEffect>().visualEffectAsset.
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
                m_goPortalUIInstance = Instantiate(m_goPortalUIPrefab);
                m_btEnterDungeon = m_goPortalUIInstance.GetComponentsInChildren<Button>()[0];
                m_btCloseWindow = m_goPortalUIInstance.GetComponentsInChildren<Button>()[1];
                m_tmpDifficulty = m_goPortalUIInstance.GetComponentInChildren<TextMeshProUGUI>();

                m_btEnterDungeon.onClick.AddListener(() => m_csTools.ChangeScene("HackNSlash"));
                m_btCloseWindow.onClick.AddListener(CloseWindow);
                m_tmpDifficulty.text = "Difficulty : " + m_csSelectedPortal.m_iDifficulty.ToString();
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
