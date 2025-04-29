using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region Animator
    [Header("Référence Animator")]
    [SerializeField] private Animator m_animator;
    #endregion

    private void Awake()
    {
        if (m_animator == null)
            m_animator = GetComponent<Animator>();

        if (m_animator != null)
            m_animator.SetTrigger("PlayerTriggerIdle");
        else
            Debug.LogError(" Aucun Animator trouvé sur " + gameObject.name);
    }

    public void PlayIdle() => SetTrigger("PlayerTriggerIdle");

    public void PlayBlock() => SetTrigger("PlayerTriggerBlock");

    public void PlayDeath() => SetTrigger("PlayerTriggerDeath");

    public void PlayPowerAttack() => SetTrigger("PlayerTriggerPower");

    public void PlayLightAttack(float cooldown)
        => SetTriggerWithDuration("PlayerTriggerLight", cooldown);

    public void PlayMediumAttack(float cooldown)
        => SetTriggerWithDuration("PlayerTriggerMedium", cooldown);

    public void PlayHeavyAttack(float cooldown)
        => SetTriggerWithDuration("PlayerTriggerHeavy", cooldown);

    #region Core Trigger Logic

    private void SetTrigger(string triggerName)
    {
        if (m_animator == null)
        {
            Debug.LogWarning("Animator manquant pour l’animation " + triggerName);
            return;
        }

        m_animator.ResetTrigger("PlayerTriggerIdle"); 
        m_animator.SetTrigger(triggerName);
    }

    private void SetTriggerWithDuration(string triggerName, float duration)
    {
        if (m_animator == null) return;

        AnimationClip clip = GetAnimationClipForTrigger(triggerName);
        if (clip == null)
        {
            m_animator.SetTrigger(triggerName);
            return;
        }

       
        float speedMultiplier = duration < clip.length ? clip.length / duration : 1f;
        m_animator.speed = speedMultiplier;

        m_animator.SetTrigger(triggerName);

        StartCoroutine(ResetSpeedAfter(clip.length / speedMultiplier));
    }

    private IEnumerator ResetSpeedAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_animator.speed = 1f;
    }

    #endregion

    #region Clip Finder
    private AnimationClip GetAnimationClipForTrigger(string triggerName)
    {
        if (m_animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning(" Animator Controller non assigné");
            return null;
        }

        foreach (var clip in m_animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.ToLower().Contains(triggerName.ToLower().Replace("playertrigger", "")))
            {
                return clip;
            }
        }

        Debug.LogWarning(" Aucun clip trouvé pour le trigger : " + triggerName);
        return null;
    }
    #endregion
}
