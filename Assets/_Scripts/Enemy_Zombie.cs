public class Enemy_Zombie : Enemy_Base
{
    protected void Start()
    {
        FindPlayer();
        
        health = 80f;
        speed = 2f;
        attackDamage = 15f;
        attackSpeed = 0.8f;
    }
    
    public override void Attack()
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
    
    protected override void Die()
    {
        base.Die();
    }
}