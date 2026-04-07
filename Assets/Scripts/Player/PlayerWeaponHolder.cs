using System;
using System.Collections;
using MaiNull.Item;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull.Player
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerWeaponHolder : WeaponHolder
    {
        [SerializeField] private InputActionReference reloadAction;
        [SerializeField] private InputActionReference dropAction;
        
        public bool IsReloading { get; private set; } = false;
        public bool IsLerping { get; private set; } = false;

        private Transform _cameraTransform;
        private Inventory _inventory;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
        }

        private void OnEnable()
        {
            dropAction.action.started += DropWeaponAction;
            dropAction.action.Enable();

            reloadAction.action.started += ReloadWeaponAction;
            reloadAction.action.Enable();
        }



        void Start()
        {
            CameraPivot cameraPivot = GetComponentInChildren<CameraPivot>();

            if (cameraPivot)
            {
                //weaponContainer = cameraPivot.attatchedCamera.GetComponentInChildren<WeaponHolder>().transform;
                _cameraTransform = cameraPivot.attatchedCamera.transform;
            }
        }

        private void OnDisable()
        {
            dropAction.action.started-= DropWeaponAction;
            dropAction.action.Disable();

            reloadAction.action.started -= ReloadWeaponAction;
            reloadAction.action.Disable();
        }

        void Update()
        {
            if (CurrentWeapon != null)
            {
                if (IsReloading) return;
                if (IsLerping) return;

                if (InputShoot())
                {
                    CurrentWeapon.Shoot(_cameraTransform);
                }
            }
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
                EShootType.Single => Mouse.current.leftButton.wasPressedThisFrame,
                EShootType.Automatic => Mouse.current.leftButton.isPressed,
                _ => false,
            };
        }
        
        private void ReloadWeaponAction(InputAction.CallbackContext obj)
        {
            ReloadWeapon();
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
            if (_inventory.GetAmmoAmountByType(CurrentWeapon.WeaponData.ammoType) == 0) yield break;

            //OnReloadStart?.Invoke();

            IsReloading = true;
            yield return new WaitForSeconds(CurrentWeapon.WeaponData.reloadTime);

            EAmmoType ammoType = CurrentWeapon.WeaponData.ammoType;
            int amountToReload = 0;
            int inventoryAmmoAmount = _inventory.GetAmmoAmountByType(ammoType);

            // Reload amount logic
            for (int i = CurrentWeapon.CurrentAmmo; i < CurrentWeapon.WeaponData.maxAmmo; i++)
            {
                if (inventoryAmmoAmount == 0) break;

                inventoryAmmoAmount--;
                amountToReload++;
            }

            CurrentWeapon.CurrentAmmo += amountToReload;
            _inventory.RemoveAmmo(ammoType, amountToReload);

            IsReloading = false;
            //OnReloadEnd?.Invoke();

            yield break;
        }

        private void DropWeaponAction(InputAction.CallbackContext obj)
        {
            DropWeapon();
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