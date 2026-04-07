using UnityEngine;

namespace MaiNull.Item
{
    [CreateAssetMenu(fileName = "ConsumableData", menuName = "ConsumableData")]
    public class HealingItemData : InventoryItemData
    {
        [Header("Healing item effects")]
        public int recoverHealthAmount = 1;
    }
}