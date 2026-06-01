using System;
using UnityEngine;

namespace MaiNull
{
    [Serializable]
    public class Effect
    {
        public bool enabled = true;
        public EffectData effectData;
        public Transform target;
        public float currentDuration;
        
        public Effect(EffectData effectData, Transform target)
        {
            this.effectData = effectData;
            this.target = target;
            this.currentDuration = effectData.duration;
            
            effectData.EffectStart(target, this);
        }
    }
}