using UnityEngine;

public class TrailParticleController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private ParticleSystem m_ParticleSystem;

    [Header("Vitesse")]
    public float m_SpeedMultiplier = 1f;
    public float m_MaxParticleSpeed = 5f;

    private Vector3 m_LastPosition;
    private float m_CurrentSpeed;
    private TrailRenderer m_Trail;

    private void Awake()
    {
        if (m_ParticleSystem == null)
        {
            m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
        }

        m_Trail = GetComponent<TrailRenderer>();

        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.Stop();
            m_ParticleSystem.Clear();
        }

        if (m_Trail != null)
        {
            m_Trail.Clear();
        }

        m_LastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 direction = currentPosition - m_LastPosition;
        float distance = direction.magnitude;
        m_CurrentSpeed = distance / Time.deltaTime;

        if (m_ParticleSystem != null && m_ParticleSystem.isPlaying)
        {
            var main = m_ParticleSystem.main;
            main.startSpeed = Mathf.Min(m_CurrentSpeed * m_SpeedMultiplier, m_MaxParticleSpeed);

            if (direction.sqrMagnitude > 0.001f)
            {
                m_ParticleSystem.transform.rotation = Quaternion.LookRotation(-direction);
            }
        }

        m_LastPosition = currentPosition;
    }

    public void PlayTrailAndParticles()
    {
        if (m_Trail != null)
        {
            m_Trail.Clear();
        }

        gameObject.SetActive(true);

        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.Clear();
            m_ParticleSystem.Play();
        }

        m_LastPosition = transform.position;
    }

    public void StopTrailAndParticles()
    {
        if (m_ParticleSystem != null)
        {
            m_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        gameObject.SetActive(false);
    }
}
