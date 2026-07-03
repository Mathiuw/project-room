using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MaiNull.UI
{
	public class UIHud : MonoBehaviour
	{
		[SerializeField] private UIDocument healthBar;
		[SerializeField] private UIDocument staminaBar;
		[SerializeField] private UIDocument interactText;
		private VisualElement _healthBarRoot;
		private VisualElement _staminaBarRoot;
		private VisualElement _interactTextRoot;
		private Label _interactTextLabel;
		private Player _player;
		private Transform _playerCameraTransform;
		
		[Header("HUD Settings")]
		[SerializeField] private float tickRate = 0.1f;
		
		[Header("Interact Text Settings")]
		[SerializeField] private LayerMask interactMask;
		
		private void Awake()
		{
			_healthBarRoot = healthBar?.rootVisualElement;
			_staminaBarRoot = staminaBar?.rootVisualElement;
			_interactTextRoot =  interactText?.rootVisualElement;
		}

		private void OnEnable()
		{
			_player = FindFirstObjectByType<Player>();
			if (_player == null) return;

			if (Camera.main != null) _playerCameraTransform = Camera.main.transform;

			_healthBarRoot.Q<VisualElement>("HealthBar").dataSource = _player;
			_staminaBarRoot.Q<VisualElement>("StaminaBar").dataSource = _player;
			_interactTextLabel = _interactTextRoot.Q<Label>("InteractionText");
			_interactTextLabel.text = "";
			
			InvokeRepeating(nameof(UpdateUI), 0, tickRate);
		}

		private void UpdateUI()
		{
			if (!_player) return;

			if (_player.CurrentStamina >= _player.MaxStamina) {
				_staminaBarRoot.AddToClassList("hidden");
			} 
			else if (_staminaBarRoot.ClassListContains("hidden")) {
				_staminaBarRoot.RemoveFromClassList("hidden");
			}
			
			CheckInteractText();
		}

		private void CheckInteractText()
		{
			if (!_playerCameraTransform) return;
			
			Ray ray = new Ray(_playerCameraTransform.position, _playerCameraTransform.forward);
			// Debug.DrawRay(ray.origin, ray.direction * _player.InteractMaxDistance, Color.yellow, 1f);

			if (Physics.Raycast(ray, out RaycastHit hitInfo, _player.InteractMaxDistance, interactMask)) {
				if (hitInfo.collider != null && hitInfo.transform.TryGetComponent(out IUIName uiName)) {
					_interactTextLabel.text = uiName.readName;
					return;
				}
			}
			
			_interactTextLabel.text = "";
		}
		
		private void OnDisable()
		{
			CancelInvoke();
			
			_healthBarRoot.Q<VisualElement>("HealthBar").dataSource = null;
			_staminaBarRoot.Q<VisualElement>("StaminaBar").dataSource = null;
		}
	}
}
