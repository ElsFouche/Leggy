using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdhouse : MonoBehaviour
{
    public GameObject edge1;
    public GameObject edge2;
    public GameObject edge3;
    public GameObject edge4;


    public Transform snapTarget; // The target position where this piece should fit
    public float snapDistance = 1f; // Distance to snap to target position
    public float snapSpeed = 5f; // Speed at which the piece moves towards the target
    private bool isSnapped = false; // If the piece is snapped into place

    private Rigidbody rb;

    public bool IsSnapped { get { return isSnapped; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Only move if it's not snapped yet
        if (isSnapped)
            return;  // No movement if already snapped

        // Check distance to snap target
        float distanceToTarget = Vector3.Distance(transform.position, snapTarget.position);

        if (distanceToTarget < snapDistance)
        {
            // Move the piece towards the snap target
            transform.position = Vector3.Lerp(transform.position, snapTarget.position, Time.deltaTime * snapSpeed);

            // If close enough, lock the piece in place
            if (distanceToTarget < 0.2f)
            {
                isSnapped = true;
                transform.position = snapTarget.position; // Lock position
                transform.rotation = snapTarget.rotation; // Lock rotation if needed
            }
        }
    }

    public void OnGrabbed()
    {
        // Disable physics while moving with the claw
        if (rb != null)
        {
            rb.isKinematic = true;
            isSnapped = false; // Allow the piece to be moved when grabbed
        }
    }

    public void OnReleased()
    {
        // Re-enable physics after being released
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
