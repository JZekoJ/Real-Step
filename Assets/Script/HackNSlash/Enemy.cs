using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    // Variables de base
    [Header("Statistiques")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int attackPower = 10;
    [SerializeField] private int defense = 5;

    // Événement pour notifier de la mort
    public UnityEvent onDeath;

    private void Awake()
    {
        // Initialiser la santé au démarrage
        currentHealth = maxHealth;
        
        // Initialiser l'événement s'il est null
        if (onDeath == null)
            onDeath = new UnityEvent();
    }

    // Fonction simple pour prendre des dégâts
    public void TakeDamage(int damage)
    {
        // Réduire les points de vie directement
        currentHealth -= damage;
        
        // Afficher les HP dans la console
        Debug.Log(gameObject.name + " a pris " + damage + " dégâts. HP restants: " + currentHealth + "/" + maxHealth);
        
        // Vérifier si l'ennemi est mort
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Fonction appelée quand l'ennemi meurt
    private void Die()
    {
        Debug.Log(gameObject.name + " est mort!");
        
        // Déclencher l'événement de mort
        onDeath.Invoke();
        
        // Détruire le GameObject
        Destroy(gameObject);
    }
    
    // Accesseurs pour les statistiques
    public int GetHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public int GetAttackPower() { return attackPower; }
    public int GetDefense() { return defense; }
}