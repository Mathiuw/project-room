using System;
using System.Collections.Generic;

namespace MaiNull
{
	public class Inventory<T>
	{
		private readonly List<T> _items = new List<T>();
		private int _inventorySize;
		private int _currentIndex = 0;

		public T CurrentItem => _items[_currentIndex];
		public int CurrentIndex { get => _currentIndex; set => _currentIndex = value; }

		public event Action<T> OnItemAdded;
		public event Action<T> OnItemRemoved;
		public event Action<int> OnSizeChanged;
		
		public Inventory (int inventorySize, int currentIndex)
		{
			_inventorySize = inventorySize;
			_currentIndex = currentIndex;
		}

		public bool AddItem (T item)
		{
			if (_items.Count + 1 > _inventorySize) return false;

			_items.Add(item);
			OnItemAdded?.Invoke(item);
			return true;
		}

		public bool RemoveItem (T item)
		{
			if (_items.Count == 0) return false;

			_items.Remove(item);
			OnItemRemoved?.Invoke(item);
			return true;
		}

		public void AddInventoryAmount (int amount)
		{
			_inventorySize += amount;
			OnSizeChanged?.Invoke(_inventorySize);
		}

		public void RemoveInventoryAmount (int amount)
		{
			_inventorySize -= amount;
			OnSizeChanged?.Invoke(_inventorySize);
		}
	}
}