using System.Collections;
using UnityEngine;

namespace MaiNull
{
    [RequireComponent(typeof(Animator))]
    public class PlayerCameraAnimationManager : MonoBehaviour
    {
        private WeaponHolder _playerWeaponHolder;
        private PlayerRbMovement _playerRbMovement;
        private Animator _animator;
        private Rigidbody _playerRb;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _playerWeaponHolder = FindFirstObjectByType<WeaponHolder>();
            _playerRbMovement = FindAnyObjectByType<PlayerRbMovement>();
            _playerRb = _playerWeaponHolder?.GetComponent<Rigidbody>();

            if (_playerWeaponHolder)
            {
                _playerWeaponHolder.OnWeaponPickup += OnWeaponPickup;
                _playerWeaponHolder.OnWeaponDrop += OnDrop;
                if (_playerWeaponHolder.CurrentWeapon != null)
                {
                    _playerWeaponHolder.CurrentWeapon.OnWeaponShot += OnWeaponShot;
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
            _playerWeaponHolder.OnWeaponPickup -= OnWeaponPickup;
            _playerWeaponHolder.OnWeaponDrop -= OnDrop;
            if (_playerWeaponHolder.CurrentWeapon != null)
            {
                _playerWeaponHolder.CurrentWeapon.OnWeaponShot -= OnWeaponShot;
            }
            //playerWeaponHolder.OnReloadStart -= ReloadStart;
            //playerWeaponHolder.OnReloadEnd -= ReloadEnd;
        }

        void Update()
        {
            if (!_playerRb) return;

            _animator.SetFloat("Walk Speed", WalkSpeed(_playerRbMovement));
            _animator.SetFloat("RbVelocity", _playerRb.linearVelocity.magnitude);
        }

        private void OnWeaponPickup(Weapon weapon)
        {
            SetShootFireRateTime(weapon);
            _animator.runtimeAnimatorController = weapon.WeaponData.animatorOverride;
        }

        private void OnWeaponShot(Weapon weapon, RaycastHit hit)
        {
            _animator.Play("Shoot", 1);
        }

        private void ReloadStart() => _animator.Play("Start Reload", 0);

        private void ReloadEnd() => _animator.Play("End Reload", 0);

        private void OnDrop() => _animator.Rebind();

        private static float WalkSpeed(PlayerRbMovement playerRbMovement)
        {
            if (!playerRbMovement) return 1f;

            return playerRbMovement.IsSprinting ? 1.5f : 1f;
        }

        private void SetShootFireRateTime(Weapon weapon)
        {
            _animator.SetFloat("firerate", weapon.WeaponData.fireRate);
        }

        private IEnumerator LerpWeaponCoroutine(float time, Transform weapon, Vector3 desiredPosition, Quaternion desiredRotation)
        {
            float elapsedTime = 0f;
            float percentageComplete = 0f;

            weapon.transform.GetLocalPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);

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