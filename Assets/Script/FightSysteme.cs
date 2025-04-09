using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public enum TypeAttaque
    {
        Legere,
        Moyenne,
        Lourde
    }

    public float CalculerDegats(float pv, float attaque, float defense, TypeAttaque typeAttaque)
    {
        
        if (defense <= 0) defense = 1;

        float multiplicateur = GetMultiplicateur(typeAttaque);
        float degats = (attaque / (defense * 0.5f)) * multiplicateur;
        float nouveauPV = pv - degats;

       
        return Mathf.Max(nouveauPV, 0);
    }

    private float GetMultiplicateur(TypeAttaque typeAttaque)
    {
        switch (typeAttaque)
        {
            case TypeAttaque.Legere:
                return 1.0f;
            case TypeAttaque.Moyenne:
                return 2.0f;
            case TypeAttaque.Lourde:
                return 4.0f;
            default:
                Debug.LogWarning("Type d'attaque inconnu, valeur par défaut appliquée !");
                return 1.0f;
        }
    }
}
