using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Statistiques")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackPower = 10;
    [SerializeField] private float defense = 5;

    [Header("Attaque automatique")]
    [SerializeField] private float attackInterval = 2f;

    public UnityEvent onDeath;

    private Coroutine attackRoutine;
    private SwipeDetection player;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (onDeath == null)
            onDeath = new UnityEvent();
    }

    public void StartAttacking(SwipeDetection playerTarget)
    {
        player = playerTarget;

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(attackInterval); // délai initial facultatif

        while (player != null)
        {
            player.ReceiveDamage(attackPower);
            Debug.Log($"{gameObject.name} attaque le joueur pour {attackPower} dégâts !");
            yield return new WaitForSeconds(attackInterval);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " a pris " + damage + " dégâts. HP restants: " + currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " est mort !");
        onDeath.Invoke();

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        Destroy(gameObject);
    }

    // Accesseurs
    public float GetHealth() { return currentHealth; }
    public float GetMaxHealth() { return maxHealth; }
    public float GetAttackPower() { return attackPower; }
    public float GetDefense() { return defense; }
}
