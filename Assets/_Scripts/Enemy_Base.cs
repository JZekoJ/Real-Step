using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    [Header("Statistiques")]
    public float health;
    public float speed;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    
    protected Transform target;
    protected bool isAlive = true;
    protected float lastAttackTime;
    
    public virtual void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public virtual void Move(float speed, Vector3 direction)
    {
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
    }
    
    public virtual bool CheckRange(Transform targetToCheck = null)
    {
        Transform checkTarget = targetToCheck != null ? targetToCheck : target;
        
        if (checkTarget != null)
        {
            float distance = Vector3.Distance(transform.position, checkTarget.position);
            return distance <= attackRange;
        }
        return false;
    }
    
    public virtual void Attack()
    {
        if (Time.time >= lastAttackTime + (1f / attackSpeed))
        {
            if (CheckRange())
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
            
            lastAttackTime = Time.time;
        }
    }
    
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        
        if (health <= 0 && isAlive)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
        isAlive = false;
        Destroy(gameObject, 2f);
    }
}