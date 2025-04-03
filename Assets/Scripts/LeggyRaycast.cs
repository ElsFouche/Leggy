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
    public LayerMask obstacleLayer; // Define which layers should be detected

    void Update()
    {
        CheckAndAdjustIK();
    }

    void CheckAndAdjustIK()
    {
        // Define ray directions (+X, +Y, -Y, +Z, -Z)
        Vector3[] directions = {
            transform.right,  // +X
            transform.up,     // +Y
            -transform.up,    // -Y
            transform.forward, // +Z
            -transform.forward // -Z
        };

        // Corresponding detection & casting distances
        float[] detectDistances = { detectDistanceX, detectDistanceY, detectDistanceY, detectDistanceZ, detectDistanceZ };
        float[] castDistances = { castDistanceX, castDistanceY, castDistanceY, castDistanceZ, castDistanceZ };

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 rayStart = transform.position;
            Vector3 rayDirection = directions[i];
            float rayDetectDistance = detectDistances[i];
            float rayCastDistance = castDistances[i];

            RaycastHit hit;

            // Draw full ray for debugging
            Debug.DrawRay(rayStart, rayDirection * rayCastDistance, Color.yellow);

            // Check for collisions within cast distance
            if (Physics.Raycast(rayStart, rayDirection, out hit, rayCastDistance, obstacleLayer))
            {
                float hitDistance = hit.distance;

                // If within detection range, adjust the IK target
                if (hitDistance <= rayDetectDistance)
                {
                    Debug.DrawRay(rayStart, rayDirection * hitDistance, Color.red); // Collision within detect distance
                    Debug.Log($"[LeggyRaycast] HIT: {hit.collider.gameObject.name} at {hitDistance}m in {rayDirection}");
                    AdjustIKTarget(rayDirection, hitDistance, i == 0); // i == 0 means it's +X
                }
                else
                {
                    Debug.DrawRay(rayStart, rayDirection * hitDistance, Color.blue); // Hit, but outside detection range
                }
            }
        }
    }

    void AdjustIKTarget(Vector3 direction, float hitDistance, bool isXAxis)
    {
        if (IKTarget == null) return;

        if (isXAxis) // If obstacle is too close on +X, rotate instead of moving
        {
            IKTarget.Rotate(Vector3.up * rotationAdjustment);
            Debug.Log("[LeggyRaycast] Rotating IK Target to avoid +X obstacle.");
        }
        else // Move away on Y/Z
        {
            IKTarget.position += -direction * moveAdjustment;
            Debug.Log("[LeggyRaycast] Moving IK Target away from obstacle.");
        }
    }
}
