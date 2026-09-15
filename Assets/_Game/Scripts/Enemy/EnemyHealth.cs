using System;
using UnityEngine;

public sealed class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private EnemyDataSO enemyData;

    public event Action<float, float> HealthChanged;
    public event Action<EnemyHealth> Died;

    public float CurrentHealth { get; private set; }

    public float MaxHealth =>enemyData != null? enemyData.MaxHealth: 0f;

    public bool IsDead { get; private set; }

    private void Awake()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        ResetHealth();
    }

    public void TakeDamage(float damageAmount)
    {
        if (IsDead || damageAmount <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Max(CurrentHealth - damageAmount,0f);

        HealthChanged?.Invoke(CurrentHealth,MaxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        if (enemyData == null)
        {
            return;
        }

        CurrentHealth = MaxHealth;
        IsDead = false;

        HealthChanged?.Invoke(CurrentHealth,MaxHealth);
    }

    private void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;

        Died?.Invoke(this);

        gameObject.SetActive(false);
    }

    private bool ValidateReferences()
    {
        if (enemyData != null)
        {
            return true;
        }

        Debug.LogError($"{nameof(EnemyHealth)} on {gameObject.name} " + $"requires an {nameof(EnemyDataSO)}.",this);

        return false;
    }

#if UNITY_EDITOR

    [ContextMenu("Debug/Take 10 Damage")]
    private void DebugTakeDamage()
    {
        TakeDamage(10f);
    }

#endif
}