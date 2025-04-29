using System.Collections;
using UnityEngine;
using FightSysteme;

public class SwipeDetection : MonoBehaviour
{
    #region Paramètres Swipe
    [Header("Paramètres de Swipe")]
    [SerializeField] private float m_fMinimumDistance = 0.2f;
    [SerializeField] private float m_fMaximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float m_fDirectionTreshold = 0.9f;
    [SerializeField] private float m_fLongPressDuration = 0.5f;
    [SerializeField] private float m_fLongPressMaxDuration = 2f;
    #endregion

    #region Références Externes
    [Header("Références Externes")]
    [SerializeField] private GameObject m_goTrail;
    [SerializeField] private FightSystem m_csCombatSystem;
    [SerializeField] private GameObject m_goFloatingDamagePrefab;
    [SerializeField] private PlayerAnimation m_playerAnim;
    private Tools m_csTools;
    #endregion

    #region Stats Joueur
    [Header("Stats Joueur")]
    [SerializeField] public float m_fPv = 100f;
    [SerializeField] private float m_fAttaque = 20f;
    [SerializeField] private float m_fDefense = 10f;
    #endregion

    #region Cooldowns
    [Header("Cooldowns des Attaques")]
    [SerializeField] private float m_fCooldownLegere = 1f;
    [SerializeField] private float m_fCooldownMoyenne = 2f;
    [SerializeField] private float m_fCooldownLourde = 3f;
    #endregion

    #region Private Variables
    private InputManager m_csInputManager;
    private Enemy m_csCurrentEnemy;

    private Vector2 m_vStartPosition;
    private float m_fStartTime;
    private Vector2 m_vEndPosition;
    private float m_fEndTime;
    private bool m_bIsBlocking = false;

    private float m_fGlobalDelayUntil = 0f;

    private Coroutine m_cTrailCoroutine;
    private Coroutine m_cShieldCoroutine;
    #endregion

    #region Public
    public void SetCurrentEnemy(Enemy csEnemy)
    {
        m_csCurrentEnemy = csEnemy;
    }
    #endregion

    #region Unity Events
    private void Awake()
    {
        m_csInputManager = InputManager.Instance;
        m_csCombatSystem = GetComponent<FightSystem>();
        if (m_playerAnim == null)
            m_playerAnim = GetComponentInChildren<PlayerAnimation>();
        m_csTools = FindAnyObjectByType<Tools>();
        m_playerAnim?.PlayIdle();
    }

    private void OnEnable()
    {
        m_csInputManager.OnStartTouch += SwipeStart;
        m_csInputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        m_csInputManager.OnStartTouch -= SwipeStart;
        m_csInputManager.OnEndTouch -= SwipeEnd;
    }
    #endregion

    #region Swipe Logic
    private void SwipeStart(Vector2 vPosition, float fTime)
    {
        m_vStartPosition = vPosition;
        m_fStartTime = fTime;

        m_goTrail.GetComponent<TrailParticleController>()?.PlayTrailAndParticles();
        m_cTrailCoroutine = StartCoroutine(Trail());
        StartCoroutine(LongPressDetection());
    }

    private IEnumerator Trail()
    {
        while (true)
        {
            if (!m_csTools.IsPointerOverUIElement())
            {
                m_goTrail.transform.position = m_csInputManager.PrimaryPosition();
            }
            yield return null;
        }
    }

    private void SwipeEnd(Vector2 vPosition, float fTime)
    {
        StopCoroutine(m_cTrailCoroutine);
        m_goTrail.GetComponent<TrailParticleController>()?.StopTrailAndParticles();

        if (m_bIsBlocking)
        {
            m_bIsBlocking = false;
            if (m_cShieldCoroutine != null) StopCoroutine(m_cShieldCoroutine);
            Debug.Log("🛡️ Bouclier désactivé (fin de touch) !");
            return;
        }

        m_vEndPosition = vPosition;
        m_fEndTime = fTime;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(m_vStartPosition, m_vEndPosition) >= m_fMinimumDistance && (m_fEndTime - m_fStartTime) <= m_fMaximumTime)
        {
            Debug.DrawLine(m_vStartPosition, m_vEndPosition, Color.red, 5f);
            Vector3 vDirection = m_vEndPosition - m_vStartPosition;
            Vector2 vDirection2D = new Vector2(vDirection.x, vDirection.y).normalized;
            SwipeDirection(vDirection2D);
        }
        else
        {
            Debug.Log("❌ Swipe trop court ou trop lent");
        }

        ResetSwipe();
    }

    private void ResetSwipe()
    {
        m_vStartPosition = Vector2.zero;
        m_vEndPosition = Vector2.zero;
        m_fStartTime = 0f;
        m_fEndTime = 0f;
    }
    #endregion

    #region Cooldown Logic
    private bool IsInGlobalDelay()
    {
        return Time.time < m_fGlobalDelayUntil;
    }

    private void SetGlobalDelay(float delay)
    {
        m_fGlobalDelayUntil = Time.time + delay;
    }
    #endregion

    #region Défense
    private void ActivateShield()
    {
        if (IsInGlobalDelay())
        {
            Debug.Log("⛔ Bouclier en cooldown !");
            return;
        }

        m_bIsBlocking = true;
        Debug.Log("🛡️ Bouclier activé !");
        SetGlobalDelay(1f);

        m_playerAnim?.PlayBlock();

        if (m_cShieldCoroutine != null)
            StopCoroutine(m_cShieldCoroutine);
        m_cShieldCoroutine = StartCoroutine(ShieldDuration());
    }

    private IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(2f);
        if (m_bIsBlocking)
        {
            m_bIsBlocking = false;
            Debug.Log("🛡️ Bouclier désactivé automatiquement après 2s !");
        }

        m_playerAnim?.PlayIdle();
    }

    private IEnumerator LongPressDetection()
    {
        float fTimer = 0f;

        while (fTimer < m_fLongPressDuration)
        {
            if (Vector2.Distance(m_csInputManager.PrimaryPosition(), m_vStartPosition) > 0.1f)
            {
                yield break;
            }
            fTimer += Time.deltaTime;
            yield return null;
        }

        ActivateShield();
    }
    #endregion

    #region Dégâts
    public void ReceiveDamage(float fAmount)
    {
        if (m_bIsBlocking)
        {
            Debug.Log("🛡️ Le joueur bloque les dégâts !");
            return;
        }
        //m_playerAnim?.PlayTakeDamage();
        m_fPv -= fAmount;
        m_fPv = Mathf.Max(0, m_fPv);
        Debug.Log($"🟥 Le joueur prend {fAmount} dégâts. PV restants : {m_fPv}");

        if (m_fPv <= 0)
        {
            Debug.Log("☠️ Le joueur est KO !");
            m_playerAnim?.PlayDeath();
        }
    }
    #endregion

    #region Attaque
    private void SwipeDirection(Vector2 vDirection)
    {
        if (!m_csTools.IsPointerOverUIElement())
        {
            if (m_csCombatSystem == null)
            {
                Debug.LogError("CombatSystem non assigné !");
                return;
            }

            if (m_csCurrentEnemy == null)
            {
                Debug.Log("❌ Aucun ennemi actif !");
                return;
            }

            if (IsInGlobalDelay())
            {
                Debug.Log("⏳ Cooldown en cours !");
                return;
            }

            if (Vector2.Dot(Vector2.up, vDirection) > m_fDirectionTreshold)
            {
                float fDegats = m_csCombatSystem.GetDegatsInfliges(m_fAttaque, m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Legere);
                m_csCurrentEnemy.TakeDamage(fDegats);
                ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
                SetGlobalDelay(m_fCooldownLegere);
                m_playerAnim.PlayLightAttack(m_fCooldownLegere);
                Debug.Log("⚔️ Attaque légère !");
            }
            else if (Vector2.Dot(Vector2.down, vDirection) > m_fDirectionTreshold)
            {
                float fDegats = m_csCombatSystem.GetDegatsInfliges(m_fAttaque, m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Lourde);
                m_csCurrentEnemy.TakeDamage(fDegats);
                ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
                SetGlobalDelay(m_fCooldownLourde);
                m_playerAnim.PlayHeavyAttack(m_fCooldownLourde);
                Debug.Log("💥 Attaque lourde !");
            }
            else if (Vector2.Dot(Vector2.right, vDirection) > m_fDirectionTreshold)
            {
                float fDegats = m_csCombatSystem.GetDegatsInfliges(m_fAttaque, m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Moyenne);
                m_csCurrentEnemy.TakeDamage(fDegats);
                ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
                SetGlobalDelay(m_fCooldownMoyenne);
                m_playerAnim.PlayMediumAttack(m_fCooldownMoyenne);
                Debug.Log("🥊 Attaque moyenne !");
            }
            else if (Vector2.Dot(Vector2.left, vDirection) > m_fDirectionTreshold)
            {
                Debug.Log("⬅️ Swipe gauche — aucune attaque");
            }

            Debug.Log("❤️ PV de l'ennemi : " + m_csCurrentEnemy.GetHealth());
        }
    }

    private void ShowFloatingDamage(float fAmount, Vector3 vPosition)
    {
        if (m_goFloatingDamagePrefab != null)
        {
            GameObject goDamageText = Instantiate(m_goFloatingDamagePrefab, vPosition, Quaternion.identity);
            FloatingDamageText csFloating = goDamageText.GetComponent<FloatingDamageText>();
            if (csFloating != null)
            {
                csFloating.SetDamage(fAmount);
            }
        }
    }
    #endregion
}
