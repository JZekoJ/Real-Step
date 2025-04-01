using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private ScriptablePortal m_ScriptObj;
    [Header("UI")]
    public GameObject m_goPortalUIPrefab;
    private GameObject m_goPortalUI;
    private Button m_bEnterDungeon;
    private Button m_bCloseWindow;

    
    private SceneChange Scenechange;

    private bool m_bWindowOpen ;
    void Start()
    {



        Scenechange = (SceneChange)FindAnyObjectByType(typeof(SceneChange));
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (!m_bWindowOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_goPortalUI = Instantiate(m_goPortalUIPrefab);
                m_bEnterDungeon = m_goPortalUI.GetComponentsInChildren<Button>()[0];
                m_bCloseWindow = m_goPortalUI.GetComponentsInChildren<Button>()[1];

                m_bEnterDungeon.onClick.AddListener(() => Scenechange.ChangeScene("HackNSlash - Test"));
                m_bCloseWindow.onClick.AddListener(CloseWindow);
                m_bWindowOpen = true;
            }
        }
        
    }

    public void CloseWindow()
    {
        Destroy(m_goPortalUI);
        m_bWindowOpen = false;
    }
}
