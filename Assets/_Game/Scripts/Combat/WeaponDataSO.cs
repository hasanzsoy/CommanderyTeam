using UnityEngine;

[CreateAssetMenu(fileName = "SO_Weapon_New",menuName = "CommanderyTeam/Data/Combat/Weapon Data")]
public sealed class WeaponDataSO : ScriptableObject
{
    [Header("Combat")]
    [SerializeField, Min(0f)]
    private float damage = 10f;

    [SerializeField, Min(0.1f)]
    private float fireRate = 2f;

    [SerializeField, Min(0.1f)]
    private float attackRange = 8f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField, Min(0.1f)]
    private float projectileSpeed = 14f;

    public float Damage => damage;
    public float FireRate => fireRate;
    public float AttackRange => attackRange;

    public GameObject ProjectilePrefab => projectilePrefab;
    public float ProjectileSpeed => projectileSpeed;
}