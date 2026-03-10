using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public int HealthAmount { get; private set; } = 0;
    public bool Dead { get; private set; } = false;

    public event Action<int> OnHealthUpdated;
    public event Action OnDead;

    void Awake() 
    {
        // On start, health is set to max
        HealthAmount = MaxHealth;
    }

    private void OnEnable()
    {
        // Check for body parts
        foreach (BodyPart bodyPart in GetComponentsInChildren<BodyPart>()) 
        {
            bodyPart.OnBodyPartHit += OnBodyPartHit;
        }
    }

    private void OnDisable()
    {
        foreach (BodyPart bodyPart in GetComponentsInChildren<BodyPart>())
        {
            bodyPart.OnBodyPartHit -= OnBodyPartHit;
        }
    }

    private void OnBodyPartHit(float resultDamage, Transform damageInstigator)
    {
        RemoveHealth((int)resultDamage);
    }

    public void AddHealth(int amount)
    {
        HealthAmount += amount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0, MaxHealth);
        OnHealthUpdated?.Invoke(HealthAmount);
    }

    public void RemoveHealth(int amount)
    {
        if (Dead) return;

        HealthAmount -= amount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0, MaxHealth);
        OnHealthUpdated?.Invoke(HealthAmount);

        if (HealthAmount <= 0)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath() 
    {
        Dead = true;
        OnDead?.Invoke();
    }

    public void Damage(float damageValue, Transform damageInstigator)
    {
        RemoveHealth((int)damageValue);
    }
}