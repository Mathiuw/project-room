using MaiNull;
using UnityEngine;
using MaiNull.Interact;

namespace MaiNull.Item
{
    public class PickableItem : Pickable
    {
        [field: SerializeField] public InventoryItemData PickableItemData { get; private set; }

        public override string readName => PickableItemData.itemName;

        public override void Interact(Transform interactor)
        {
            Inventory inventory;

            if ((inventory = interactor.GetComponent<Inventory>()) && inventory.AddItem(GetComponent<PickableItem>()))
            {
                Debug.Log("Picked " + PickableItemData.name);
                Destroy(gameObject);
            }
        }
    }
}