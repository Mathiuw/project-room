using UnityEngine;

namespace MaiNull
{
    public abstract class Card : ScriptableObject
    {
        public string cardName;
        [TextArea] public string cardDescription;
        
        public abstract void ApplyCardEffect(Transform objectToApply);
    }
}