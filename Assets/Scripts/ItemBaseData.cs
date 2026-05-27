using UnityEngine;

namespace MaiNull	
{
	public abstract class ItemBaseData : ScriptableObject
	{
        [Header("Name")]
        public string itemName = "Item Name";
    }
}