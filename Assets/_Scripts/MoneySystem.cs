using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    [SerializeField] private int money = 0;

    public int CurrentMoney => money;

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
