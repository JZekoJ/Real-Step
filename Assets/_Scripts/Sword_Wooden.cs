public class Sword_Wooden : Item_Sword
{
    private void Start()
    {
        itemId = 1001;
        itemName = "Épée en bois";
        description = "Une simple épée d'entraînement en bois";
        value = 10;

        damage = 5f;
        attackSpeed = 1.2f;
    }

    public override void Attack(Transform target)
    {
        base.Attack(target);
    }
}