using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    // Variable publique pour voir l'argent dans l'Inspector (modifiable seulement dans le code)
    [SerializeField] private int money = 0;

    // Propriété en lecture seule pour accéder à l'argent
    public int CurrentMoney => money;

    // Ajoute de l'argent
    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Impossible d'ajouter un montant négatif.");
            return;
        }

        money += amount;
        Debug.Log("Argent ajouté : " + amount + " | Total : " + money);
    }

    // Retire de l'argent
    public void RemoveMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Impossible de retirer un montant négatif.");
            return;
        }

        if (money - amount < 0)
        {
            Debug.LogWarning("Pas assez d'argent pour retirer : " + amount);
            return;
        }

        money -= amount;
        Debug.Log("Argent retiré : " + amount + " | Total : " + money);
    }

    // Définit directement l'argent (attention aux valeurs négatives)
    public void SetMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("L'argent ne peut pas être négatif.");
            return;
        }

        money = amount;
        Debug.Log("Argent défini à : " + money);
    }
}
