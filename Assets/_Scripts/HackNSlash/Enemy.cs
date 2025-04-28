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
    private Animator m_animator;

    #endregion

    //—------Unity Events—----
    private void Awake()
    {
        m_fCurrentHealth = m_fMaxHealth;

        if (m_eOnDeath == null)
            m_eOnDeath = new UnityEvent();

        if(!m_bIsBoss)
        {
            m_sSlider = GetComponentInChildren<Slider>();
            m_sSlider.maxValue = m_fMaxHealth;
        }

        m_animator = GetComponent<Animator>();

        // On commence en Idl
        m_animator.SetTrigger("TriggerIdle");
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

        m_animator.SetTrigger("TriggerMove"); 

        m_cAttackRoutine = StartCoroutine(AttackLoop());
    }

    public void TakeDamage(float fDamage)
    {
        m_fCurrentHealth -= fDamage;
        m_animator.SetTrigger("TriggerTakeDamage"); 

        Debug.Log(name + " a pris " + fDamage + " dégâts. HP restants: " + m_fCurrentHealth + "/" + m_fMaxHealth);

        if (m_fCurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ReturnToIdleAfterDelay(0.5f));
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
            m_animator.SetTrigger("TriggerAttack"); 
            m_csPlayer.ReceiveDamage(m_fAttackPower);

            yield return new WaitForSeconds(m_fAttackInterval);

            m_animator.SetTrigger("TriggerIdle"); 
        }
    }

    private void Die()
    {
        m_animator.SetTrigger("TriggerDeath");

        if (m_cAttackRoutine != null)
            StopCoroutine(m_cAttackRoutine);

        StartCoroutine(DelayedDeath());
    }

    private IEnumerator ReturnToIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_animator.SetTrigger("TriggerIdle");
    }

    /*private void PlayAnimation(string triggerName)
    {
        if (CompareTag("Boss"))
        {
            triggerName = "Boss" + triggerName;
        }

        m_animator.SetTrigger(triggerName);
    }*/


    private IEnumerator DelayedDeath()
    {
        float deathAnimDuration = GetClipLength("Death"); // le nom de ton clip d'anim

        if (deathAnimDuration <= 0f)
            deathAnimDuration = 2.5f; // fallback si le clip n’est pas trouvé

        yield return new WaitForSeconds(deathAnimDuration);

        m_eOnDeath.Invoke();
        Destroy(gameObject);
    }

    private float GetClipLength(string clipName)
    {
        foreach (AnimationClip clip in m_animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.ToLower().Contains(clipName.ToLower()))
            {
                return clip.length;
            }
        }

        Debug.LogWarning("Animation 'Death' non trouvée !");
        return 0f;
    }
    #endregion
}
