using UnityEngine;

namespace MaiNull
{
    public abstract class CardData : ScriptableObject
    {
        public string tittle;
        [TextArea] public string description;
        public Sprite sprite;
        
        public abstract void ApplyEffect(Transform objectToApply);
    }
}