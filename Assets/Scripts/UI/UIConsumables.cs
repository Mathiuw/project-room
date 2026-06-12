using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaiNull.UI
{
    [RequireComponent (typeof(CanvasGroup))]
    public class UIConsumables : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        // Inventory inventory;
        [SerializeField] Image selectedConsumableImage;
        [SerializeField] TextMeshProUGUI amountText;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            // inventory = FindAnyObjectByType<Inventory>();
            //
            // if (inventory)
            // {
            //     DrawConsumableInventory();
            //
            //     inventory.OnConsumableAdd += OnItemAdded;
            //     inventory.OnConsumableUse += OnItemRemoved;
            //     inventory.OnConsumableIndexUpdate += OnConsumableIndexUpdate;
            // }
            // else 
            // {
            //     Debug.LogError("Cant find inventory");
            //     canvasGroup.alpha = 0;
            //     enabled = false;
            //     return;
            // }
        }

        private void OnDisable()
        {
            // inventory.OnConsumableAdd -= OnItemAdded;
            // inventory.OnConsumableUse -= OnItemRemoved;
            // inventory.OnConsumableIndexUpdate += OnConsumableIndexUpdate;
        }

        private void OnItemAdded(Consumable item)
        {
            DrawConsumableInventory();
        }

        private void OnItemRemoved()
        {
            DrawConsumableInventory();
        }

        private void OnConsumableIndexUpdate(int index)
        {
            DrawConsumableInventory();
        }

        private void DrawConsumableInventory()
        {
            // if (inventory.Consumables.Count == 0)
            // {
            //     canvasGroup.alpha = 0;
            //     return;
            // }
            // else
            // {
            //     canvasGroup.alpha = 1;
            //
            //     Sprite selectedSprite = inventory.Consumables[inventory.ConsumableIndex].PickableItemData.hotbarSprite;
            //     selectedConsumableImage.sprite = selectedSprite;
            //     amountText.text = inventory.Consumables[inventory.ConsumableIndex].Amount.ToString();
            //     Debug.Log("Draw Consumable inventory");
            // }
        }
    }
}