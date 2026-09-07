using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputReader))]
public sealed class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSO playerData;

    private Rigidbody playerRigidbody;
    private PlayerInputReader inputReader;

    private Vector3 moveDirection;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        inputReader = GetComponent<PlayerInputReader>();

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
        Vector2 input = inputReader.MoveInput;

        moveDirection = new Vector3(input.x,0f,input.y);

        moveDirection = Vector3.ClampMagnitude(moveDirection,1f);
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