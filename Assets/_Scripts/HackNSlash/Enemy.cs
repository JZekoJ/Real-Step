using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Statistiques")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int attackPower = 10;
    [SerializeField] private int defense = 5;
    
    public UnityEvent onDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        
        if (onDeath == null)
            onDeath = new UnityEvent();
    }
    
    public void TakeDamage(int damage)
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
    public int GetHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public int GetAttackPower() { return attackPower; }
    public int GetDefense() { return defense; }
}