using UnityEngine;

public sealed class Projectile : MonoBehaviour
{
    private EnemyHealth target;

    private float damage;
    private float moveSpeed;

    private bool isInitialized;

    public void Initialize(EnemyHealth enemyTarget,float projectileDamage,float projectileMoveSpeed)
    {
        if (enemyTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        target = enemyTarget;
        damage = projectileDamage;
        moveSpeed = projectileMoveSpeed;

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        if (target == null || target.IsDead || !target.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetPosition = GetTargetPosition();

        Vector3 direction = targetPosition - transform.position;

        float step = moveSpeed * Time.deltaTime;

        if (direction.magnitude <= step)
        {
            HitTarget();
            return;
        }

        transform.position += direction.normalized * step;

        RotateTowards(direction);
    }

    private Vector3 GetTargetPosition()
    {
        if (target.TryGetComponent(out Collider targetCollider))
        {
            return targetCollider.bounds.center;
        }

        return target.transform.position;
    }

    private void HitTarget()
    {
        target.TakeDamage(damage);

        Destroy(gameObject);
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(direction.normalized,Vector3.up);
    }
}