using UnityEngine;

namespace MaiNull
{
    public struct Knockback
    {
        public readonly float Force; 
        public float Duration;
        public Vector3 KnockbackDirection;

        public Knockback(float force, float duration, Vector3 knockbackDirection)
        {
            this.Force = force;
            this.Duration = duration;
            this.KnockbackDirection = knockbackDirection;
        }
    }     
}