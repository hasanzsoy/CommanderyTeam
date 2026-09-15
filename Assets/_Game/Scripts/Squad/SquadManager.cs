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

    private SquadTrailRecorder trailRecorder;

    public event Action SquadChanged;

    public IReadOnlyList<SquadMember> Members => members;

    public int MemberCount => members.Count;

    public bool IsFull =>
        squadSettings != null &&
        MemberCount >= squadSettings.MaxMemberCount;

    private void Start()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        if (!leader.TryGetComponent(out trailRecorder))
        {
            Debug.LogError($"{leader.name} requires a {nameof(SquadTrailRecorder)} component.",leader);

            enabled = false;
            return;
        }

        SpawnStartingMember();
    }

    public bool TryAddMember(SquadMemberDataSO memberData)
    {
        if (memberData == null)
        {
            Debug.LogWarning($"{nameof(SquadManager)} cannot add a null member.",this);

            return false;
        }

        if (IsFull)
        {
            Debug.LogWarning("Squad is full.",this);

            return false;
        }

        if (memberData.Prefab == null)
        {
            Debug.LogError($"{memberData.name} does not have a prefab assigned.",memberData);

            return false;
        }

        int formationIndex = members.Count;

        SquadMember newMember = CreateMember(memberData,formationIndex);

        if (newMember == null)
        {
            return false;
        }

        members.Add(newMember);

        RefreshFormation();

        SquadChanged?.Invoke();

        return true;
    }

    private void SpawnStartingMember()
    {
        if (startingMember == null)
        {
            Debug.LogWarning($"{nameof(SquadManager)} does not have a starting member.",this);

            return;
        }

        TryAddMember(startingMember);
    }

    private SquadMember CreateMember(SquadMemberDataSO memberData,int formationIndex)
    {
        Vector3 formationOffset = squadSettings.GetFormationOffset(formationIndex);

        Vector3 spawnPosition = leader.TransformPoint(formationOffset);

        GameObject instance = Instantiate(memberData.Prefab,spawnPosition,leader.rotation,memberParent);

        if (!instance.TryGetComponent(out SquadMember squadMember))
        {
            Debug.LogError($"{memberData.Prefab.name} requires a " + $"{nameof(SquadMember)} component.",instance);

            Destroy(instance);

            return null;
        }

        if (!instance.TryGetComponent(out SquadFollower squadFollower))
        {
            Debug.LogError($"{memberData.Prefab.name} requires a " + $"{nameof(SquadFollower)} component.",instance);

            Destroy(instance);

            return null;
        }

        squadMember.Initialize(memberData);

        squadFollower.Initialize(trailRecorder,memberData,formationOffset,squadSettings.MinimumLeaderDistance);

        return squadMember;
    }

    private void RefreshFormation()
    {
        for (int i = 0; i < members.Count; i++)
        {
            SquadMember member = members[i];

            if (member == null)
            {
                continue;
            }

            if (!member.TryGetComponent(out SquadFollower follower))
            {
                Debug.LogWarning($"{member.name} does not have a " + $"{nameof(SquadFollower)} component.",member);

                continue;
            }

            Vector3 formationOffset = squadSettings.GetFormationOffset(i);

            follower.SetFormationOffset(formationOffset);
        }
    }

    private bool ValidateReferences()
    {
        if (leader == null)
        {
            Debug.LogError($"{nameof(SquadManager)} requires a leader.",this);

            return false;
        }

        if (memberParent == null)
        {
            Debug.LogError($"{nameof(SquadManager)} requires a member parent.",this);

            return false;
        }

        if (squadSettings == null)
        {
            Debug.LogError($"{nameof(SquadManager)} requires a " + $"{nameof(SquadSettingsSO)}.",this);

            return false;
        }

        return true;
    }
}