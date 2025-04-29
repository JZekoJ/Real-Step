using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    #region Private Static
    private static T m_tInstance;
    #endregion

    #region Public Static
    public static T Instance
    {
        get
        {
            if (m_tInstance == null)
            {
                GameObject goObj = new GameObject
                {
                    name = typeof(T).Name,
                    hideFlags = HideFlags.HideAndDontSave
                };
                m_tInstance = goObj.AddComponent<T>();
            }
            return m_tInstance;
        }
    }
    #endregion

    //—------Unity Events—----
    protected virtual void Awake()
    {
        if (m_tInstance == null)
        {
            m_tInstance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (m_tInstance == this)
        {
            m_tInstance = null;
        }
    }
    //—------------------
}
