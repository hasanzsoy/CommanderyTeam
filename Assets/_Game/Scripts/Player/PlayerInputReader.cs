using UnityEngine;

public sealed class PlayerInputReader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatingJoystick floatingJoystick;

    [Header("Input Settings")]
    [SerializeField, Range(0f, 0.5f)]
    private float joystickDeadZone = 0.1f;

    [Header("Editor Settings")]
    [SerializeField] private bool enableKeyboardInput = true;

    public Vector2 MoveInput { get; private set; }

    private void Update()
    {
        Vector2 joystickInput = ReadJoystickInput();

        if (joystickInput != Vector2.zero)
        {
            MoveInput = joystickInput;
            return;
        }

        MoveInput = ReadKeyboardInput();
    }

    private Vector2 ReadKeyboardInput()
    {
        if (!enableKeyboardInput)
        {
            return Vector2.zero;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontal -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontal += 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            vertical -= 1f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            vertical += 1f;
        }

        return Vector2.ClampMagnitude(
            new Vector2(horizontal, vertical),
            1f);
    }

    private Vector2 ReadJoystickInput()
    {
        if (floatingJoystick == null)
        {
            return Vector2.zero;
        }

        Vector2 joystickInput = floatingJoystick.Input;

        if (joystickInput.magnitude < joystickDeadZone)
        {
            return Vector2.zero;
        }

        return Vector2.ClampMagnitude(
            joystickInput,
            1f);
    }
}