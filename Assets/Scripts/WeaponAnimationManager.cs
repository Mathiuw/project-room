using MaiNull.Item;
using UnityEngine;

namespace MaiNull
{
    public class WeaponAnimationManager : MonoBehaviour
    {
        private static readonly int Time = Animator.StringToHash("Time");

        [Header("Weapon Sway")]
        [SerializeField] private float smooth = 8;
        [SerializeField] private float swayMultiplier = 4;
        private Animator _animator;
        private Weapon _weapon;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _animator.enabled = true;
        }

        private void OnEnable()
        {
            _weapon = GetComponent<Weapon>();
            //weapon.onShoot += ShootWeaponAnimation;

            SetShootFireRateTime();
        }

        private void SetShootFireRateTime()
        {
            _animator.SetFloat(Time, _weapon.WeaponData.fireRate);
        }

        public void PlayShootAnimation()
        {
            _animator.Play("Shoot", -1, 0f);
        }
    }
}