using UnityEngine;
[CreateAssetMenu(fileName = "SO_Player_Default", menuName = "CommanderyTeam/Data/Player Data")]
public sealed class PlayerDataSO : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField,Min(0f)] private float moveSpeed=5f;
    [SerializeField,Min(0f)] private float rotationSpeed=12f;

    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
}