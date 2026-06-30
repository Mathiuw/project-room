using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MaiNull.UI
{
	public class UIHud : MonoBehaviour
	{
		[SerializeField] private UIDocument healthBar;
		[SerializeField] private UIDocument staminaBar;
		private VisualElement _healthBarRoot;
		private VisualElement _staminaBarRoot;

		private void Awake()
		{
			_healthBarRoot = healthBar?.rootVisualElement;
			_staminaBarRoot = staminaBar?.rootVisualElement;
		}

		private void OnEnable()
		{
			Player player = FindFirstObjectByType<Player>();
			if (player == null) return;
			
			_healthBarRoot.Q<VisualElement>("HealthBar").dataSource = player;
			_staminaBarRoot.Q<VisualElement>("StaminaBar").dataSource = player;
		}

		private void Update()
		{
			
		}

		private void OnDisable()
		{
			_healthBarRoot.Q<VisualElement>("HealthBar").dataSource = null;
			_staminaBarRoot.Q<VisualElement>("StaminaBar").dataSource = null;
		}
	}
}
