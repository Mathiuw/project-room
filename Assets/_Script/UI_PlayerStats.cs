using MaiNull.Player;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    PlayerMovement playerMovement;
    Health playerHealth;

    void Start() 
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (playerMovement)
        {
            staminaBar.maxValue = playerMovement.MaxStamina;
            SetStaminaUI(playerMovement.Stamina);

            playerMovement.OnStaminaUpdated += SetStaminaUI;


            playerHealth = playerMovement.GetComponent<Health>();

            if (playerHealth)
            {
                healthBar.maxValue = playerHealth.MaxHealth;
                SetHealthUI(playerHealth.HealthAmount);

                playerHealth.OnHealthUpdated += SetHealthUI;
            }
            else
            {
                Debug.LogError("Cant find player health");
            }
        }
        else Debug.LogError("Cant find PlayerMovement class");
    }

    private void OnDisable()
    {
        playerMovement.OnStaminaUpdated -= SetStaminaUI;
        playerHealth.OnHealthUpdated -= SetHealthUI;
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
