using UnityEngine;

namespace MaiNull
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float knockback = 3f;
        [SerializeField] private float damageCooldown = 0.1f;
        private float currentDamageCooldown;

        private void Update()
        {
            if (currentDamageCooldown > 0f)
            {
                currentDamageCooldown -= Time.deltaTime;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (currentDamageCooldown > 0) return;


            if (collision.transform.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.Damage(damage, knockback, transform);
            }

            currentDamageCooldown = damageCooldown;
        }
    }
}
