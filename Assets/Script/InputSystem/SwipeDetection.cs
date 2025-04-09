using System.Collections;
using UnityEngine;
using FightSysteme;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] 
    private float minimumDistance = .2f;
    [SerializeField]
    private float maximumTime = 1f;
    [SerializeField, Range(0f,1f)]
    private float directionTreshold = 0.9f;
    [SerializeField]
    private GameObject trail;
    [SerializeField]
    private float longPressDuration = 0.5f;
    [SerializeField]
    private float longPressMaxDuration = 2f;
    [SerializeField] 
    private FightSystem combatSystem;
    [SerializeField]
    float pv = 100f;
    [SerializeField]
    float attaque = 20f;
    [SerializeField]
    float defense = 10f;
    
    



    private InputManager inputManager;
   
    private float pvRestant;
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private bool isBlocking = false;

    private Coroutine coroutine;
    private Coroutine shieldCoroutine;

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
            // Tu peux aussi ici faire une animation de fin de blocage
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

      

        if (Vector2.Dot(Vector2.up, direction) > directionTreshold)
        {
            pv = combatSystem.CalculerDegats(pv, attaque, defense, FightSystem.TypeAttaque.Legere);
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionTreshold)
        {
            pv = combatSystem.CalculerDegats(pv, attaque, defense, FightSystem.TypeAttaque.Lourde);
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionTreshold)
        {
            pv = combatSystem.CalculerDegats(pv, attaque, defense, FightSystem.TypeAttaque.Moyenne);
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionTreshold)
        {
            Debug.Log("Swipe Left ");
            return;
        }

        Debug.Log("PV restants après attaque : " + pv);
    }

}
