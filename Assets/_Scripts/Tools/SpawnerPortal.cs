using System.Collections;
using UnityEngine;

public class SpawnerPortal : MonoBehaviour
{
    #region Public Variables
    public GameObject m_goPortal;
    #endregion

    #region Serialized Fields
    [Header("Paramètres de spawn")]
    [SerializeField] private float m_fRange;
    #endregion

    #region Private Variables
    private float m_fTime;
    private int m_iAmount = 0;
    #endregion

    //—------Unity Events—----
    private void Start()
    {
        // Initialisation si nécessaire
    }

    private void Update()
    {
        m_fTime += Time.deltaTime;
        if (m_iAmount < 5)
        {
            if (m_fTime > 1f)
            {
                Vector3 vLocation = new Vector3(Random.Range(-290, 400), 0.5f, Random.Range(-250, 450));
                //Vector3 vRotation = new Vector3(m_goPortal.transform.rotation.x, m_goPortal.transform.rotation.y, Random.Range(0f, 360f));
                //m_goPortal.transform.Rotate(vRotation);
                Instantiate(m_goPortal, vLocation, m_goPortal.transform.rotation, transform);
                m_fTime = 0f;
                m_iAmount++;
            }
            
        }
        
    }
    //—------------------
}