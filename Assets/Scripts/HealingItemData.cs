using UnityEngine;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "ConsumableData", menuName = "ConsumableData")]
    public class HealingItemData : InventoryItemData
    {
        [Header("Healing item effects")]
        public int recoverHealthAmount = 1;
    }
}