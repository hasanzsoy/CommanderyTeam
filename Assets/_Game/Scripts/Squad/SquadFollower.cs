using UnityEngine;

public sealed class SquadFollower : MonoBehaviour
{
    private SquadTrailRecorder trailRecorder;
    private SquadMemberDataSO memberData;

    private float followDistance;
    private float minimumLeaderDistance;
    private float fixedY;

    private bool isInitialized;

    public void Initialize(
        SquadTrailRecorder recorder,
        SquadMemberDataSO data,
        Vector3 formationOffset,
        float minLeaderDistance)
    {
        if (recorder == null)
        {
            Debug.LogError(
                $"{nameof(SquadFollower)} requires a " +
                $"{nameof(SquadTrailRecorder)}.",
                this);

            return;
        }

        if (data == null)
        {
            Debug.LogError(
                $"{nameof(SquadFollower)} requires " +
                $"{nameof(SquadMemberDataSO)}.",
                this);

            return;
        }

        trailRecorder = recorder;
        memberData = data;

        minimumLeaderDistance =
            Mathf.Max(0.1f, minLeaderDistance);

        fixedY = transform.position.y;

        SetFormationOffset(formationOffset);

        isInitialized = true;
    }

    public void SetFormationOffset(
        Vector3 formationOffset)
    {
        followDistance = Mathf.Max(
            Mathf.Abs(formationOffset.z),
            minimumLeaderDistance);
    }

    private void LateUpdate()
    {
        if (!isInitialized)
        {
            return;
        }

        FollowTrail();
    }

    private void FollowTrail()
    {
        Vector3 targetPosition =
            trailRecorder.GetPositionBehind(
                followDistance);

        // Rifleman yüksekliğini koru.
        targetPosition.y = fixedY;

        targetPosition =
            GetSafeTargetPosition(targetPosition);

        Vector3 previousPosition =
            transform.position;

        Vector3 nextPosition =
            Vector3.MoveTowards(
                transform.position,
                targetPosition,
                memberData.MoveSpeed * Time.deltaTime);

        nextPosition.y = fixedY;

        transform.position = nextPosition;

        EnforceMinimumLeaderDistance();

        Vector3 movementDirection =
            transform.position - previousPosition;

        RotateTowardsMovement(
            movementDirection);
    }

    private Vector3 GetSafeTargetPosition(
        Vector3 targetPosition)
    {
        Vector3 leaderPosition =
            trailRecorder.transform.position;

        leaderPosition.y = fixedY;

        Vector3 leaderToTarget =
            targetPosition - leaderPosition;

        leaderToTarget.y = 0f;

        if (leaderToTarget.sqrMagnitude >=
            minimumLeaderDistance *
            minimumLeaderDistance)
        {
            return targetPosition;
        }

        if (leaderToTarget.sqrMagnitude <= 0.0001f)
        {
            leaderToTarget =
                transform.position -
                leaderPosition;

            leaderToTarget.y = 0f;
        }

        if (leaderToTarget.sqrMagnitude <= 0.0001f)
        {
            leaderToTarget =
                -trailRecorder.transform.forward;

            leaderToTarget.y = 0f;
        }

        targetPosition =
            leaderPosition +
            leaderToTarget.normalized *
            minimumLeaderDistance;

        targetPosition.y = fixedY;

        return targetPosition;
    }

    private void EnforceMinimumLeaderDistance()
    {
        Vector3 leaderPosition =
            trailRecorder.transform.position;

        leaderPosition.y = fixedY;

        Vector3 offset =
            transform.position -
            leaderPosition;

        offset.y = 0f;

        float distance =
            offset.magnitude;

        if (distance >= minimumLeaderDistance)
        {
            return;
        }

        if (offset.sqrMagnitude <= 0.0001f)
        {
            offset =
                -trailRecorder.transform.forward;

            offset.y = 0f;
        }

        Vector3 safePosition =
            leaderPosition +
            offset.normalized *
            minimumLeaderDistance;

        safePosition.y = fixedY;

        transform.position = safePosition;
    }

    private void RotateTowardsMovement(
        Vector3 movementDirection)
    {
        movementDirection.y = 0f;

        if (movementDirection.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(
                movementDirection.normalized,
                Vector3.up);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                memberData.RotationSpeed *
                Time.deltaTime);
    }
}