using UnityEngine;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "Effect_Data", menuName = "Effect")]
    public abstract class EffectData : ScriptableObject
    {
        [Header("Effect Data")]
        public string tittle;
        [TextArea] public string description;
        public float duration;
        public Sprite sprite;

        public virtual void EffectStart(Transform target, Effect effectObject) { }

        public virtual void EffectUpdate(Transform target, Effect effectObject)
        {
            if (!effectObject.enabled) return;
            
            effectObject.currentDuration -= Time.deltaTime;
            if (effectObject.currentDuration > 0) return;
            EffectEnd(target, effectObject);
            effectObject.enabled = false;
        }
        
        public virtual void EffectEnd(Transform target, Effect effectObjects) { }
    }
}