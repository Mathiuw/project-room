using MaiNull.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaiNull
{
    public class UI_Ammo : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammoUI;
        [SerializeField] Image ammoSprite;
        PlayerWeaponHolder playerWeaponHolder;
        Inventory playerInventory;

        void Start()
        {
            playerWeaponHolder = FindFirstObjectByType<PlayerWeaponHolder>();

            if (playerWeaponHolder)
            {
                playerWeaponHolder.OnWeaponPickup += ActivateUISprite;
                if (playerWeaponHolder.CurrentWeapon != null)
                {
                    playerWeaponHolder.CurrentWeapon.OnWeaponShot += OnWeaponShot;
                }
                playerWeaponHolder.OnWeaponDrop += DisableUISprite;
                playerWeaponHolder.OnWeaponReload += SetUIAmmoText;

                if (playerWeaponHolder.CurrentWeapon != null) ActivateUISprite();
                else DisableUISprite();
            }

            playerInventory = playerWeaponHolder.GetComponent<Inventory>();

            if (playerInventory)
            {
                playerInventory.OnAmmoCountUpdate += SetUIAmmoText;
            }
        }

        private void OnDisable()
        {
            playerWeaponHolder.OnWeaponPickup -= ActivateUISprite;
            playerWeaponHolder.OnWeaponDrop -= DisableUISprite;
            if (playerWeaponHolder.CurrentWeapon != null)
            {
                playerWeaponHolder.CurrentWeapon.OnWeaponShot -= OnWeaponShot;
            }
            playerWeaponHolder.OnWeaponReload -= SetUIAmmoText;

            playerInventory.OnAmmoCountUpdate -= SetUIAmmoText;
        }

        private void OnWeaponShot(Weapon weapon, RaycastHit hit)
        {
            SetUIAmmoText();
        }

        void ActivateUISprite(Weapon weapon = null)
        {
            ammoUI.enabled = true;
            ammoSprite.sprite = weapon.WeaponData.ammoSprite;
            SetUIAmmoText();
        }

        void DisableUISprite()
        {
            ammoUI.enabled = false;
            ammoSprite.enabled = false;
        }

        void SetUIAmmoText()
        {
            if (playerWeaponHolder.CurrentWeapon == null)
            {
                ammoUI.SetText("");
                return;
            }

            int ammo = playerWeaponHolder.CurrentWeapon.CurrentAmmo;
            int ammoStored = playerInventory.GetAmmoAmountByType(playerWeaponHolder.CurrentWeapon.WeaponData.ammoType);

            ammoUI.SetText(ammo + "/" + ammoStored);
        }
    }
}