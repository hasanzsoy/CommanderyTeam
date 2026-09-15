using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class SquadManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform leader;
    [SerializeField] private Transform memberParent;

    [Header("Data")]
    [SerializeField] private SquadSettingsSO squadSettings;

    [Header("Starting Squad")]
    [SerializeField] private SquadMemberDataSO startingMember;

    private readonly List<SquadMember> members = new();

    public event Action SquadChanged;

    public IReadOnlyList<SquadMember> Members => members;

    public int MemberCount => members.Count;

    public bool IsFull =>squadSettings != null && MemberCount >= squadSettings.MaxMemberCount;

    private void Start()
    {
        SpawnStartingMember();
    }

    public bool TryAddMember(SquadMemberDataSO memberData)
    {
        if (memberData == null)
        {
            Debug.LogWarning($"{nameof(SquadManager)} cannot add a null SquadMemberDataSO.",this);

            return false;
        }

        if (IsFull)
        {
            Debug.LogWarning("Squad is full. New member could not be added.",this);

            return false;
        }

        if (memberData.Prefab == null)
        {
            Debug.LogError($"{memberData.name} does not have a prefab assigned.",memberData);

            return false;
        }

        SquadMember newMember = CreateMember(memberData);

        if (newMember == null)
        {
            return false;
        }

        members.Add(newMember);

        SquadChanged?.Invoke();

        return true;
    }

    private void SpawnStartingMember()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        if (startingMember == null)
        {
            Debug.LogWarning($"{nameof(SquadManager)} does not have a starting squad member.",this);

            return;
        }

        TryAddMember(startingMember);
    }

    private SquadMember CreateMember(SquadMemberDataSO memberData)
    {
        Vector3 spawnPosition = leader.position + leader.TransformDirection(squadSettings.InitialSpawnOffset);

        GameObject instance = Instantiate(memberData.Prefab,spawnPosition,leader.rotation,memberParent);

        if (!instance.TryGetComponent(out SquadMember squadMember))
        {
            Debug.LogError($"{memberData.Prefab.name} requires a {nameof(SquadMember)} component.",instance);

            Destroy(instance);

            return null;
        }

        squadMember.Initialize(memberData);

        return squadMember;
    }

    private bool ValidateReferences()
    {
        if (leader == null)
        {
            Debug.LogError($"{nameof(SquadManager)} requires a leader Transform.",this);

            return false;
        }

        if (memberParent == null)
        {
            Debug.LogError($"{nameof(SquadManager)} requires a member parent Transform.",this);

            return false;
        }

        if (squadSettings == null)
        {
            Debug.LogError($"{nameof(SquadManager)} requires SquadSettingsSO.",this);

            return false;
        }

        return true;
    }
}