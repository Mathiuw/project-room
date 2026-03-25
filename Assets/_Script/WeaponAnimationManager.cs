using MaiNull.Item;
using UnityEngine;

namespace MaiNull
{
    public class WeaponAnimationManager : MonoBehaviour
    {
        Animator animator;
        Weapon weapon;

        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            animator.enabled = true;
        }

        private void OnEnable()
        {
            weapon = GetComponent<Weapon>();
            //weapon.onShoot += ShootWeaponAnimation;

            SetShootFirerateTime();
        }

        private void OnDisable()
        {
            //weapon.onShoot -= ShootWeaponAnimation;
        }

        void SetShootFirerateTime()
        {
            animator.SetFloat("Time", weapon.WeaponData.firerate);
        }

        public void PlayShootAnimation()
        {
            animator.Play("Shoot", -1, 0f);
        }
    }
}