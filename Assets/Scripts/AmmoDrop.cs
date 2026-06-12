using UnityEngine;

namespace MaiNull
{
    public class AmmoDrop : MonoBehaviour, IInteractable, IUIName
    {
        [SerializeField] private EAmmoType ammoType;
        [SerializeField] private int ammoAmount = 1;

        public string readName => "Pickup " + GetAmmoName() + " Ammo";

        public void Interact(Transform interactor)
        {
            // Inventory inventory = interactor.GetComponent<Inventory>();
            //
            // if (inventory)
            // {
            //     inventory.AddAmmo(ammoType, ammoAmount);
            //     Destroy(gameObject);
            // }
            // else
            // {
            //     Debug.LogWarning("interactor does not have inventory");
            // }
        }

        private string GetAmmoName()
        {
            return ammoType switch {
                EAmmoType.Pistol => "Pistol",
                EAmmoType.Riffle => "Riffle",
                EAmmoType.Shell => "Shell",
                var _ => "INVALID",
            };
        }
    }
}


