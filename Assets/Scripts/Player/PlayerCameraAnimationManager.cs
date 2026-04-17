using System.Collections;
using MaiNull.Item;
using UnityEngine;

namespace MaiNull.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerCameraAnimationManager : MonoBehaviour
    {
        PlayerWeaponHolder playerWeaponHolder;
        PlayerRBMovement _playerRbMovement;
        Animator animator;
        Rigidbody playerRb;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            playerWeaponHolder = FindFirstObjectByType<PlayerWeaponHolder>();
            _playerRbMovement = FindAnyObjectByType<PlayerRBMovement>();
            playerRb = playerWeaponHolder?.GetComponent<Rigidbody>();

            if (playerWeaponHolder)
            {
                playerWeaponHolder.OnWeaponPickup += OnWeaponPickup;
                playerWeaponHolder.OnWeaponDrop += OnDrop;
                if (playerWeaponHolder.CurrentWeapon != null)
                {
                    playerWeaponHolder.CurrentWeapon.OnWeaponShot += OnWeaponShot;
                }
                //playerWeaponHolder.OnReloadStart += ReloadStart;
                //playerWeaponHolder.OnReloadEnd += ReloadEnd;
            }
            else
            {
                Debug.LogError("Cant find player");
                enabled = false;
            }
        }

        private void OnDisable()
        {
            playerWeaponHolder.OnWeaponPickup -= OnWeaponPickup;
            playerWeaponHolder.OnWeaponDrop -= OnDrop;
            if (playerWeaponHolder.CurrentWeapon != null)
            {
                playerWeaponHolder.CurrentWeapon.OnWeaponShot -= OnWeaponShot;
            }
            //playerWeaponHolder.OnReloadStart -= ReloadStart;
            //playerWeaponHolder.OnReloadEnd -= ReloadEnd;
        }

        void Update()
        {
            if (!playerRb) return;

            animator.SetFloat("Walk Speed", WalkSpeed(_playerRbMovement));
            animator.SetFloat("RbVelocity", playerRb.linearVelocity.magnitude);
        }

        private void OnWeaponPickup(Weapon weapon)
        {
            SetShootFirerateTime(weapon);
            animator.runtimeAnimatorController = weapon.WeaponData.animatorOverride;
        }

        private void OnWeaponShot(Weapon weapon, RaycastHit hit)
        {
            animator.Play("Shoot", 1);
        }

        private void ReloadStart() => animator.Play("Start Reload", 0);

        private void ReloadEnd() => animator.Play("End Reload", 0);

        private void OnDrop() => animator.Rebind();

        private float WalkSpeed(PlayerRBMovement playerRbMovement)
        {
            if (!playerRbMovement) return 1f;

            if (playerRbMovement.IsSprinting) return 1.5f;
            else return 1f;
        }

        private void SetShootFirerateTime(Weapon weapon)
        {
            animator.SetFloat("firerate", weapon.WeaponData.fireRate);
        }

        IEnumerator LerpWeaponCoroutine(float time, Transform weapon, Vector3 desiredPosition, Quaternion desiredRotation)
        {
            float elapsedTime = 0f;
            float percentageComplete = 0f;

            Vector3 startPosition;
            Quaternion startRotation;

            weapon.transform.GetLocalPositionAndRotation(out startPosition, out startRotation);

            while (elapsedTime < time)
            {
                weapon.localPosition = Vector3.Lerp(startPosition, desiredPosition, percentageComplete);
                weapon.localRotation = Quaternion.Lerp(startRotation, desiredRotation, percentageComplete);

                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / time;
                yield return null;
            }

            weapon.SetLocalPositionAndRotation(desiredPosition, desiredRotation);
        }
    }
}