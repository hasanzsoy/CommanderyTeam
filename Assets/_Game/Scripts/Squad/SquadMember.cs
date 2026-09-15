using UnityEngine;

public sealed class SquadMember : MonoBehaviour
{
    public SquadMemberDataSO Data { get; private set; }

    public SquadMemberType MemberType =>Data != null? Data.MemberType: default;

    public FusionTier FusionTier =>Data != null? Data.FusionTier: default;

    public bool IsInitialized { get; private set; }

    public void Initialize(SquadMemberDataSO memberData)
    {
        if (memberData == null)
        {
            Debug.LogError($"{nameof(SquadMember)} on {gameObject.name} received null member data.",this);

            return;
        }

        Data = memberData;
        IsInitialized = true;

        gameObject.name = $"{memberData.DisplayName}_{memberData.FusionTier}";
    }
}