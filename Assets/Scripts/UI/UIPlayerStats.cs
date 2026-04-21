using UnityEngine;
using UnityEngine.UI;

namespace MaiNull.UI
{
    public class UIPlayerStats : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        [SerializeField] Slider staminaBar;

        Player player;
        PlayerRBMovement _playerRbMovement;

        void Start()
        {
            player = FindFirstObjectByType<Player>();

            if (!player)
            {
                Debug.LogError("Cant find Player!");
                return;
            }

            healthBar.maxValue = player.Health.MaxHealth;
            player.Health.OnHealthChange += SetHealthUI;
            SetHealthUI(player.Health.HealthAmount);

            _playerRbMovement = player.GetComponent<PlayerRBMovement>();
            if (_playerRbMovement)
            {
                staminaBar.maxValue = _playerRbMovement.MaxStamina;
                SetStaminaUI(_playerRbMovement.Stamina);

                _playerRbMovement.OnStaminaUpdated += SetStaminaUI;
            }
        }

        private void OnDisable()
        {
            if (_playerRbMovement)
            {
                _playerRbMovement.OnStaminaUpdated -= SetStaminaUI;
            }

            player.Health.OnHealthChange -= SetHealthUI;
        }

        void SetStaminaUI(float stamina)
        {
            staminaBar.value = stamina;

            if (staminaBar.value == staminaBar.maxValue) staminaBar.gameObject.SetActive(false);
            else staminaBar.gameObject.SetActive(true);
        }

        void SetHealthUI(int healthAmount)
        {
            healthBar.value = healthAmount;
        }
    }
}