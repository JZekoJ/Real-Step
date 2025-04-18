using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    #region Unity Parameters
    [Header("Paramètres de l'animation de dégâts")]
    [SerializeField] private float m_fFloatSpeed = 1f;
    [SerializeField] private float m_fDuration = 1f;
    #endregion

    #region Private Variables
    private TextMeshPro m_tmTextMesh;
    #endregion

    //—------Unity Events—----
    private void Awake()
    {
        m_tmTextMesh = GetComponent<TextMeshPro>();
        Destroy(gameObject, m_fDuration);
    }

    private void Update()
    {
        transform.position += Vector3.up * m_fFloatSpeed * Time.deltaTime;

        if (Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
    //—------------------

    //—------public—----
    public void SetDamage(float fAmount)
    {
        if (m_tmTextMesh != null)
            m_tmTextMesh.text = "-" + fAmount.ToString();
    }
    //—------------------
}
