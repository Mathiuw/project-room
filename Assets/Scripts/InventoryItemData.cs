using UnityEngine;

namespace MaiNull
{
    public abstract class InventoryItemData : ItemBaseData
    {
        [Header("Sprite and mesh")]
        public Sprite hotbarSprite;
        public GameObject itemPrefab;

        [Header("Stack")]
        public bool isStackable;
        public int maxStack;
    }
}