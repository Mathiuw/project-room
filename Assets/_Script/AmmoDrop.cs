using UnityEngine;
using MaiNull.Interact;

public class AmmoDrop : MonoBehaviour, IInteractable, IUIName
{
    [SerializeField]EAmmoType ammoType;
    [SerializeField] int ammoAmount = 1;

    public string ReadName => "Pickup " + GetAmmoName() + " Ammo";

    public void Interact(Transform interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();

        if (inventory)
        {
            inventory.AddAmmo(ammoType, ammoAmount);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("interactor does not have inventory");
        }
    }

    private string GetAmmoName() 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                return "Small";
            case EAmmoType.largeAmmo:
                return "Large";
            case EAmmoType.ShellAmmo:
                return "Shell";
            default:
                return "INVALID";
        }
    }
}
