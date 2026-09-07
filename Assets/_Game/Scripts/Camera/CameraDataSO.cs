using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_Camera_Default",
    menuName = "CommanderyTeam/Data/Camera Data")]
public sealed class CameraDataSO : ScriptableObject
{
    [Header("Position Settings")]
    [SerializeField]
    private Vector3 positionOffset = new Vector3(0f, 10f, -8f);

    [SerializeField, Min(0.01f)]
    private float followSmoothTime = 0.15f;

    [Header("Rotation Settings")]
    [SerializeField]
    private Vector3 rotationEuler = new Vector3(45f, 0f, 0f);

    [Header("Lens Settings")]
    [SerializeField, Range(20f, 90f)]
    private float fieldOfView = 45f;

    public Vector3 PositionOffset => positionOffset;
    public float FollowSmoothTime => followSmoothTime;
    public Vector3 RotationEuler => rotationEuler;
    public float FieldOfView => fieldOfView;
}