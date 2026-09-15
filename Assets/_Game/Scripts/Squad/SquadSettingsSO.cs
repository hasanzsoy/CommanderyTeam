using UnityEngine;

[CreateAssetMenu(fileName = "SO_Squad_Default",menuName = "CommanderyTeam/Data/Squad/Squad Settings")]
public sealed class SquadSettingsSO : ScriptableObject
{
    [Header("Capacity")]
    [SerializeField, Min(1)]
    private int maxMemberCount = 6;

    [Header("Spawn Settings")]
    [SerializeField]
    private Vector3 initialSpawnOffset = new Vector3(0f, 1f, -1.5f);

    [Header("Follow Settings")]
    [SerializeField, Min(0.1f)]
    private float minimumLeaderDistance = 1.1f;
    public float MinimumLeaderDistance => minimumLeaderDistance;
    private Vector3[] formationOffsets =
    {
        new Vector3(0f, 0.85f, -1.5f),
        new Vector3(-0.8f, 1f, -2.4f),
        new Vector3(0.8f, 1f, -2.4f),
        new Vector3(-1.2f, 1f, -3.3f),
        new Vector3(0f, 1f, -3.3f),
        new Vector3(1.2f, 1f, -3.3f)
    };

    public int MaxMemberCount => maxMemberCount;

    public Vector3 InitialSpawnOffset => initialSpawnOffset;

    public Vector3 GetFormationOffset(int memberIndex)
    {
        if (formationOffsets == null || formationOffsets.Length == 0)
        {
            return initialSpawnOffset;
        }

        int safeIndex = Mathf.Clamp(memberIndex,0,formationOffsets.Length - 1);

        return formationOffsets[safeIndex];
    }
}