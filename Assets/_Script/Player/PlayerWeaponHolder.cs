using MaiNull.Item;
using System.Collections;
using UnityEngine;

namespace MaiNull
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerWeaponHolder : WeaponHolder
    {
        [Header("Weapon Sway")]
        [SerializeField] float smooth = 8;
        [SerializeField] float swayMultiplier = 4;

        public bool IsReloading { get; private set; } = false;
        public bool IsLerping { get; private set; } = false;

        Transform cameraTransform;
        Inventory inventory;

        void Awake()
        {
            inventory = GetComponent<Inventory>();
        }

        void Start()
        {
            CameraPivot cameraPivot = GetComponentInChildren<CameraPivot>();

            if (cameraPivot)
            {
                //weaponContainer = cameraPivot.attatchedCamera.GetComponentInChildren<WeaponHolder>().transform;
                cameraTransform = cameraPivot.attatchedCamera.transform;
            }
        }

        void Update()
        {
            if (CurrentWeapon != null)
            {
                SwayWeapon(swayMultiplier);

                if (IsReloading) return;
                if (IsLerping) return;

                if (InputShoot())
                {
                    CurrentWeapon.Shoot(cameraTransform);
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) ReloadWeapon();

            if (Input.GetKeyDown(KeyCode.G)) DropWeapon();
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

        public override void PickUpWeapon(Weapon weapon)
        {
            StartCoroutine(PickUpWeaponCoroutine(weapon));
            base.PickUpWeapon(weapon);
        }

        public IEnumerator PickUpWeaponCoroutine(Weapon weapon)
        {
            if (IsLerping) yield break;
            //if (weapon.Owner != null)
            //{
            //    Debug.LogError("Gun already have owner!");
            //    yield break;
            //}
            if (CurrentWeapon != null) 
            {
                DropWeapon();
            } 

            // Set weapon hold state
            //CurrentWeapon.SetHoldState(true, transform);

            // Lerp weapon to player
            //StartCoroutine(LerpWeaponCoroutine(0.2f, weapon.transform, Vector3.zero, Quaternion.identity));
            while (IsLerping) yield return null;
       
            yield break;
        }

        bool InputShoot()
        {
            return CurrentWeapon.WeaponData.shootType switch
            {
                EShootType.Single => Input.GetKeyDown(KeyCode.Mouse0),
                EShootType.Automatic => Input.GetKey(KeyCode.Mouse0),
                _ => false,
            };
        }

        public override void ReloadWeapon()
        {
            StartCoroutine(ReloadWeaponCoroutine());
        }

        public IEnumerator ReloadWeaponCoroutine()
        {
            if (CurrentWeapon == null) yield break;
            if (IsReloading) yield break;
            if (CurrentWeapon.CurrentAmmo == CurrentWeapon.WeaponData.maxAmmo) yield break;
            if (inventory.GetAmmoAmountByType(CurrentWeapon.WeaponData.ammoType) == 0) yield break;

            //OnReloadStart?.Invoke();

            IsReloading = true;
            yield return new WaitForSeconds(CurrentWeapon.WeaponData.reloadTime);

            EAmmoType ammoType = CurrentWeapon.WeaponData.ammoType;
            int amountToReload = 0;
            int inventoryAmmoAmount = inventory.GetAmmoAmountByType(ammoType);

            // Reload amount logic
            for (int i = CurrentWeapon.CurrentAmmo; i < CurrentWeapon.WeaponData.maxAmmo; i++)
            {
                if (inventoryAmmoAmount == 0) break;

                inventoryAmmoAmount--;
                amountToReload++;
            }

            CurrentWeapon.CurrentAmmo += amountToReload;
            inventory.RemoveAmmo(ammoType, amountToReload);

            IsReloading = false;
            //OnReloadEnd?.Invoke();

            yield break;
        }

        public override void DropWeapon()
        {
            if (CurrentWeapon == null) return;
            if (IsReloading) return;

            //StopAllCoroutines();
            //OnWeaponDrop?.Invoke();

            //Transform weaponTransform = CurrentWeapon.transform;
            //Rigidbody weaponRb = weaponTransform.GetComponent<Rigidbody>();

            //CurrentWeapon.SetHoldState(false, null);
            //weaponTransform.SetParent(null);
            //weaponTransform.transform.position = transform.position;
            //weaponRb.AddForce(transform.forward * 5, ForceMode.VelocityChange);
            //weaponTransform.localScale = Vector3.one;

            IsLerping = false;
            IsReloading = false;

            base.DropWeapon();
        }

        public void Dead()
        {
            DropWeapon();
            Destroy(this);
        }
    }
}