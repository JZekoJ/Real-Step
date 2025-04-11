using System.Collections;
using UnityEngine;
using FightSysteme;

public class SwipeDetection : MonoBehaviour
{

    [Header("Paramètres de Swipe")]
    [SerializeField] 
    private float minimumDistance = .2f;
    [SerializeField]
    private float maximumTime = 1f;
    [SerializeField, Range(0f,1f)]
    private float directionTreshold = 0.9f;
    
    

    [Header("Références Externes")]
    [SerializeField]
    private GameObject trail;
    [SerializeField] 
    private FightSystem combatSystem;

    [Header("Stats Joueur")]
    [SerializeField]
    float pv = 100f;
    [SerializeField]
    float attaque = 20f;
    [SerializeField]
    float defense = 10f;

    [Header("Cooldowns des Attaques")]
    [SerializeField] 
    private float cooldownLegere = 1f;
    [SerializeField] 
    private float cooldownMoyenne = 2f;
    [SerializeField] 
    private float cooldownLourde = 3f;
    [SerializeField]
    private float longPressDuration = 0.5f;
    [SerializeField]
    private float longPressMaxDuration = 2f;






    private InputManager inputManager;
    private Enemy enemy;


    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private bool isBlocking = false;
    private bool isDelay = false;
    private Enemy currentEnemy;

    private float lastLegereTime = -999f;
    private float lastMoyenneTime = -999f;
    private float lastLourdeTime = -999f;

    private Coroutine coroutine;
    private Coroutine shieldCoroutine;
    private Coroutine lightAttackCoroutine;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        combatSystem = GetComponent<FightSystem>();
    }
    
    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }
    public void SetCurrentEnemy(Enemy enemy)
    {
        currentEnemy = enemy;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
        trail.GetComponent<TrailRenderer>().Clear();
        trail.transform.position = position;
        trail.SetActive(true);
        coroutine = StartCoroutine(Trail());
        StartCoroutine(LongPressDetection());
    }

    private IEnumerator Trail()
    {
        while (true) 
        { 
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        StopCoroutine(coroutine);
        trail.SetActive(false);
        trail.GetComponent<TrailRenderer>().Clear();

        if (isBlocking)
        {
            isBlocking = false;
            if (shieldCoroutine != null) StopCoroutine(shieldCoroutine);
            Debug.Log(" Bouclier désactivé (fin de touch) !");
            return; // pas de swipe
        }

        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
       if(Vector3.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime)
        {
           Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
        else
        {
            Debug.Log("Swipe too short or too slow");
        }

        ResetSwipe();
    }
    private void ResetSwipe()
    {
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        startTime = 0f;
        endTime = 0f;
    }

    private void ActivateShield()
    {
        isBlocking = true;
        Debug.Log(" Bouclier activé !");

        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(ShieldDuration());
    }

    private IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(2f);
        if (isBlocking)
        {
            isBlocking = false;
            Debug.Log("Bouclier désactivé automatiquement après 2s !");
            // start anim here
        }
    }

    private IEnumerator LightAttack()
    {
        yield return new WaitForSeconds(0.25f);
        if (isDelay)
        {
            isDelay = false;
            Debug.Log("Delay light attack");
           
        }
    }


    private IEnumerator LongPressDetection()
    {
        float timer = 0f;

        while (timer < longPressDuration)
        {
            if (Vector2.Distance(inputManager.PrimaryPosition(), startPosition) > 0.1f)
            {
                // Le doigt a bougé, ce n’est pas un long press
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Long press détecté
        ActivateShield();
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (combatSystem == null)
        {
            Debug.LogError("CombatSystem n’est pas assigné ! Ajoute-le dans l’inspecteur !");
            return;
        }

        if (currentEnemy == null)
        {
            Debug.Log("Aucun ennemi actif !");
            return;
        }

        if (Vector2.Dot(Vector2.up, direction) > directionTreshold)
        {
            if (Time.time - lastLegereTime >= cooldownLegere)
            {

                float degats = combatSystem.GetDegatsInfliges(attaque, currentEnemy.GetDefense(), FightSystem.TypeAttaque.Legere);
                currentEnemy.TakeDamage(degats);
                lastLegereTime = Time.time;
                //Debug.Log("Attaque légère lancée. PV restants : " + pv);
            }
            else
            {
                Debug.Log("Attaque légère en cooldown !");
            }
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionTreshold)
        {
            if (Time.time - lastLourdeTime >= cooldownLourde)
            {
                float degats = combatSystem.GetDegatsInfliges(attaque, currentEnemy.GetDefense(), FightSystem.TypeAttaque.Lourde);
                currentEnemy.TakeDamage(degats);
                lastLourdeTime = Time.time;
                //Debug.Log("Attaque lourde lancée. PV restants : " + pv);
            }
            else
            {
                Debug.Log("Attaque lourde en cooldown !");
            }
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionTreshold)
        {
            if (Time.time - lastMoyenneTime >= cooldownMoyenne)
            {
                float degats = combatSystem.GetDegatsInfliges(attaque, currentEnemy.GetDefense(), FightSystem.TypeAttaque.Moyenne);
                currentEnemy.TakeDamage(degats);
                lastMoyenneTime = Time.time;
                //Debug.Log("Attaque moyenne lancée. PV restants : " + pv);
            }
            else
            {
                Debug.Log("Attaque moyenne en cooldown !");
            }
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionTreshold)
        {
            Debug.Log("Swipe Left — aucune attaque ici");
        }

        Debug.Log("PV restants après attaque : " + currentEnemy.GetHealth());
    }

}
