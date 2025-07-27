using System;
using UnityEngine;

public class BodyPart : MonoBehaviour, IDamageable, IDead
{
    [field: SerializeField] public float DamageMultiplier { get; private set; } = 1.0f;

    public event Action<float, Transform> onBodyPartHit;

    public void Damage(float damageValue, Transform damageInstigator)
    {
        onBodyPartHit?.Invoke(damageValue * DamageMultiplier, damageInstigator);
    }

    public void Dead()
    {
        Destroy(this);
    }
}
