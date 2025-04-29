using UnityEngine;

namespace FightSysteme
{
    public class FightSystem : MonoBehaviour
    {
        #region Enum
        public enum TypeAttaque
        {
            Legere,
            Moyenne,
            Lourde
        }
        #endregion

        private PlayerScript m_playerScript;

        #region Public Functions
        public int CalculerDegats(float fPv, float fAttaque, float fDefense)
        {
            if (fDefense <= 0f) fDefense = 1f;

            
            float fDegats = (fAttaque / (fDefense * 0.5f));
            float fNouveauPv = fPv - fDegats;

            return Mathf.RoundToInt(fNouveauPv);
        }

        

        public int GetDegatsInfliges(float fAttaque, float fDefense, TypeAttaque eTypeAttaque)
        {
            if (fDefense <= 0f) fDefense = 1f;

            float fMultiplicateur = GetMultiplicateur(eTypeAttaque);
            float fDegats = (fAttaque / (fDefense * 0.5f)) * fMultiplicateur;

            return Mathf.RoundToInt(fDegats);
        }
        #endregion

        #region Private Functions
        private float GetMultiplicateur(TypeAttaque eTypeAttaque)
        {
            switch (eTypeAttaque)
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
        #endregion
    }
}
