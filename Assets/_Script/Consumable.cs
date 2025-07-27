using UnityEngine;

public class Consumable : Item
{
    [field: SerializeField] public int Amount { get; set; } = 1;

    public void UseConsumable(Health health) 
    {
        if (health.HealthAmount < health.MaxHealth)
        {
            SOConsumable soConsumable = (SOConsumable)SOItem;

            health.AddHealth(soConsumable.recoverHealth);
        }   
    }
}