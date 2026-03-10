using UnityEngine;

namespace MaiNull.Item
{
    public abstract class PickableItemData : ItemBaseData
    {
        [Header("Sprite and mesh")]
        public Sprite hotbarSprite;
        public GameObject itemPrefab;

        [Header("Stack")]
        public bool isStackable;
        public int maxStack;
    }
}