using MaiNull.Item;
using UnityEngine;

namespace MaiNull
{
    public class WeaponAnimationManager : MonoBehaviour
    {
        [Header("Weapon Sway")]
        [SerializeField] float smooth = 8;
        [SerializeField] float swayMultiplier = 4;

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
            animator.SetFloat("Time", weapon.WeaponData.fireRate);
        }

        public void PlayShootAnimation()
        {
            animator.Play("Shoot", -1, 0f);
        }

        void SwayWeapon(float swayMultiplier)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

            Quaternion targetRotation = rotationX * rotationY;

            //CurrentWeapon.transform.localRotation = Quaternion.Slerp(CurrentWeapon.transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
    }
}