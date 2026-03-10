using System;
using UnityEngine;

public class BodyPart : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float DamageMultiplier { get; private set; } = 1.0f;

    public event Action<float, Transform> OnBodyPartHit;

    public void Damage(float damageValue, Transform damageInstigator)
    {
        OnBodyPartHit?.Invoke(damageValue * DamageMultiplier, damageInstigator);
    }
}
