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
    private WeaponDataSO weaponData;

    private EnemyHealth currentTarget;

    private float nextScanTime;
    private float nextFireTime;

    private void Start()
    {
        squadMember = GetComponent<SquadMember>();

        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        weaponData = squadMember.Data.WeaponData;
    }

    private void Update()
    {
        if (weaponData == null)
        {
            return;
        }

        UpdateTarget();

        TryFire();
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

        nextScanTime = Time.time + targetScanInterval;

        currentTarget = FindNearestTarget();
    }

    private EnemyHealth FindNearestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,weaponData.AttackRange,enemyLayer);

        EnemyHealth nearestTarget = null;
        float nearestDistanceSqr = float.MaxValue;

        foreach (Collider hit in hits)
        {
            if (!hit.TryGetComponent(out EnemyHealth enemyHealth))
            {
                continue;
            }

            if (enemyHealth.IsDead)
            {
                continue;
            }

            float distanceSqr = (enemyHealth.transform.position - transform.position).sqrMagnitude;

            if (distanceSqr >= nearestDistanceSqr)
            {
                continue;
            }

            nearestDistanceSqr = distanceSqr;
            nearestTarget = enemyHealth;
        }

        return nearestTarget;
    }

    private bool IsCurrentTargetValid()
    {
        if (currentTarget == null)
        {
            return false;
        }

        if (currentTarget.IsDead || !currentTarget.gameObject.activeInHierarchy)
        {
            return false;
        }

        float rangeSqr = weaponData.AttackRange * weaponData.AttackRange;

        float distanceSqr =(currentTarget.transform.position - transform.position).sqrMagnitude;

        return distanceSqr <= rangeSqr;
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

        nextFireTime = Time.time + (1f / weaponData.FireRate);
    }

    private void Fire()
    {
        GameObject projectileInstance = Instantiate(weaponData.ProjectilePrefab,firePoint.position,firePoint.rotation);

        if (!projectileInstance.TryGetComponent(out Projectile projectile))
        {
            Debug.LogError($"{weaponData.ProjectilePrefab.name} requires a " + $"{nameof(Projectile)} component.",projectileInstance);

            Destroy(projectileInstance);

            return;
        }

        projectile.Initialize(currentTarget,weaponData.Damage,weaponData.ProjectileSpeed);
    }

    private bool ValidateReferences()
    {
        if (firePoint == null)
        {
            Debug.LogError($"{nameof(SquadAutoAttack)} on {gameObject.name} " + "requires a FirePoint.", this);

            return false;
        }

        if (squadMember.Data == null)
        {
            Debug.LogError($"{nameof(SquadMember)} on {gameObject.name} " + "has not been initialized.",this);

            return false;
        }

        if (squadMember.Data.WeaponData == null)
        {
            Debug.LogError($"{squadMember.Data.name} requires WeaponData.", squadMember.Data);

            return false;
        }

        if (squadMember.Data.WeaponData.ProjectilePrefab == null)
        {
            Debug.LogError($"{squadMember.Data.WeaponData.name} requires " + "a projectile prefab.",squadMember.Data.WeaponData);

            return false;
        }

        return true;
    }
}