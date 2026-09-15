using UnityEngine;

[CreateAssetMenu(fileName = "SO_Squad_Default",menuName = "CommanderyTeam/Data/Squad/Squad Settings")]
public sealed class SquadSettingsSO : ScriptableObject
{
    [Header("Capacity")]
    [SerializeField, Min(1)] private int maxMemberCount = 6;

    [Header("Spawn Settings")]
    [SerializeField]
    private Vector3 initialSpawnOffset = new Vector3(0f, 0f, -1.5f);

    [Header("Formation Settings")]
    [SerializeField, Min(0.1f)] private float formationSpacing = 1.25f;

    public int MaxMemberCount => maxMemberCount;
    public Vector3 InitialSpawnOffset => initialSpawnOffset;
    public float FormationSpacing => formationSpacing;
}