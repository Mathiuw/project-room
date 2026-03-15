
using UnityEngine;

public interface IDamageable
{
    public void Damage(float damageValue, float knockback, Transform damageInstigator);
}