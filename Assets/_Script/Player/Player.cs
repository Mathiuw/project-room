using UnityEngine;

namespace MaiNull.Player
{
	public class Player : MonoBehaviour, IDamageable
	{
        private Health health;
        private Rigidbody rb;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            if (TryGetComponent(out health))
            {
                health.OnDead += OnDead;
            }

            rb = GetComponent<Rigidbody>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        public void Damage(float damageValue, float knockback, Transform damageInstigator)
        {
            health.RemoveHealth((int)damageValue);
            //Debug.Log("DAMAGE!");
        }

        private void OnDead()
        {
            if (health)
            {
                health.OnDead += OnDead;
            }

            if (rb)
            {
                rb.freezeRotation = false;
            }

            if (playerMovement)
            {
                playerMovement.Input.Disable();
                playerMovement.enabled = false;
            }

            Debug.Log("Player is dead!!");
        }
    }
}