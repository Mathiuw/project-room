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
            playerWeaponInteraction.onWeaponPickup += ActivateUISprite;
            playerWeaponInteraction.onWeaponShot += SetUIAmmoText;
            playerWeaponInteraction.onWeaponDrop += DisableUISprite;
            playerWeaponInteraction.onReloadEnd += SetUIAmmoText;

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
        playerWeaponInteraction.onWeaponPickup -= ActivateUISprite;
        playerWeaponInteraction.onWeaponDrop -= DisableUISprite;
        playerWeaponInteraction.onWeaponShot -= SetUIAmmoText;
        playerWeaponInteraction.onReloadEnd -= SetUIAmmoText;

        playerInventory.OnAmmoCountUpdate -= SetUIAmmoText;
    }

    void ActivateUISprite(Weapon weapon = null)
    {
        ammoUI.enabled = true;
        ammoSprite.sprite = weapon.SOWeapon.ammoSprite;
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
        int ammoStored = playerInventory.GetAmmoAmountByType(playerWeaponInteraction.Weapon.SOWeapon.ammoType);

        ammoUI.SetText(ammo + "/" + ammoStored);
    }  
}
