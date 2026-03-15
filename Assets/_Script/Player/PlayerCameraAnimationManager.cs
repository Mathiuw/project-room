using UnityEngine;

namespace MaiNull.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerCameraAnimationManager : MonoBehaviour
    {
        PlayerWeaponInteraction playerWeaponInteraction;
        PlayerMovement playerMovement;
        Animator animator;
        Rigidbody playerRb;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            playerWeaponInteraction = FindFirstObjectByType<PlayerWeaponInteraction>();
            playerMovement = FindAnyObjectByType<PlayerMovement>();
            playerRb = playerWeaponInteraction?.GetComponent<Rigidbody>();

            if (playerWeaponInteraction)
            {
                playerWeaponInteraction.OnWeaponPickup += OnWeaponPickup;
                playerWeaponInteraction.OnWeaponDrop += OnDrop;
                playerWeaponInteraction.OnWeaponShot += OnWeaponShot;
                playerWeaponInteraction.OnReloadStart += ReloadStart;
                playerWeaponInteraction.OnReloadEnd += ReloadEnd;
            }
            else
            {
                Debug.LogError("Cant find player");
                enabled = false;
            }
        }

        private void OnDisable()
        {
            playerWeaponInteraction.OnWeaponPickup -= OnWeaponPickup;
            playerWeaponInteraction.OnWeaponDrop -= OnDrop;
            playerWeaponInteraction.OnWeaponShot -= OnWeaponShot;
            playerWeaponInteraction.OnReloadStart -= ReloadStart;
            playerWeaponInteraction.OnReloadEnd -= ReloadEnd;
        }

        void Update()
        {
            animator.SetFloat("Walk Speed", WalkSpeed(playerMovement));
            animator.SetFloat("RbVelocity", playerRb.linearVelocity.magnitude);
        }

        private void OnWeaponPickup(Weapon weapon)
        {
            SetShootFirerateTime(weapon);
            animator.runtimeAnimatorController = weapon.WeaponData.animatorOverride;
        }

        private void OnWeaponShot(Weapon weapon)
        {
            animator.Play("Shoot", 1);
        }

        private void ReloadStart() => animator.Play("Start Reload", 0);

        private void ReloadEnd() => animator.Play("End Reload", 0);

        private void OnDrop() => animator.Rebind();

        private float WalkSpeed(PlayerMovement playerMovement)
        {
            if (playerMovement.IsSprinting) return 1.5f;
            else return 1f;
        }

        private void SetShootFirerateTime(Weapon weapon)
        {
            animator.SetFloat("firerate", weapon.WeaponData.firerate);
        }
    }
}