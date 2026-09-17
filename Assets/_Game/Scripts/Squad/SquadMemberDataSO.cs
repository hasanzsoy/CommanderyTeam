using UnityEngine;

[CreateAssetMenu(fileName = "SO_SquadMember_New", menuName = "CommanderyTeam/Data/Squad/Squad Member Data")]
public sealed class SquadMemberDataSO : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string displayName = "Squad Member";
    [SerializeField] private SquadMemberType memberType;
    [SerializeField] private FusionTier fusionTier = FusionTier.Tier1;

    [Header("Prefab")]
    [SerializeField] private GameObject prefab;

    [Header("Movement")]
    [SerializeField, Min(0f)]
    private float moveSpeed = 6f;

    [SerializeField, Min(0f)]
    private float rotationSpeed = 12f;

    [Header("Combat")]
    [SerializeField] private WeaponDataSO weaponData;

    public string DisplayName => displayName;
    public SquadMemberType MemberType => memberType;
    public FusionTier FusionTier => fusionTier;

    public GameObject Prefab => prefab;

    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;

    public WeaponDataSO WeaponData => weaponData;
}