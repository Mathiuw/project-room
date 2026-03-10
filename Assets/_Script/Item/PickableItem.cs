using UnityEngine;
using MaiNull.Interact;

namespace MaiNull.Item
{
    public class PickableItem : MonoBehaviour, IInteractable, IUIName
    {
        [field: SerializeField] public PickableItemData PickableItemData { get; private set; }

        public string ReadName => PickableItemData.itemName;

        public void Interact(Transform interactor)
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