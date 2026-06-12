using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaiNull.UI
{
    public class UIAmmo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoUI;
        [SerializeField] private Image ammoSprite;
        private WeaponHolder _playerWeaponHolder;
        // private Inventory _playerInventory;

        private void Start()
        {
            _playerWeaponHolder = FindFirstObjectByType<WeaponHolder>();

            if (_playerWeaponHolder)
            {
                _playerWeaponHolder.OnWeaponPickup += ActivateUISprite;
                if (_playerWeaponHolder.CurrentWeapon != null)
                {
                    _playerWeaponHolder.CurrentWeapon.OnWeaponShot += OnWeaponShot;
                }
                _playerWeaponHolder.OnWeaponDrop += DisableUISprite;
                _playerWeaponHolder.OnWeaponReload += SetUIAmmoText;

                if (_playerWeaponHolder.CurrentWeapon != null) ActivateUISprite();
                else DisableUISprite();
            }

            // _playerInventory = _playerWeaponHolder.GetComponent<Inventory>();
            //
            // if (_playerInventory)
            // {
            //     _playerInventory.OnAmmoCountUpdate += SetUIAmmoText;
            // }
        }

        private void OnDisable()
        {
            _playerWeaponHolder.OnWeaponPickup -= ActivateUISprite;
            _playerWeaponHolder.OnWeaponDrop -= DisableUISprite;
            if (_playerWeaponHolder.CurrentWeapon != null)
            {
                _playerWeaponHolder.CurrentWeapon.OnWeaponShot -= OnWeaponShot;
            }
            _playerWeaponHolder.OnWeaponReload -= SetUIAmmoText;
        }

        private void OnWeaponShot(Weapon weapon, RaycastHit hit)
        {
            SetUIAmmoText();
        }

        private void ActivateUISprite(Weapon weapon = null)
        {
            ammoUI.enabled = true;
            ammoSprite.sprite = weapon.WeaponData.ammoSprite;
            SetUIAmmoText();
        }

        private void DisableUISprite()
        {
            ammoUI.enabled = false;
            ammoSprite.enabled = false;
        }

        private void SetUIAmmoText()
        {
            if (_playerWeaponHolder.CurrentWeapon == null)
            {
                ammoUI.SetText("");
                return;
            }

            int ammo = _playerWeaponHolder.CurrentWeapon.CurrentAmmo;
            // int ammoStored = _playerInventory.GetAmmoAmountByType(_playerWeaponHolder.CurrentWeapon.WeaponData.ammoType);

            // ammoUI.SetText(ammo + "/" + ammoStored);
        }
    }
}