using UnityEngine;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "Poison_Data", menuName = "Effect/Poison")]
    public class EffectPoisonData : EffectData
    {
        [Header("Poison Data")]
        public float damagePerTickAmount = 3;
        
        public override void EffectUpdate(Transform target, Effect effectObject)
        {
            base.EffectUpdate(target, effectObject);
        }
    }
}