using MaiNull.Item;
using System;
using System.Collections;
using UnityEngine;

namespace MaiNull.Player
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerWeaponInteraction : WeaponInteraction
    {
        [Header("Weapon Interation")]
        [SerializeField] float aimTime;

        [Header("Weapon Sway")]
        [SerializeField] float smooth = 8;
        [SerializeField] float swayMultiplier = 4;

        public bool IsReloading { get; private set; } = false;
        public bool IsLerping { get; private set; } = false;

        Transform cameraTransform;
        Inventory inventory;

        public event Action<Weapon> OnWeaponShot;
        public event Action<Weapon> OnWeaponPickup;
        public event Action<RaycastHit> OnWeaponHit;
        public event Action OnReloadStart;
        public event Action OnReloadEnd;
        public event Action OnWeaponDrop;

        void Awake()
        {
            inventory = GetComponent<Inventory>();
        }

        void Start()
        {
            CameraPivot cameraPivot = GetComponentInChildren<CameraPivot>();

            if (cameraPivot)
            {
                weaponContainer = cameraPivot.attatchedCamera.GetComponentInChildren<WeaponHolder>().transform;
                cameraTransform = cameraPivot.attatchedCamera.transform;
            }
        }

        void Update()
        {
            if (Weapon)
            {
                SwayWeapon(swayMultiplier);

                if (IsReloading) return;
                if (IsLerping) return;

                if (InputShoot())
                {
                    if (Weapon.Shoot(cameraTransform, OnWeaponHit))
                    {
                        OnWeaponShot?.Invoke(Weapon);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(ReloadWeapon());

            if (Input.GetKeyDown(KeyCode.G)) DropWeapon();
        }

        IEnumerator LerpWeaponCoroutine(float time, Transform weapon, Vector3 desiredPosition, Quaternion desiredRotation, Transform parent = null)
        {
            IsLerping = true;

            if (parent != null)
            {
                weapon.SetParent(parent);
                weapon.localScale = Vector3.one;
            }

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

            IsLerping = false;
        }

        void SwayWeapon(float swayMultiplier)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

            Quaternion targetRotation = rotationX * rotationY;

            Weapon.transform.localRotation = Quaternion.Slerp(Weapon.transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }

        public override IEnumerator PickUpWeapon(Weapon weapon)
        {
            if (Weapon) yield break;
            if (IsLerping) yield break;
            if (weapon.owner != null)
            {
                Debug.LogError("Gun already Picked up");
                yield break;
            }

            // Set current weapon
            Weapon = weapon;

            // Set weapon hold state
            Weapon.SetHoldState(true, transform);

            // Lerp weapon to player
            StartCoroutine(LerpWeaponCoroutine(0.2f, weapon.transform, Vector3.zero, Quaternion.identity, weaponContainer));
            while (IsLerping) yield return null;

            OnWeaponPickup?.Invoke(Weapon);

            Debug.Log("Picked up weapon");
        }

        bool InputShoot()
        {
            return Weapon.WeaponData.shootType switch
            {
                EShootType.Single => Input.GetKeyDown(KeyCode.Mouse0),
                EShootType.Automatic => Input.GetKey(KeyCode.Mouse0),
                _ => false,
            };
        }

        public override IEnumerator ReloadWeapon()
        {
            if (!Weapon) yield break;
            if (IsReloading) yield break;
            if (Weapon.Ammo == Weapon.WeaponData.maxAmmo) yield break;
            if (inventory.GetAmmoAmountByType(Weapon.WeaponData.ammoType) == 0) yield break;

            OnReloadStart?.Invoke();

            IsReloading = true;
            yield return new WaitForSeconds(Weapon.WeaponData.reloadTime);

            EAmmoType ammoType = Weapon.WeaponData.ammoType;
            int amountToReload = 0;
            int inventoryAmmoAmount = inventory.GetAmmoAmountByType(ammoType);

            // Reload amount logic
            for (int i = Weapon.Ammo; i < Weapon.WeaponData.maxAmmo; i++)
            {
                if (inventoryAmmoAmount == 0) break;

                inventoryAmmoAmount--;
                amountToReload++;
            }

            Weapon.AddAmmo(amountToReload);
            inventory.RemoveAmmo(ammoType, amountToReload);

            IsReloading = false;
            OnReloadEnd?.Invoke();

            yield break;
        }

        public override void DropWeapon()
        {
            if (!Weapon) return;
            if (IsReloading) return;

            StopAllCoroutines();
            OnWeaponDrop?.Invoke();

            Transform weaponTransform = Weapon.transform;
            Rigidbody weaponRb = weaponTransform.GetComponent<Rigidbody>();

            Weapon.SetHoldState(false, null);
            weaponTransform.SetParent(null);
            weaponTransform.transform.position = transform.position;
            weaponRb.AddForce(transform.forward * 5, ForceMode.VelocityChange);
            weaponTransform.localScale = Vector3.one;

            IsLerping = false;
            IsReloading = false;

            Weapon = null;

            Debug.Log("Dropped weapon");
        }

        public void Dead()
        {
            DropWeapon();
            Destroy(this);
        }
    }
}