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
		public int CurrentIndex {
			get => _currentIndex;
			set {
				_currentIndex = value;
                
				if (_currentIndex >= _items.Count)
				{
					_currentIndex = 0;
				}
				else if (_currentIndex <= 0 && _items.Count > 0)
				{
					_currentIndex = _items.Count - 1;
				}
				else if (_items.Count == 0) {
					_currentIndex = 0;
				}
			}
		}

		public event Action<T> OnItemAdded;
		public event Action OnItemRemoved;
		public event Action<T> OnItemChange;
		public event Action<int> OnSizeChanged;
		
		public Inventory (int inventorySize)
		{
			_inventorySize = inventorySize;
		}

		public void IncreaseIndex()
		{
			int beforeChangeIndex = _currentIndex;
			_currentIndex++;

			if (_currentIndex >= _items.Count)
			{
				_currentIndex = 0;
			}
            
			if (_currentIndex == beforeChangeIndex) return;
			OnItemChange?.Invoke(CurrentItem);
		} 

		public void DecreaseIndex()
		{
			int beforeChangeIndex = _currentIndex;
            
			_currentIndex--;
			if (_currentIndex <= 0 && _items.Count > 0)
			{
				_currentIndex = _items.Count - 1;
			}
			else _currentIndex = 0;

			if (_currentIndex == beforeChangeIndex) return;
			OnItemChange?.Invoke(CurrentItem);
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
			OnItemRemoved?.Invoke();
			return true;
		}

		public bool RemoveCurrentItem()
		{
			if (_items.Count == 0) return false;

			_items.Remove(CurrentItem);
			OnItemRemoved?.Invoke();
			return true;
		}
		
		public void AddInventorySize (int amount)
		{
			_inventorySize += amount;
			OnSizeChanged?.Invoke(_inventorySize);
		}

		public void RemoveInventorySize (int amount)
		{
			_inventorySize -= amount;
			OnSizeChanged?.Invoke(_inventorySize);
		}
	}
}