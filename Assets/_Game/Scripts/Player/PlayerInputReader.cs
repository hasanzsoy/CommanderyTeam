using UnityEngine;

public sealed class PlayerInputReader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatingJoystick floatingJoystick;

    [Header("Debug / Editor")]
    [SerializeField] private bool enableKeyboardInput = true;

    public Vector2 MoveInput { get; private set; }

    private void Update()
    {
        Vector2 keyboardInput = ReadKeyboardInput();
        Vector2 joystickInput = ReadJoystickInput();

        MoveInput = joystickInput.sqrMagnitude > 0.001f
            ? joystickInput
            : keyboardInput;
    }

    private Vector2 ReadKeyboardInput()
    {
        if (!enableKeyboardInput)
        {
            return Vector2.zero;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

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

        return floatingJoystick.Input;
    }
}