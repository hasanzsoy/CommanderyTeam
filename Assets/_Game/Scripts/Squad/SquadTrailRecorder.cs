using System.Collections.Generic;
using UnityEngine;

public sealed class SquadTrailRecorder : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField, Min(0.01f)]
    private float recordDistance = 0.1f;

    [SerializeField, Min(10)]
    private int maxRecordedPoints = 300;

    private readonly List<Vector3> trailPoints = new();

    private Vector3 lastRecordedPosition;

    private void Awake()
    {
        lastRecordedPosition = transform.position;

        trailPoints.Add(transform.position);
    }

    private void LateUpdate()
    {
        RecordPosition();
    }

    public Vector3 GetPositionBehind(float distanceBehind)
    {
        if (trailPoints.Count == 0)
        {
            return transform.position;
        }

        float remainingDistance = distanceBehind;

        Vector3 previousPoint =  transform.position;

        for (int i = trailPoints.Count - 1; i >= 0; i--)
        {
            Vector3 currentPoint =  trailPoints[i];

            float segmentDistance =  Vector3.Distance(previousPoint,currentPoint);

            if (remainingDistance <= segmentDistance)
            {
                if (segmentDistance <= 0.001f)
                {
                    return currentPoint;
                }

                float t = remainingDistance / segmentDistance;

                return Vector3.Lerp(previousPoint,currentPoint,t);
            }

            remainingDistance -= segmentDistance;
            previousPoint = currentPoint;
        }

        return trailPoints[0];
    }

    private void RecordPosition()
    {
        float distance = Vector3.Distance(transform.position,lastRecordedPosition);

        if (distance < recordDistance)
        {
            return;
        }

        trailPoints.Add(transform.position);

        lastRecordedPosition = transform.position;

        if (trailPoints.Count > maxRecordedPoints)
        {
            trailPoints.RemoveAt(0);
        }
    }
}