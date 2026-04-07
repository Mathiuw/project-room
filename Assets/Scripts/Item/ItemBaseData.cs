using UnityEngine;

namespace MaiNull.Item	
{
	public abstract class ItemBaseData : ScriptableObject
	{
        [Header("Name")]
        public string itemName = "Item Name";
    }
}