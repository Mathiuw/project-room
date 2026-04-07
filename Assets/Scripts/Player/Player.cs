using UnityEngine;

namespace MaiNull.Player
{
	public class Player : MonoBehaviour, IDamageable
	{
        public Health health { get; } = new();
        private PlayerMovementRB playerMovement;

        private void Awake()
        {
            health.OnDie += OnDead;

            playerMovement = GetComponent<PlayerMovementRB>();
        }

        private void OnDead()
        {
            Rigidbody rb = GetComponent<Rigidbody>();

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
            health.RemoveHealth((int)damageValue);
            
            if (playerMovement)
            {
                playerMovement.CurrentKnockback = knockback;
            }

            //Debug.Log("DAMAGE!");
        }
    }
}