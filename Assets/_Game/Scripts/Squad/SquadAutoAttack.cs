using UnityEngine;

[RequireComponent(typeof(SquadMember))]
public sealed class SquadAutoAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;

    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField, Min(0.05f)]
    private float targetScanInterval = 0.2f;

    private SquadMember squadMember;
    private SquadCombatStats squadCombatStats;
    private WeaponDataSO weaponData;

    private EnemyHealth currentTarget;

    private float nextScanTime;
    private float nextFireTime;

    private bool isInitialized;

    private void Awake()
    {
        squadMember =
            GetComponent<SquadMember>();
    }

    private void Start()
    {
        TryInitialize();
    }

    private void Update()
    {
        if (!isInitialized)
        {
            TryInitialize();
            return;
        }

        UpdateTarget();
        TryFire();
    }

    private void TryInitialize()
    {
        if (isInitialized)
        {
            return;
        }

        if (squadMember == null)
        {
            return;
        }

        if (!squadMember.IsInitialized)
        {
            return;
        }

        if (squadMember.Data == null)
        {
            return;
        }

        weaponData =
            squadMember.Data.WeaponData;

        if (weaponData == null)
        {
            Debug.LogError(
                $"{squadMember.Data.name} requires WeaponData.",
                this);

            return;
        }

        if (weaponData.ProjectilePrefab == null)
        {
            Debug.LogError(
                $"{weaponData.name} requires a projectile prefab.",
                this);

            return;
        }

        if (firePoint == null)
        {
            Debug.LogError(
                $"{nameof(SquadAutoAttack)} on {gameObject.name} " +
                "requires a FirePoint.",
                this);

            return;
        }

        squadCombatStats =
            GetComponentInParent<SquadCombatStats>();

        if (squadCombatStats == null)
        {
            Debug.LogError(
                $"{nameof(SquadAutoAttack)} could not find " +
                $"{nameof(SquadCombatStats)} in the parent hierarchy.",
                this);

            return;
        }

        isInitialized = true;
    }

    private void UpdateTarget()
    {
        if (IsCurrentTargetValid())
        {
            return;
        }

        if (Time.time < nextScanTime)
        {
            return;
        }

        nextScanTime =
            Time.time + targetScanInterval;

        currentTarget =
            FindNearestTarget();
    }

    private EnemyHealth FindNearestTarget()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                weaponData.AttackRange,
                enemyLayer);

        EnemyHealth nearestTarget = null;

        float nearestDistanceSqr =
            float.MaxValue;

        foreach (Collider hit in hits)
        {
            if (!hit.TryGetComponent(
                    out EnemyHealth enemyHealth))
            {
                continue;
            }

            if (enemyHealth.IsDead)
            {
                continue;
            }

            float distanceSqr =
                (enemyHealth.transform.position -
                 transform.position).sqrMagnitude;

            if (distanceSqr >= nearestDistanceSqr)
            {
                continue;
            }

            nearestDistanceSqr =
                distanceSqr;

            nearestTarget =
                enemyHealth;
        }

        return nearestTarget;
    }

    private bool IsCurrentTargetValid()
    {
        if (currentTarget == null)
        {
            return false;
        }

        if (currentTarget.IsDead)
        {
            return false;
        }

        if (!currentTarget.gameObject.activeInHierarchy)
        {
            return false;
        }

        float attackRangeSqr =
            weaponData.AttackRange *
            weaponData.AttackRange;

        float distanceSqr =
            (currentTarget.transform.position -
             transform.position).sqrMagnitude;

        return distanceSqr <= attackRangeSqr;
    }

    private void TryFire()
    {
        if (!IsCurrentTargetValid())
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        Fire();

        nextFireTime =
            Time.time +
            (1f / weaponData.FireRate);
    }

    private void Fire()
    {
        GameObject projectileInstance =
            Instantiate(
                weaponData.ProjectilePrefab,
                firePoint.position,
                firePoint.rotation);

        if (!projectileInstance.TryGetComponent(
                out Projectile projectile))
        {
            Debug.LogError(
                $"{weaponData.ProjectilePrefab.name} requires a " +
                $"{nameof(Projectile)} component.",
                projectileInstance);

            Destroy(projectileInstance);

            return;
        }

        float finalDamage =
            weaponData.Damage *
            squadCombatStats.DamageMultiplier;
        Debug.Log($"Final Damage: {finalDamage}");

        projectile.Initialize(
            currentTarget,
            finalDamage,
            weaponData.ProjectileSpeed);
    }
}