using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Statistiques")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackPower = 10;
    [SerializeField] private float defense = 5;
    
    public UnityEvent onDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        
        if (onDeath == null)
            onDeath = new UnityEvent();
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
        Debug.Log(gameObject.name + " est mort!");
        
        onDeath.Invoke();
        
        Destroy(gameObject);
    }
    
    // Accesseurs pour les statistiques
    public float GetHealth() { return currentHealth; }
    public float GetMaxHealth() { return maxHealth; }
    public float GetAttackPower() { return attackPower; }
    public float GetDefense() { return defense; }
}