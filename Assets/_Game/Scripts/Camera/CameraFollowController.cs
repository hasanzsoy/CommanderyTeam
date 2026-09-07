using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraFollowController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform followTarget;
    [SerializeField] private CameraDataSO cameraData;

    private Camera targetCamera;
    private Vector3 followVelocity;

    private void Awake()
    {
        targetCamera = GetComponent<Camera>();

        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        ApplyCameraSettings();
    }

    private void Start()
    {
        SnapToTarget();
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private bool ValidateReferences()
    {
        if (followTarget == null)
        {
            Debug.LogError($"{nameof(CameraFollowController)} on {gameObject.name} " + "requires a follow target.", this);

            return false;
        }

        if (cameraData == null)
        {
            Debug.LogError($"{nameof(CameraFollowController)} on {gameObject.name} " + "requires a CameraDataSO.",this);

            return false;
        }

        return true;
    }

    private void ApplyCameraSettings()
    {
        targetCamera.fieldOfView = cameraData.FieldOfView;

        transform.rotation = Quaternion.Euler(cameraData.RotationEuler);
    }

    private void SnapToTarget()
    {
        transform.position = followTarget.position + cameraData.PositionOffset;
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = followTarget.position + cameraData.PositionOffset;

        transform.position = Vector3.SmoothDamp(transform.position,targetPosition,ref followVelocity,cameraData.FollowSmoothTime);
    }
}