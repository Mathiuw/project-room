using UnityEngine;
using UnityEngine.UI;

namespace MaiNull.UI
{
    public class UIPlayerStats : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider staminaBar;
        private Player _player;
        private PlayerRbMovement _playerRbMovement;

        private void Start()
        {
            _player = FindFirstObjectByType<Player>();

            if (!_player)
            {
                Debug.LogError("Cant find Player!");
                return;
            }

            healthBar.maxValue = _player.Health.MaxHealth;
            _player.Health.OnHealthChange += SetHealthUI;
            SetHealthUI(_player.Health.HealthAmount);

            _playerRbMovement = _player.GetComponent<PlayerRbMovement>();
            if (!_playerRbMovement)
            {
                Debug.LogError("PlayerRbMovement not found");
                return;
            }
            
            staminaBar.maxValue = _playerRbMovement.MaxStamina;
            SetStaminaUI(_playerRbMovement.Stamina);

            _playerRbMovement.OnStaminaUpdated += SetStaminaUI;
        }

        private void OnDisable()
        {
            if (_playerRbMovement)
            {
                _playerRbMovement.OnStaminaUpdated -= SetStaminaUI;
            }

            _player.Health.OnHealthChange -= SetHealthUI;
        }

        private void SetStaminaUI(float stamina)
        {
            if (!staminaBar) return;
            
            staminaBar.value = stamina;
            staminaBar.gameObject.SetActive(!Mathf.Approximately(staminaBar.value, staminaBar.maxValue));
        }

        private void SetHealthUI(int healthAmount)
        {
            if (!healthBar) return;
            
            healthBar.value = healthAmount;
        }
    }
}