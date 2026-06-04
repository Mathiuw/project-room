using UnityEngine;

namespace MaiNull
{
    public interface IDamageable
    {
        public Health Health { get; }

        public void Damage (float damageValue, Knockback knockback, Transform damageInstigator)
        {
            Health.RemoveHealth((int)damageValue);
        }
    }
}

