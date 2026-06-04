using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace MaiNull
{
    public enum EAmmoType 
    {
        SmallAmmo,
        LargeAmmo,
        ShellAmmo
    }

    public class Inventory : MonoBehaviour
    {
        [field: SerializeField] public List<Consumable> Consumables { get; private set; } = new List<Consumable>();
        [field: SerializeField] public List<PickableKeycard> Keycards { get; private set; } = new List<PickableKeycard>();
        public int ConsumableIndex { get; set; } = 0;

        [Header("Ammo Inventory")]
        public int SmallAmmoAmount { get; private set; } = 0;
        public int LargeAmmoAmount { get; private set; } = 0;
        public int ShellAmmoAmount { get; private set; } = 0;

        public event Action<Consumable> OnConsumableAdd;
        public event Action OnConsumableUse;
        public event Action<int> OnConsumableIndexUpdate;

        public event Action<PickableKeycard> OnKeycardAdd;

        public event Action OnAmmoCountUpdate;

        private void Update()
        {
            switch (Mouse.current.scroll.y.ReadValue())
            {
                // Scroll consumables input
                case > 0f:
                    ChangeConsumableIndex(1);
                    break;
                case < 0f:
                    ChangeConsumableIndex(-1);
                    break;
            }
        }

        public int GetAmmoAmountByType(EAmmoType ammoType) 
        {
            switch (ammoType)
            {
                case EAmmoType.SmallAmmo:
                    return SmallAmmoAmount;
                case EAmmoType.LargeAmmo:
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
                case EAmmoType.SmallAmmo:
                    SmallAmmoAmount += amount;
                    break;
                case EAmmoType.LargeAmmo:
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
                case EAmmoType.SmallAmmo:
                    SmallAmmoAmount -= amount;
                    SmallAmmoAmount = Mathf.Clamp(SmallAmmoAmount, 0, 999);
                    break;
                case EAmmoType.LargeAmmo:
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

        public bool AddItem(PickableItem item)
        {
            if (item.GetType() == typeof(Consumable))
            {
                for (int i = 0; i < Consumables.Count; i++)
                {
                    // Check if already have the item
                    if (item.PickableItemData.itemName == Consumables[i].PickableItemData.itemName)
                    {
                        // If have the item, check if you have the max amount
                        if (Consumables[i].PickableItemData.isStackable && Consumables[i].Amount < Consumables[i].PickableItemData.maxStack)
                        {
                            Consumable consumable = (Consumable)item;

                            // Increase item quantity
                            Consumables[i].Amount += consumable.Amount;
                            OnConsumableAdd?.Invoke(Consumables[i]);
                            return true;
                        }
                        else
                        {
                            Debug.Log(name + " have the max amount of " + Consumables[i].PickableItemData.itemName);
                            return false;
                        }
                    }
                }

                // Add new item
                Consumables.Add((Consumable)item);
                OnConsumableAdd?.Invoke((Consumable)item);
                ;
                return true;
            }
            else if (item.GetType() == typeof(PickableKeycard))
            {
                Keycards.Add((PickableKeycard)item);
                OnKeycardAdd?.Invoke((PickableKeycard)item);
                return true;
            }

            return false;
        }

        public bool RemoveConsumable(InventoryItemData item)
        {
            for (int i = 0; i < Consumables.Count; i++)
            {
                if (Consumables[i].PickableItemData.itemName == item.itemName)
                {
                    Consumables[i].Amount--;

                    if (Consumables[i].Amount == 0)
                    {
                        Consumables.RemoveAt(i);
                    }
                    return true;
                }
            }

            Debug.LogError("Failed remove item");
            return false;
        }

        private void ChangeConsumableIndex(int amount)
        {
            ConsumableIndex += amount;

            if (ConsumableIndex >= Consumables.Count)
            {
                ConsumableIndex = 0;
            }
            else if (ConsumableIndex < 0)
            {
                if (Consumables.Count == 0)
                {
                    ConsumableIndex = 0;
                }
                else 
                {
                    ConsumableIndex = Consumables.Count - 1;
                } 
            }

            OnConsumableIndexUpdate?.Invoke(ConsumableIndex);
        }

        private void UseSelectedConsumable(InputAction.CallbackContext obj)
        {
            if (Consumables.Count == 0)
            {
                Debug.Log("No item to use");
                return;
            }

            for (int i = 0; i < Consumables.Count; i++)
            {
                if (i == ConsumableIndex)
                {
                    if (Consumables[i].PickableItemData.GetType() == typeof(HealingItemData))
                    {
                        if (Consumables[i].UseConsumable(GetComponent<Health>()))
                        {
                            RemoveConsumable(Consumables[i].PickableItemData);
                            Debug.Log(Consumables[i].PickableItemData.name + " used");

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

        public bool HaveKeycard(KeycardData keycard)
        {
            if (Keycards.Any(i => keycard.name == i.PickableItemData.name))
            {
                Debug.Log("Player has " + keycard.itemName);
                return true;
            }
            Debug.Log("Player has not " + keycard.itemName);
            return false;
        }
    }
}