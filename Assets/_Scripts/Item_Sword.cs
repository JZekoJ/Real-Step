public class Item_Sword : Item_Base
{
    [Header("Propriétés d'épée")]
    public float damage;
    public float attackSpeed;

    public GameObject slashEffect;

    public virtual void Attack(Transform target)
    {
        if (target != null)
        {
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    public override void Use()
    {
        Attack(null);
    }
}