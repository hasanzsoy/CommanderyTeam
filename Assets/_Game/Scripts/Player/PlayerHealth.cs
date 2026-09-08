using System;
using UnityEngine;

public sealed class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private PlayerDataSO playerData;

    public event Action<float, float> HealthChanged;
    public event Action Died;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => playerData != null ? playerData.MaxHealth : 0f;

    public bool IsDead { get; private set; }

    private void Awake()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        InitializeHealth();
    }

    public void TakeDamage(float damageAmount)
    {
        if (IsDead || damageAmount <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Max(CurrentHealth - damageAmount,0f);

        HealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (IsDead || healAmount <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Min(CurrentHealth + healAmount,MaxHealth);

        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void RestoreFullHealth()
    {
        if (IsDead)
        {
            return;
        }

        CurrentHealth = MaxHealth;

        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void InitializeHealth()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;
    }

    private void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;

        Died?.Invoke();
    }
#if UNITY_EDITOR

    [ContextMenu("Debug/Take 25 Damage")]
    private void DebugTakeDamage()
    {
        TakeDamage(25f);
    }

    [ContextMenu("Debug/Heal 25")]
    private void DebugHeal()
    {
        Heal(25f);
    }

#endif
    private bool ValidateReferences()
    {
        if (playerData != null)
        {
            return true;
        }

        Debug.LogError($"{nameof(PlayerHealth)} on {gameObject.name} requires a PlayerDataSO.",this);

        return false;
    }
}