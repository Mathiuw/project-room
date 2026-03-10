using UnityEngine;

namespace MaiNull.Item
{
    [CreateAssetMenu(fileName = "ConsumableData", menuName = "ConsumableData")]
    public class ConsumableData : PickableItemData
    {
        [Header("Consumable effects")]
        public int recoverHealth;
    }
}