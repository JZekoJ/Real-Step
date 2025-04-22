using UnityEngine;

public class TrailParticleController : MonoBehaviour
{
    [Header("Référence au système de particules")]
    [SerializeField] private ParticleSystem m_ParticleSystem;

    private TrailRenderer m_Trail;

    private void Awake()
    {
        m_Trail = GetComponent<TrailRenderer>();

        if (m_ParticleSystem == null)
        {
            m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
        }

        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.Stop();
        }

        if (m_Trail != null)
        {
            m_Trail.Clear();
        }
    }

    public void PlayTrailAndParticles()
    {
        if (m_Trail != null)
        {
            m_Trail.Clear();
        }

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.Clear();
            m_ParticleSystem.Play();
        }
    }

    public void StopTrailAndParticles()
    {
        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
