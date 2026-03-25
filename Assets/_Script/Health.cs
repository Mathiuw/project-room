using System;
using UnityEngine;

public class Health
{
    public int MaxHealth { get; private set; }

    public int HealthAmount { get; private set; }

    public bool Dead { get; private set; } = false;

    public event Action<int> OnHealthChange;
    public event Action OnDie;

    // Constructors
    public Health() 
    { 
        MaxHealth = 100;
        HealthAmount = 100;
        Dead = false;
    }

    public Health(int MaxHealth)
    {
        this.MaxHealth = MaxHealth;
        HealthAmount = MaxHealth;
        Dead = false;
    }

    public void AddHealth(int amount)
    {
        HealthAmount += amount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0, MaxHealth);
        OnHealthChange?.Invoke(HealthAmount);
    }

    public void RemoveHealth(int amount)
    {
        if (Dead) return;

        HealthAmount -= amount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0, MaxHealth);
        OnHealthChange?.Invoke(HealthAmount);

        if (HealthAmount <= 0) Die();
    }

    private void Die() 
    {
        Dead = true;
        OnDie?.Invoke();
    }
}