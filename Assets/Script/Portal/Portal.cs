using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private List<ScriptablePortal> m_ScriptObjList;

    private ScriptablePortal m_ScriptObj;
    [Header("UI")]
    public GameObject m_goPortalUIPrefab;

    private TextMeshProUGUI m_tDifficulty;
    private GameObject m_goPortalUI;
    private Button m_bEnterDungeon;
    private Button m_bCloseWindow;

    private SpawnerPortal spawnerPortal;
    private SceneChange Scenechange;

    void Start()
    {


        m_ScriptObj = m_ScriptObjList[Random.Range(0, m_ScriptObjList.Count)];
        Scenechange = (SceneChange)FindAnyObjectByType(typeof(SceneChange));
        spawnerPortal = (SpawnerPortal)FindAnyObjectByType(typeof(SpawnerPortal));
        GetComponent<MeshRenderer>().material = m_ScriptObj.m_mMaterial;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (!spawnerPortal.m_bPortalOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_goPortalUI = Instantiate(m_goPortalUIPrefab);
                m_bEnterDungeon = m_goPortalUI.GetComponentsInChildren<Button>()[0];
                m_bCloseWindow = m_goPortalUI.GetComponentsInChildren<Button>()[1];
                m_tDifficulty = m_goPortalUI.GetComponentInChildren<TextMeshProUGUI>();

                m_bEnterDungeon.onClick.AddListener(() => Scenechange.ChangeScene("HackNSlash - Test"));
                m_bCloseWindow.onClick.AddListener(CloseWindow);
                m_tDifficulty.text = "Difficulty : " + m_ScriptObj.m_iDificulty.ToString();
                //m_bWindowOpen = true;
                spawnerPortal.m_bPortalOpen = true;

            }
        }
        
    }

    public void CloseWindow()
    {
        Destroy(m_goPortalUI);
        spawnerPortal.m_bPortalOpen = false;
    }
}
