using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public int HealthAmount { get; private set; } = 0;
    public bool Dead { get; private set; } = false;

    public event Action<int> healthUpdated;
    public event Action onDead;

    void Awake() 
    {
        // On start, health is set to max
        HealthAmount = MaxHealth;
    } 

    public void AddHealth(int amount)
    {
        HealthAmount += amount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0, MaxHealth);
        healthUpdated?.Invoke(HealthAmount);
    }

    public void RemoveHealth(int amount)
    {
        if (Dead) return;

        HealthAmount -= amount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0, MaxHealth);
        healthUpdated?.Invoke(HealthAmount);

        if (HealthAmount <= 0)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath() 
    {
        Dead = true;

        foreach (IDead deadInterface in GetComponents<IDead>())
        {
            deadInterface.Dead();
        }

        onDead?.Invoke();
    }
}