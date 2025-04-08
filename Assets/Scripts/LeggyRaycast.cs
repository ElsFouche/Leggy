using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggyRaycast : MonoBehaviour
{
    public Transform IKTarget; // The IK target to move/rotate

    // Detection distances (trigger movement/rotation)
    public float detectDistanceX = 1f;
    public float detectDistanceY = 1f;
    public float detectDistanceZ = 1f;

    // Casting distances (ray length, should be larger than detectDistance)
    public float castDistanceX = 1.5f;
    public float castDistanceY = 1.5f;
    public float castDistanceZ = 1.5f;

    public float rotationAdjustment = 10f; // Rotation angle when too close on +X
    public float moveAdjustment = 0.1f; // Movement adjustment for Y/Z

    public float moveSpeed = 5f; // Speed for smooth movement
    public float rotationSpeed = 5f; // Speed for smooth rotation

    public LayerMask obstacleLayer; // Define which layers should be detected

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Vector3 lastKnownPosition;  // Track the last known position
    private Quaternion lastKnownRotation; // Track the last known rotation

    void Start()
    {
        if (IKTarget != null)
        {
            targetRotation = IKTarget.rotation;
            targetPosition = IKTarget.position;
            lastKnownPosition = targetPosition;
            lastKnownRotation = targetRotation;
        }
    }

    void Update()
    {
        if (IKTarget == null) return;

        // Keep track of the last known position and rotation
        lastKnownPosition = IKTarget.position;
        lastKnownRotation = IKTarget.rotation;

        // Perform raycast checks and adjust the IK target if needed
        CheckAndAdjustIK();

        // Smoothly apply the position and rotation if necessary
        // Clamp X to 0 but allow Y and Z to adjust
        targetPosition = new Vector3(0f, targetPosition.y, targetPosition.z);

        IKTarget.position = Vector3.MoveTowards(IKTarget.position, targetPosition, moveSpeed * Time.deltaTime);
        IKTarget.rotation = Quaternion.RotateTowards(IKTarget.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void CheckAndAdjustIK()
    {
        Vector3[] directions = {
            transform.right,  // +X (Rotate)
            transform.up,     // +Y (Move Z)
            -transform.up,    // -Y (Move Z)
            transform.forward, // +Z (Move Y)
            -transform.forward // -Z (Move Y)
        };

        float[] detectDistances = { detectDistanceX, detectDistanceY, detectDistanceY, detectDistanceZ, detectDistanceZ };
        float[] castDistances = { castDistanceX, castDistanceY, castDistanceY, castDistanceZ, castDistanceZ };

        bool adjustmentsMade = false;  // Flag to track if any adjustments were made

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 rayStart = transform.position;
            Vector3 rayDirection = directions[i];
            float rayDetectDistance = detectDistances[i];
            float rayCastDistance = castDistances[i];

            RaycastHit hit;
            Debug.DrawRay(rayStart, rayDirection * rayCastDistance, Color.yellow);

            if (Physics.Raycast(rayStart, rayDirection, out hit, rayCastDistance, obstacleLayer))
            {
                float hitDistance = hit.distance;

                if (hitDistance <= rayDetectDistance)
                {
                    Debug.DrawRay(rayStart, rayDirection * hitDistance, Color.red);
                    Debug.Log($"[LeggyRaycast] HIT: {hit.collider.gameObject.name} at {hitDistance}m in {rayDirection}");
                    AdjustIKTarget(rayDirection, i);
                    adjustmentsMade = true;
                }
                else
                {
                    Debug.DrawRay(rayStart, rayDirection * hitDistance, Color.blue);
                }
            }
        }

        // Only update the target position/rotation if any adjustments were made
        if (!adjustmentsMade)
        {
            targetPosition = lastKnownPosition;  // Keep the target position from the last frame
            targetRotation = lastKnownRotation;  // Keep the target rotation from the last frame
        }
    }

    void AdjustIKTarget(Vector3 direction, int directionIndex)
    {
        if (IKTarget == null) return;

        switch (directionIndex)
        {
            case 0: // +X ? Rotate to avoid
                targetRotation *= Quaternion.Euler(0, rotationAdjustment * Time.deltaTime, 0);
                break;

            case 1: // +Y ? Move along Z
            case 2: // -Y ? Move along Z
                targetPosition += new Vector3(0, 0, -direction.z * moveAdjustment * Time.deltaTime);
                break;

            case 3: // +Z ? Move upward (Y)
            case 4: // -Z ? Move upward (Y)
                targetPosition += new Vector3(0, -direction.z * moveAdjustment * Time.deltaTime, 0);
                break;
        }
    }
}
