using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        if (!ValidateReferences())
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.HealthChanged += UpdateHealthBar;
    }

    private void Start()
    {
        RefreshHealthBar();
    }

    private void OnDisable()
    {
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.HealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth,float maxHealth)
    {
        if (maxHealth <= 0f)
        {
            healthSlider.value = 0f;
            return;
        }

        healthSlider.value = currentHealth / maxHealth;
    }

    private void RefreshHealthBar()
    {
        UpdateHealthBar(playerHealth.CurrentHealth,playerHealth.MaxHealth);
    }

    private bool ValidateReferences()
    {
        if (playerHealth == null)
        {
            Debug.LogError($"{nameof(PlayerHealthUI)} requires a PlayerHealth reference.",this);

            return false;
        }

        if (healthSlider == null)
        {
            Debug.LogError($"{nameof(PlayerHealthUI)} requires a Slider reference.",this);

            return false;
        }

        return true;
    }
}