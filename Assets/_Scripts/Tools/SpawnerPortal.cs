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
    #endregion

    //—------Unity Events—----
    private void Start()
    {
        // Initialisation si nécessaire
    }

    private void Update()
    {
        m_fTime += Time.deltaTime;
        if (m_fTime > 1f)
        {
            Vector3 vLocation = new Vector3(Random.Range(-m_fRange, m_fRange), 0.5f, Random.Range(-m_fRange, m_fRange));
            Vector3 vRotation = new Vector3(m_goPortal.transform.rotation.x, m_goPortal.transform.rotation.y, Random.Range(0f, 360f));
            m_goPortal.transform.Rotate(vRotation);
            Instantiate(m_goPortal, vLocation, m_goPortal.transform.rotation);
            m_fTime = 0f;
        }
    }
    //—------------------
}