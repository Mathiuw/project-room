using System;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [field: SerializeField] public float DamageMultiplier { get; private set; } = 1.0f;

    public event Action<float, Transform> OnBodyPartHit;
}
