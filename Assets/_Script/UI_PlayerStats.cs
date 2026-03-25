using UnityEngine;
using UnityEngine.UI;

namespace MaiNull
{
    public class UI_PlayerStats : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        [SerializeField] Slider staminaBar;

        Player player;
        PlayerMovement playerMovement;

        void Start()
        {
            player = FindFirstObjectByType<Player>();

            if (!player)
            {
                Debug.LogError("Cant find Player!");
                return;
            }

            healthBar.maxValue = player.health.MaxHealth;
            player.health.OnHealthChange += SetHealthUI;
            SetHealthUI(player.health.HealthAmount);

            playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement)
            {
                staminaBar.maxValue = playerMovement.MaxStamina;
                SetStaminaUI(playerMovement.Stamina);

                playerMovement.OnStaminaUpdated += SetStaminaUI;
            }
        }

        private void OnDisable()
        {
            if (playerMovement)
            {
                playerMovement.OnStaminaUpdated -= SetStaminaUI;
            }

            player.health.OnHealthChange -= SetHealthUI;
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