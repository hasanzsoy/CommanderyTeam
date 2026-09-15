using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy_New",menuName = "CommanderyTeam/Data/Enemy/Enemy Data")]
public sealed class EnemyDataSO : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string displayName = "Enemy";
    [SerializeField] private EnemyType enemyType;

    [Header("Health")]
    [SerializeField, Min(1f)]
    private float maxHealth = 40f;

    [Header("Movement")]
    [SerializeField, Min(0f)]
    private float moveSpeed = 2.5f;

    [Header("Combat")]
    [SerializeField, Min(0f)]
    private float attackDamage = 10f;

    [SerializeField, Min(0.1f)]
    private float attackRange = 1.1f;

    [SerializeField, Min(0.01f)]
    private float attackCooldown = 1f;

    public string DisplayName => displayName;
    public EnemyType EnemyType => enemyType;

    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;

    public float AttackDamage => attackDamage;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
}