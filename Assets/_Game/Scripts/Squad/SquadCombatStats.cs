using UnityEngine;

public sealed class SquadCombatStats : MonoBehaviour
{
    public float DamageMultiplier { get; private set; } = 1f;
    public void AddDamagePercent(float percentAmount)
    {
        if (percentAmount <= 0f)
        {
            return;
        }
        float multiplierIncrease = percentAmount / 100f;
        DamageMultiplier += multiplierIncrease;
    }
}