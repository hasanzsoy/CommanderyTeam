using UnityEngine;

[CreateAssetMenu(fileName = "SO_Player_Default",menuName = "CommanderyTeam/Data/Player Data")]
public sealed class PlayerDataSO : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField, Min(0f)] private float moveSpeed = 4f;
    [SerializeField, Min(0f)] private float rotationSpeed = 12f;

    [Header("Health Settings")]
    [SerializeField, Min(1f)] private float maxHealth = 150f;

    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float MaxHealth => maxHealth;
}