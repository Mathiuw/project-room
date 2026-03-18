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

        public void Damage(float damageValue, Knockback knockback, Transform damageInstigator)
        {
            if (health)
            {
                health.RemoveHealth((int)damageValue);
            }

            if (playerMovement)
            {
                playerMovement.CurrentKnockback = knockback;
            }


            //Debug.Log("DAMAGE!");
        }
    }
}