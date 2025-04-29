using System.Collections;
using UnityEngine;
using FightSysteme;
using UnityEngine.UI;

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
    [SerializeField] private PlayerScript m_playerScript;
    #endregion

    #region Stats Joueur
    [Header("Stats Joueur")]
    [SerializeField] public int m_iPv;

    public Slider m_sSlider;
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
    //private PlayerScript m_playerScript;

    private Vector2 m_vStartPosition;
    private float m_fStartTime;
    private Vector2 m_vEndPosition;
    private float m_fEndTime;
    private bool m_bIsBlocking = false;

    private float m_fGlobalDelayUntil = 0f;

    private Coroutine m_cTrailCoroutine;
    private Coroutine m_cShieldCoroutine;
    
    private bool isAlive = true;

    
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
        

        m_playerAnim?.PlayIdle();
    }
    private void Start()
    {
        ReferenceManager.SaveLoader.OnLoadSave.AddListener(() => {
            m_iPv = m_playerScript._iPlayerHp();
            m_sSlider.maxValue = m_iPv;
            m_sSlider.value = m_iPv;
        });
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
            m_goTrail.transform.position = m_csInputManager.PrimaryPosition();
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
    public void ReceiveDamage(int iEnemyAttack)
    {
        if (m_bIsBlocking)
        {
            Debug.Log("🛡️ Le joueur bloque les dégâts !");
            return;
        }

        int iPv = m_iPv;

        int iAmount = m_csCombatSystem.CalculerDegats(iPv, iEnemyAttack, m_playerScript._iPlayerDefense());

        //m_playerAnim?.PlayTakeDamage();
        m_iPv = iAmount;
        m_iPv = Mathf.Max(0, m_iPv);
        Debug.Log($"🟥 Le joueur prend {iAmount} dégâts. PV restants : {m_iPv}");
        m_sSlider.value = m_iPv;

        if (m_iPv <= 0 && isAlive)
        {
            Debug.Log("☠️ Le joueur est KO !");
            isAlive = false;
            m_playerAnim?.PlayDeath();
        }
    }
    #endregion

    #region Attaque
    private void SwipeDirection(Vector2 vDirection)
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
            int fDegats = m_csCombatSystem.GetDegatsInfliges(m_playerScript._iPlayerAttack(), m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Legere);
            m_csCurrentEnemy.TakeDamage(fDegats);
            ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
            SetGlobalDelay(m_fCooldownLegere);
            m_playerAnim.PlayLightAttack(m_fCooldownLegere);
            Debug.Log("⚔️ Attaque légère !");
            Vibrator.Vibrate(100);
        }
        else if (Vector2.Dot(Vector2.down, vDirection) > m_fDirectionTreshold)
        {
            int fDegats = m_csCombatSystem.GetDegatsInfliges(m_playerScript._iPlayerAttack(), m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Lourde);
            m_csCurrentEnemy.TakeDamage(fDegats);
            ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
            SetGlobalDelay(m_fCooldownLourde);
            m_playerAnim.PlayHeavyAttack(m_fCooldownLourde);
            Debug.Log("💥 Attaque lourde !");
            Vibrator.Vibrate(300);
        }
        else if (Vector2.Dot(Vector2.right, vDirection) > m_fDirectionTreshold)
        {
            int fDegats = m_csCombatSystem.GetDegatsInfliges(m_playerScript._iPlayerAttack(), m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Moyenne);
            m_csCurrentEnemy.TakeDamage(fDegats);
            ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
            SetGlobalDelay(m_fCooldownMoyenne);
            m_playerAnim.PlayMediumAttack(m_fCooldownMoyenne);
            Debug.Log("🥊 Attaque moyenne !");
            Vibrator.Vibrate(200);
        }
        else if (Vector2.Dot(Vector2.left, vDirection) > m_fDirectionTreshold)
        {
            Debug.Log("⬅️ Swipe gauche — aucune attaque");
        }

        Debug.Log("❤️ PV de l'ennemi : " + m_csCurrentEnemy.GetHealth());
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
