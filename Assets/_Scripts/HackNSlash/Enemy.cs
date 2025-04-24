using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Statistiques
    [Header("Statistiques")]
    [SerializeField] private float m_fMaxHealth = 100f;
    [SerializeField] private float m_fCurrentHealth;
    [SerializeField] private float m_fAttackPower = 10f;
    [SerializeField] private float m_fDefense = 5f;
    [SerializeField] public bool m_bIsBoss;
    #endregion

    #region UI
    public Slider m_sSlider;
    //public Slider m_sBossSlider;
    #endregion

    #region Attaque Auto
    [Header("Attaque automatique")]
    [SerializeField] private float m_fAttackInterval = 2f;
    #endregion

    #region Événements & Références
    public UnityEvent m_eOnDeath;
    private Coroutine m_cAttackRoutine;
    private SwipeDetection m_csPlayer;
    #endregion

    //—------Unity Events—----
    private void Awake()
    {
        m_fCurrentHealth = m_fMaxHealth;

        if (m_eOnDeath == null)
            m_eOnDeath = new UnityEvent();

        if (!m_bIsBoss)
        {
            m_sSlider = GetComponentInChildren<Slider>();
            m_sSlider.maxValue = m_fMaxHealth;
        }
        
    }

    private void Update()
    {
        m_sSlider.value = m_fCurrentHealth;
    }

    //—------------------

    #region Public Functions
    public void StartAttacking(SwipeDetection csPlayerTarget)
    {
        m_csPlayer = csPlayerTarget;

        if (m_cAttackRoutine != null)
            StopCoroutine(m_cAttackRoutine);

        m_cAttackRoutine = StartCoroutine(AttackLoop());
    }

    public void TakeDamage(float fDamage)
    {
        m_fCurrentHealth -= fDamage;

        Debug.Log(name + " a pris " + fDamage + " dégâts. HP restants: " + m_fCurrentHealth + "/" + m_fMaxHealth);

        if (m_fCurrentHealth <= 0)
        {
            Die();
        }
    }

    public float GetHealth() { return m_fCurrentHealth; }
    public float GetMaxHealth() { return m_fMaxHealth; }
    public float GetAttackPower() { return m_fAttackPower; }
    public float GetDefense() { return m_fDefense; }
    #endregion

    #region Private Functions
    private IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(m_fAttackInterval);

        while (m_csPlayer != null)
        {
            m_csPlayer.ReceiveDamage(m_fAttackPower);
            Debug.Log($"{name} attaque le joueur pour {m_fAttackPower} dégâts !");
            yield return new WaitForSeconds(m_fAttackInterval);
        }
    }

    private void Die()
    {
        Debug.Log(name + " est mort !");
        m_eOnDeath.Invoke();

        if (m_cAttackRoutine != null)
            StopCoroutine(m_cAttackRoutine);

        Destroy(gameObject);
    }
    #endregion
}
