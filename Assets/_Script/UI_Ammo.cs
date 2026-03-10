using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ammo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoUI;
    [SerializeField] Image ammoSprite;
    PlayerWeaponInteraction playerWeaponInteraction;
    Inventory playerInventory;

    void Start() 
    {        
        playerWeaponInteraction = FindFirstObjectByType<PlayerWeaponInteraction>();

        if (playerWeaponInteraction) 
        {
            playerWeaponInteraction.OnWeaponPickup += ActivateUISprite;
            playerWeaponInteraction.OnWeaponShot += OnWeaponShot;
            playerWeaponInteraction.OnWeaponDrop += DisableUISprite;
            playerWeaponInteraction.OnReloadEnd += SetUIAmmoText;

            if (playerWeaponInteraction.Weapon) ActivateUISprite();
            else DisableUISprite();
        }

        playerInventory = playerWeaponInteraction.GetComponent<Inventory>();

        if (playerInventory)
        {
            playerInventory.OnAmmoCountUpdate += SetUIAmmoText;
        }
    }

    private void OnDisable()
    {
        playerWeaponInteraction.OnWeaponPickup -= ActivateUISprite;
        playerWeaponInteraction.OnWeaponDrop -= DisableUISprite;
        playerWeaponInteraction.OnWeaponShot -= OnWeaponShot;
        playerWeaponInteraction.OnReloadEnd -= SetUIAmmoText;

        playerInventory.OnAmmoCountUpdate -= SetUIAmmoText;
    }

    private void OnWeaponShot(Weapon weapon)
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
        if (!playerWeaponInteraction.Weapon)
        {
            ammoUI.SetText("");
            return;
        }

        int ammo = playerWeaponInteraction.Weapon.Ammo;
        int ammoStored = playerInventory.GetAmmoAmountByType(playerWeaponInteraction.Weapon.WeaponData.ammoType);

        ammoUI.SetText(ammo + "/" + ammoStored);
    }  
}
