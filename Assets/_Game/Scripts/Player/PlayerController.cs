using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSO playerData;

    private Rigidbody playerRigidbody;
    private Vector3 moveDirection;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        if (playerData == null)
        {
            Debug.LogError($"{nameof(PlayerController)} on {gameObject.name} requires a PlayerDataSO.",this);

            enabled = false;
            return;
        }

        playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        ReadMovementInput();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void ReadMovementInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput,0f,verticalInput);

        moveDirection = Vector3.ClampMagnitude(inputDirection, 1f);
    }

    private void Move()
    {
        Vector3 targetVelocity = moveDirection * playerData.MoveSpeed;

        playerRigidbody.linearVelocity = new Vector3(targetVelocity.x,playerRigidbody.linearVelocity.y,targetVelocity.z);
    }

    private void Rotate()
    {
        if (moveDirection.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection,Vector3.up);

        Quaternion smoothRotation = Quaternion.Slerp(playerRigidbody.rotation,targetRotation,playerData.RotationSpeed * Time.fixedDeltaTime);

        playerRigidbody.MoveRotation(smoothRotation);
    }
}