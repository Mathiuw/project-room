using MaiNull.Player;
using UnityEngine;

namespace MaiNull
{
    public class DamageArea : MonoBehaviour
    {
        [Header("Area Settings")]
        [SerializeField] private float damageCooldown = 3f;
        [Header("Knockback Settings")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float knockbackForce = 3f;
        [SerializeField] private float knockbackDuration = 0.2f;

        private float currentDamageCooldown;

        private void Update()
        {
            if (currentDamageCooldown > 0f)
            {
                currentDamageCooldown -= Time.deltaTime;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (currentDamageCooldown > 0) return;


            if (other.transform.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.Damage(damage, new Knockback(knockbackForce, knockbackDuration, -(transform.position - other.transform.position)), transform);
            }

            currentDamageCooldown = damageCooldown;
        }
    }
}
