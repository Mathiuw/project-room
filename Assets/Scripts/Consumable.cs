using UnityEngine;

namespace MaiNull
{
    public class Consumable : PickableItem
    {
        [field: SerializeField] public int Amount { get; set; } = 1;

        public bool UseConsumable(Health health)
        {
            if (health.HealthAmount < health.MaxHealth)
            {
                HealingItemData soConsumable = (HealingItemData)PickableItemData;

                health.AddHealth(soConsumable.recoverHealthAmount);

                return true;
            }

            return false;
        }
    }
}