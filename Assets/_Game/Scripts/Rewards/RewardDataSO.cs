using UnityEngine;

[CreateAssetMenu(fileName = "SO_Reward_New",menuName = "CommanderyTeam/Data/Rewards/Reward Data")]
public sealed class RewardDataSO : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string displayName = "Reward";
    [SerializeField] private RewardType rewardType;
    [Header("Value")]
    [SerializeField, Min(0f)] private float amount;
    public string DisplayName => displayName;
    public RewardType RewardType => rewardType;
    public float Amount => amount;
}