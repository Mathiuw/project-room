using System.Collections;
using UnityEngine;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "Card_Adrenaline", menuName = "Card/Consumable/Adrenaline", order = 0)]
    public class CardDataAdrenaline : CardData
    {
        public float adrenalineDuration = 10f;
        public float damageMultiplier = 1.5f;
        
        public override void ApplyEffect(Transform objectToApply)
        {
            Debug.Log("Adrenaline Effect");
        }
    }
}