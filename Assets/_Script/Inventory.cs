using System;
using System.Collections.Generic;
using UnityEngine;

public enum EAmmoType 
{
    smallAmmo,
    largeAmmo,
    ShellAmmo
}

public class Inventory : MonoBehaviour
{
    [Header("Item Inventory")]
    [field: SerializeField] public List<Consumable> consumables { get; private set; } = new List<Consumable>();
    [field: SerializeField] public List<Keycard> keycards { get; private set; } = new List<Keycard>();
    public int consumableIndex { get; set; } = 0;

    [Header("Ammo Inventory")]
    public int SmallAmmoAmount { get; private set; } = 0;
    public int LargeAmmoAmount { get; private set; } = 0;
    public int ShellAmmoAmount { get; private set; } = 0;

    public event Action<Consumable> OnConsumableAdd;
    public event Action OnConsumableUse;
    public event Action<int> OnConsumableIndexUpdate;

    public event Action<Keycard> OnKeycardAdd;

    public event Action OnAmmoCountUpdate;

    void Update()
    {
        // Use input input
        if (Input.GetKeyDown(KeyCode.F)) UseSelectedConsumable();

        // Scroll consumables input
        if (Input.mouseScrollDelta.y > 0f)
        {
            ChangeConsumableIndex(1);
        }
        else if (Input.mouseScrollDelta.y < 0f)
        {
            ChangeConsumableIndex(-1);
        }
    }

    public int GetAmmoAmountByType(EAmmoType ammoType) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                return SmallAmmoAmount;
            case EAmmoType.largeAmmo:
                return LargeAmmoAmount;
            case EAmmoType.ShellAmmo:
                return ShellAmmoAmount;
            default:
                Debug.LogError("Failed to add ammo");
                return 0;
        }
    }

    public void AddAmmo(EAmmoType ammoType, int amount) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                SmallAmmoAmount += amount;
                break;
            case EAmmoType.largeAmmo:
                LargeAmmoAmount += amount;
                break;
            case EAmmoType.ShellAmmo:
                ShellAmmoAmount += amount;
                break;
            default:
                Debug.LogError("Failed to add ammo");
                break;
        }

        OnAmmoCountUpdate?.Invoke();
    }

    public void RemoveAmmo(EAmmoType ammoType, int amount) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                SmallAmmoAmount -= amount;
                SmallAmmoAmount = Mathf.Clamp(SmallAmmoAmount, 0, 999);
                break;
            case EAmmoType.largeAmmo:
                LargeAmmoAmount -= amount;
                LargeAmmoAmount = Mathf.Clamp(LargeAmmoAmount, 0, 999);
                break;
            case EAmmoType.ShellAmmo:
                ShellAmmoAmount -= amount;
                ShellAmmoAmount = Mathf.Clamp(ShellAmmoAmount, 0, 999);
                break;
            default:
                break;
        }

        OnAmmoCountUpdate?.Invoke();
    }

    public bool AddItem(Item item)
    {
        if (item.GetType() == typeof(Consumable))
        {
            for (int i = 0; i < consumables.Count; i++)
            {
                // Check if already have the item
                if (item.SOItem.itemName == consumables[i].SOItem.itemName)
                {
                    // If have the item, check if you have the max amount
                    if (consumables[i].SOItem.isStackable && consumables[i].Amount < consumables[i].SOItem.maxStack)
                    {
                        Consumable consumable = (Consumable)item;

                        // Increase item quantity
                        consumables[i].Amount += consumable.Amount;
                        OnConsumableAdd?.Invoke(consumables[i]);
                        return true;
                    }
                    else
                    {
                        Debug.Log(name + " have the max amount of " + consumables[i].SOItem.itemName);
                        return false;
                    }
                }
            }

            // Add new item
            consumables.Add((Consumable)item);
            OnConsumableAdd?.Invoke((Consumable)item);
;
            return true;
        }
        else if (item.GetType() == typeof(Keycard))
        {
            keycards.Add((Keycard)item);
            OnKeycardAdd?.Invoke((Keycard)item);
            return true;
        }

        return false;
    }

    public bool RemoveConsumable(SOItem item)
    {
        for (int i = 0; i < consumables.Count; i++)
        {
            if (consumables[i].SOItem.itemName == item.itemName)
            {
                consumables[i].Amount--;

                if (consumables[i].Amount == 0)
                {
                    consumables.RemoveAt(i);
                }
                return true;
            }
        }

        Debug.LogError("Failed remove item");
        return false;
    }

    private void ChangeConsumableIndex(int amount)
    {
        consumableIndex += amount;

        if (consumableIndex >= consumables.Count)
        {
            consumableIndex = 0;
        }
        else if (consumableIndex < 0)
        {
            if (consumables.Count == 0)
            {
                consumableIndex = 0;
            }
            else 
            {
                consumableIndex = consumables.Count - 1;
            } 
        }

        OnConsumableIndexUpdate?.Invoke(consumableIndex);
    }

    private void UseSelectedConsumable()
    {
        if (consumables.Count == 0)
        {
            Debug.Log("No item to use");
            return;
        }

        for (int i = 0; i < consumables.Count; i++)
        {
            if (i == consumableIndex)
            {
                if (consumables[i].SOItem.GetType() == typeof(SOConsumable))
                {
                    if (consumables[i].UseConsumable(GetComponent<Health>()))
                    {
                        RemoveConsumable(consumables[i].SOItem);
                        Debug.Log(consumables[i].SOItem.name + " used");

                        // check if index is valid
                        ChangeConsumableIndex(0);

                        OnConsumableUse?.Invoke();
                    }
                    break;
                }
                else break;
            }
        }
    }

    public bool HaveKeycard(SOKeycard keycard)
    {
        foreach (Keycard i in keycards)
        {
            if (keycard.name == i.SOItem.name)
            {
                Debug.Log("Player has " + keycard.itemName);
                return true;
            }
        }
        Debug.Log("Player has not " + keycard.itemName);
        return false;
    }
}