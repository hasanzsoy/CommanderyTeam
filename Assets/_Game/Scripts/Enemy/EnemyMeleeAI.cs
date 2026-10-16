using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyHealth))]
public sealed class EnemyMeleeAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerHealth targetPlayer;
    private Rigidbody enemyRigidbody;
    private EnemyHealth enemyHealth;
    private EnemyDataSO enemyData;
    private float nextAttackTime;

    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyData = enemyHealth.Data;
        if (!ValidateEnemyData())
        {
            enabled = false;
            return;
        }
        ConfigureRigidbody();
    }

    private void Start()
    {
        if (targetPlayer != null)
        {
            return;
        }
        Debug.LogError($"{nameof(EnemyMeleeAI)} on {gameObject.name} " +"requires a PlayerHealth target.",this);
        enabled = false;
    }

    private void Update()
    {
        if (!CanAct())
        {
            return;
        }
        TryAttackPlayer();
    }

    private void FixedUpdate()
    {
        if (!CanAct())
        {
            return;
        }
        RotateTowardsPlayer();
        if (!IsPlayerInAttackRange())
        {
            MoveTowardsPlayer();
        }
    }

    public void Initialize(PlayerHealth playerHealth)
    {
        if (playerHealth == null)
        {
            Debug.LogError($"{nameof(EnemyMeleeAI)} received a null PlayerHealth.",this);
            return;
        }
        targetPlayer = playerHealth;
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }
        Vector3 movement = direction.normalized * enemyData.MoveSpeed * Time.fixedDeltaTime;
        Vector3 nextPosition = enemyRigidbody.position + movement;
        enemyRigidbody.MovePosition(nextPosition);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();

        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized,Vector3.up);
        Quaternion smoothRotation = Quaternion.Slerp(enemyRigidbody.rotation,targetRotation,enemyData.RotationSpeed * Time.fixedDeltaTime);
        enemyRigidbody.MoveRotation(smoothRotation);
    }

    private void TryAttackPlayer()
    {
        if (!IsPlayerInAttackRange())
        {
            return;
        }
        if (Time.time < nextAttackTime)
        {
            return;
        }
        targetPlayer.TakeDamage(enemyData.AttackDamage);
        nextAttackTime = Time.time + enemyData.AttackCooldown;
    }

    private bool IsPlayerInAttackRange()
    {
        Vector3 direction = GetDirectionToPlayer();
        float attackRangeSqr = enemyData.AttackRange * enemyData.AttackRange;
        return direction.sqrMagnitude <= attackRangeSqr;
    }

    private Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = targetPlayer.transform.position - transform.position;
        direction.y = 0f;
        return direction;
    }

    private bool CanAct()
    {
        if (enemyHealth.IsDead)
        {
            return false;
        }
        if (targetPlayer == null)
        {
            return false;
        }
        if (targetPlayer.IsDead)
        {
            return false;
        }
        return true;
    }

    private void ConfigureRigidbody()
    {
        enemyRigidbody.useGravity = false;
        enemyRigidbody.isKinematic = true;
        enemyRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        enemyRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private bool ValidateEnemyData()
    {
        if (enemyData != null)
        {
            return true;
        }
        Debug.LogError($"{nameof(EnemyMeleeAI)} on {gameObject.name} " + "could not find EnemyDataSO.",this);
        return false;
    }
}