using MaiNull.Item;
using UnityEngine;

namespace MaiNull.Item
{
    public class Consumable : PickableItem
    {
        [field: SerializeField] public int Amount { get; set; } = 1;

        public bool UseConsumable(Health health)
        {
            if (health.HealthAmount < health.MaxHealth)
            {
                ConsumableData soConsumable = (ConsumableData)PickableItemData;

                health.AddHealth(soConsumable.recoverHealth);

                return true;
            }

            return false;
        }
    }
}