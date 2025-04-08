using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggyRaycast : MonoBehaviour
{
<<<<<<< Updated upstream
    public Transform IKTarget; // The IK target to move/rotate

    // Detection distances (trigger movement/rotation)
=======
    public Transform IKTarget;

>>>>>>> Stashed changes
    public float detectDistanceX = 1f;
    public float detectDistanceY = 1f;
    public float detectDistanceZ = 1f;

<<<<<<< Updated upstream
    // Casting distances (ray length, should be larger than detectDistance)
=======
>>>>>>> Stashed changes
    public float castDistanceX = 1.5f;
    public float castDistanceY = 1.5f;
    public float castDistanceZ = 1.5f;

<<<<<<< Updated upstream
    public float rotationAdjustment = 10f; // Rotation angle when too close on +X
    public float moveAdjustment = 0.1f; // Movement adjustment for Y/Z

    public float moveSpeed = 5f; // Speed for smooth movement
    public float rotationSpeed = 5f; // Speed for smooth rotation

    public LayerMask obstacleLayer; // Define which layers should be detected
=======
    public float rotationAdjustment = 10f;
    public float moveAdjustment = 0.1f;

    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    public LayerMask obstacleLayer;

    private float rotationCooldownTimer = 0f;
    public float rotationCooldownDuration = 0.25f;
    public float rotationBuffer = 0.05f;
>>>>>>> Stashed changes

    private Vector3 targetPosition;
    private Quaternion targetRotation;

<<<<<<< Updated upstream
    private Vector3 lastKnownPosition;  // Track the last known position
    private Quaternion lastKnownRotation; // Track the last known rotation
=======
    private Vector3 lastKnownPosition;
    private Quaternion lastKnownRotation;

    // Block movement flags
    private bool blockZMovement = false;
    private bool blockYMovement = false;
>>>>>>> Stashed changes

    void Start()
    {
        if (IKTarget != null)
        {
<<<<<<< Updated upstream
            targetRotation = IKTarget.rotation;
            targetPosition = IKTarget.position;
=======
            targetRotation = IKTarget.localRotation;
            targetPosition = IKTarget.localPosition;
>>>>>>> Stashed changes
            lastKnownPosition = targetPosition;
            lastKnownRotation = targetRotation;
        }
    }

    void Update()
    {
        if (IKTarget == null) return;

<<<<<<< Updated upstream
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
=======
        rotationCooldownTimer -= Time.deltaTime;

        lastKnownPosition = IKTarget.localPosition;
        lastKnownRotation = IKTarget.localRotation;

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
>>>>>>> Stashed changes
    }

    void CheckAndAdjustIK()
    {
        Vector3[] directions = {
<<<<<<< Updated upstream
            transform.right,  // +X (Rotate)
            transform.up,     // +Y (Move Z)
            -transform.up,    // -Y (Move Z)
            transform.forward, // +Z (Move Y)
            -transform.forward // -Z (Move Y)
=======
            transform.right,     // +X
            transform.up,        // +Y
            -transform.up,       // -Y
            transform.forward,   // +Z
            -transform.forward   // -Z
>>>>>>> Stashed changes
        };

        float[] detectDistances = { detectDistanceX, detectDistanceY, detectDistanceY, detectDistanceZ, detectDistanceZ };
        float[] castDistances = { castDistanceX, castDistanceY, castDistanceY, castDistanceZ, castDistanceZ };

<<<<<<< Updated upstream
        bool adjustmentsMade = false;  // Flag to track if any adjustments were made
=======
        bool adjustmentsMade = false;

        // Reset block flags
        blockZMovement = false;
        blockYMovement = false;
>>>>>>> Stashed changes

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 rayStart = transform.position;
            Vector3 rayDirection = directions[i];
            float rayDetectDistance = detectDistances[i];
            float rayCastDistance = castDistances[i];

<<<<<<< Updated upstream
            RaycastHit hit;
            Debug.DrawRay(rayStart, rayDirection * rayCastDistance, Color.yellow);

            if (Physics.Raycast(rayStart, rayDirection, out hit, rayCastDistance, obstacleLayer))
            {
                float hitDistance = hit.distance;

                if (hitDistance <= rayDetectDistance)
                {
                    Debug.DrawRay(rayStart, rayDirection * hitDistance, Color.red);
                    Debug.Log($"[LeggyRaycast] HIT: {hit.collider.gameObject.name} at {hitDistance}m in {rayDirection}");
=======
            if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, rayCastDistance, obstacleLayer))
            {
                float hitDistance = hit.distance;

                if (hitDistance <= rayDetectDistance - rotationBuffer)
                {
                    Debug.DrawRay(rayStart, rayDirection * hitDistance, Color.red);
>>>>>>> Stashed changes
                    AdjustIKTarget(rayDirection, i);
                    adjustmentsMade = true;
                }
                else
                {
                    Debug.DrawRay(rayStart, rayDirection * hitDistance, Color.blue);
                }
            }
        }

<<<<<<< Updated upstream
        // Only update the target position/rotation if any adjustments were made
        if (!adjustmentsMade)
        {
            targetPosition = lastKnownPosition;  // Keep the target position from the last frame
            targetRotation = lastKnownRotation;  // Keep the target rotation from the last frame
=======
        if (!adjustmentsMade)
        {
            targetPosition = lastKnownPosition;
            targetRotation = lastKnownRotation;
>>>>>>> Stashed changes
        }
    }

    void AdjustIKTarget(Vector3 direction, int directionIndex)
    {
        if (IKTarget == null) return;

        switch (directionIndex)
        {
<<<<<<< Updated upstream
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
=======
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

>>>>>>> Stashed changes
}
