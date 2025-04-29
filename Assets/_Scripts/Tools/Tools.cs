using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Tools : MonoBehaviour
{
    #region Private Variables
    private int m_iUILayer;

    public static bool m_bOpenInv;
    public static bool m_bOpenShop;
    #endregion

    //—------Unity Events—----
    private void Start()
    {
        m_iUILayer = LayerMask.NameToLayer("UI");
    }

    private void Update()
    {
        // Debug.Log(IsPointerOverUIElement() ? "Over UI" : "Not over UI");
    }
    //—------------------

    #region Public Functions
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    public static Vector3 ScreenToWorld(Camera camCamera, Vector3 vPosition)
    {
        vPosition.z = 10f;
        return camCamera.ScreenToWorldPoint(vPosition);
    }

    public void ChangeScene(string sSceneName)
    {
        Tools.m_bOpenInv = false;
        Tools.m_bOpenShop = false;
        SceneManager.LoadScene(sSceneName);
    }

    public void GoInventory()
    {
        Tools.m_bOpenInv = true;
    }

    public void GoShop()
    {
        Tools.m_bOpenShop = true;
    }
    #endregion

    #region Private Functions
    private bool IsPointerOverUIElement(List<RaycastResult> lRaycastResults)
    {
        foreach (RaycastResult rsResult in lRaycastResults)
        {
            if (rsResult.gameObject.layer == m_iUILayer)
                return true;
        }
        return false;
    }

    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> lRaycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, lRaycastResults);
        return lRaycastResults;
    }
    #endregion
}
