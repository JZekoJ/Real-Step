// Adapté à la nomenclature demandée
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
    #endregion

    #region Stats Joueur
    [Header("Stats Joueur")]
    [SerializeField] private float m_fPv = 100f;
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
    //—------private—----
    private InputManager m_csInputManager;
    private Enemy m_csCurrentEnemy;

    private Vector2 m_vStartPosition;
    private float m_fStartTime;
    private Vector2 m_vEndPosition;
    private float m_fEndTime;
    private bool m_bIsBlocking = false;
    private bool m_bIsDelay = false;

    private float m_fLastLegereTime = -999f;
    private float m_fLastMoyenneTime = -999f;
    private float m_fLastLourdeTime = -999f;

    private Coroutine m_cTrailCoroutine;
    private Coroutine m_cShieldCoroutine;
    private Coroutine m_cLightAttackCoroutine;
    //—------------------
    #endregion

    //—-------public—----
    public void SetCurrentEnemy(Enemy csEnemy)
    {
        m_csCurrentEnemy = csEnemy;
    }
    //—------------------

    private void Awake()
    {
        m_csInputManager = InputManager.Instance;
        m_csCombatSystem = GetComponent<FightSystem>();
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

    private void SwipeStart(Vector2 vPosition, float fTime)
    {
        m_vStartPosition = vPosition;
        m_fStartTime = fTime;
        m_goTrail.GetComponent<TrailRenderer>().Clear();
        m_goTrail.transform.position = vPosition;
        m_goTrail.SetActive(true);
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
        m_goTrail.SetActive(false);
        m_goTrail.GetComponent<TrailRenderer>().Clear();

        if (m_bIsBlocking)
        {
            m_bIsBlocking = false;
            if (m_cShieldCoroutine != null) StopCoroutine(m_cShieldCoroutine);
            Debug.Log(" Bouclier désactivé (fin de touch) !");
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
            Debug.Log("Swipe trop court ou trop lent");
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

    private void ActivateShield()
    {
        m_bIsBlocking = true;
        Debug.Log(" Bouclier activé !");

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
            Debug.Log("Bouclier désactivé automatiquement après 2s !");
        }
    }

    private IEnumerator LightAttack()
    {
        yield return new WaitForSeconds(0.25f);
        if (m_bIsDelay)
        {
            m_bIsDelay = false;
            Debug.Log("Delay light attack");
        }
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

    public void ReceiveDamage(float fAmount)
    {
        if (m_bIsBlocking)
        {
            Debug.Log(" Le joueur bloque les dégâts !");
            return;
        }

        m_fPv -= fAmount;
        m_fPv = Mathf.Max(0, m_fPv);
        Debug.Log($"🟥 Le joueur prend {fAmount} dégâts. PV restants : {m_fPv}");

        if (m_fPv <= 0)
        {
            Debug.Log(" Le joueur est KO !");
        }
    }

    private void SwipeDirection(Vector2 vDirection)
    {
        if (m_csCombatSystem == null)
        {
            Debug.LogError("CombatSystem n’est pas assigné ! Ajoute-le dans l’inspecteur !");
            return;
        }

        if (m_csCurrentEnemy == null)
        {
            Debug.Log("Aucun ennemi actif !");
            return;
        }

        if (Vector2.Dot(Vector2.up, vDirection) > m_fDirectionTreshold)
        {
            if (Time.time - m_fLastLegereTime >= m_fCooldownLegere)
            {
                float fDegats = m_csCombatSystem.GetDegatsInfliges(m_fAttaque, m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Legere);
                m_csCurrentEnemy.TakeDamage(fDegats);
                ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
                m_fLastLegereTime = Time.time;
            }
            else Debug.Log("Attaque légère en cooldown !");
        }
        else if (Vector2.Dot(Vector2.down, vDirection) > m_fDirectionTreshold)
        {
            if (Time.time - m_fLastLourdeTime >= m_fCooldownLourde)
            {
                float fDegats = m_csCombatSystem.GetDegatsInfliges(m_fAttaque, m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Lourde);
                m_csCurrentEnemy.TakeDamage(fDegats);
                ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
                m_fLastLourdeTime = Time.time;
            }
            else Debug.Log("Attaque lourde en cooldown !");
        }
        else if (Vector2.Dot(Vector2.right, vDirection) > m_fDirectionTreshold)
        {
            if (Time.time - m_fLastMoyenneTime >= m_fCooldownMoyenne)
            {
                float fDegats = m_csCombatSystem.GetDegatsInfliges(m_fAttaque, m_csCurrentEnemy.GetDefense(), FightSystem.TypeAttaque.Moyenne);
                m_csCurrentEnemy.TakeDamage(fDegats);
                ShowFloatingDamage(fDegats, m_csCurrentEnemy.transform.position + Vector3.up);
                m_fLastMoyenneTime = Time.time;
            }
            else Debug.Log("Attaque moyenne en cooldown !");
        }
        else if (Vector2.Dot(Vector2.left, vDirection) > m_fDirectionTreshold)
        {
            Debug.Log("Swipe gauche — aucune attaque");
        }

        Debug.Log("PV restants après attaque : " + m_csCurrentEnemy.GetHealth());
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
}
