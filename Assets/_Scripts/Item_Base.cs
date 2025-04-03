using UnityEngine;

public class Item_Base : MonoBehaviour
{
    [Header("Propriétés de base")]
    public int itemId;
    public string itemName;
    public string description;
    public int value;
    
    protected bool isEquipped = false;
    
    public virtual void Use()
    {
    }
    
    public virtual void Pickup()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void Drop()
    {
        isEquipped = false;
        gameObject.SetActive(true);
    }
}