using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaiNull
{
    public class EffectHolder : MonoBehaviour
    {
        private readonly List<Effect> _effects = new List<Effect>();
        
        public Effect[] Effects => _effects.ToArray();

        public void AddEffect(EffectData effectData)
        {
            Effect newEffect = new(effectData, transform);
            _effects.Add(newEffect);
        }

        private void Update()
        {
            if (_effects.Count <= 0) return;

            foreach (Effect effect in _effects.ToList())
            {
                if (!effect.enabled)
                {
                    _effects.Remove(effect);
                    continue;
                }
                
                effect.effectData.EffectUpdate(transform, effect);
            }
        }
    }
}