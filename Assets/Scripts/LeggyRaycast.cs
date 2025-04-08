using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggyRaycast : MonoBehaviour
{
    public Transform IKTarget; // The IK target to move/rotate

    public float detectDistanceX = 1f;
    public float detectDistanceY = 1f;
    public float detectDistanceZ = 1f;

    public float castDistanceX = 1.5f;
    public float castDistanceY = 1.5f;
    public float castDistanceZ = 1.5f;

    public float rotationAdjustment = 10f; // Rotation angle when too close on +X
    public float moveAdjustment = 0.1f; // Movement adjustment for Y/Z

    public float moveSpeed = 5f; // Speed for smooth movement
    public float rotationSpeed = 5f; // Speed for smooth rotation

    public LayerMask obstacleLayer; // Define which layers should be detected

    private float rotationCooldownTimer = 0f;
    public float rotationCooldownDuration = 0.25f;
    public float rotationBuffer = 0.05f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Vector3 lastKnownPosition;  // Track the last known position
    private Quaternion lastKnownRotation; // Track the last known rotation

    // Block movement flags
    private bool blockZMovement = false;
    private bool blockYMovement = false;

    void Start()
    {
        if (IKTarget != null)
        {
            targetRotation = IKTarget.rotation;
            targetPosition = IKTarget.position;

            targetRotation = IKTarget.localRotation;
            targetPosition = IKTarget.localPosition;

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


        CheckAndAdjustIK();

        // Clamp X position
        targetPosition = new Vector3(0f, targetPosition.y, targetPosition.z);

        // Apply final movement with block conditions
        Vector3 finalPosition = IKTarget.localPosition;

        if (!blockYMovement)
            finalPosition.y = Mathf.MoveTowards(IKTarget.localPosition.y, targetPosition.y, moveSpeed * Time.deltaTime);

        if (!blockZMovement)
            finalPosition.z = Mathf.MoveTowards(IKTarget.localPosition.z, targetPosition.z, moveSpeed * Time.deltaTime);

        IKTarget.localPosition = finalPosition;
        IKTarget.localRotation = Quaternion.RotateTowards(IKTarget.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
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

        bool adjustmentsMade = false;

        // Reset block flags
        blockZMovement = false;
        blockYMovement = false;

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
            case 0: // +X — forward (actual X) — rotate away
                if (rotationCooldownTimer <= 0f)
                {
                    Vector3 euler = targetRotation.eulerAngles;
                    euler.x += rotationAdjustment;
                    euler.y = 0f;
                    targetRotation = Quaternion.Euler(euler);
                    rotationCooldownTimer = rotationCooldownDuration;
                }
                break;

            case 1: // +Y — block downward, move down
                blockYMovement = true;
                targetPosition.y -= moveAdjustment;
                break;

            case 2: // -Y — block upward, move up
                blockYMovement = true;
                targetPosition.y += moveAdjustment;
                break;

            case 3: // +Z — right side (actual side dodge)
                targetPosition.z -= moveAdjustment; // move left
                break;

            case 4: // -Z — left side (actual side dodge)
                targetPosition.z += moveAdjustment; // move right
                break;
        }
    }
}
