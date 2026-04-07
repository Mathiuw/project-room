
 using MaiNull.Player;
 using UnityEngine;

namespace MaiNull
{
    public interface IDamageable
    {
        public void Damage(float damageValue, Knockback knockback, Transform damageInstigator);
    }
}

