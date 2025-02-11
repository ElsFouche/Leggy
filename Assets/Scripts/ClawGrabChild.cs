using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawGrabChild : MonoBehaviour
{
    public bool isGrabbing { get; private set; } = false; // Flag to check if grabbing
    public bool clawTriggerContact = false;

    public float raycastDistance = 0.5f;
    public Vector3 boxSize = new Vector3(0.1f, 0.1f, 0.1f); // Define a small box for detection
    public LayerMask grabbableLayer;
    public bool rightClaw;

    private void Update()
    {
        //Raycasts for the raycast version of grab
        Vector3 rayDirection = rightClaw ? -transform.right : transform.right;
        Debug.DrawRay(transform.position, rayDirection * raycastDistance, Color.green);

        // Perform a BoxCast to detect objects within the claw range
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, boxSize / 2, rayDirection, out hit, transform.rotation, raycastDistance, grabbableLayer))
        {
            if (hit.collider.CompareTag("Grabbable"))
            {
                clawTriggerContact = true;
                Debug.Log("Object in center of claw: " + hit.collider.gameObject.name);
            }
        }
        else
        {
            clawTriggerContact = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Grabbable") && collision.gameObject != gameObject)
        {
            isGrabbing = true;
            Debug.Log(gameObject.name + " is now grabbing: " + collision.gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Grabbable"))
        {
            isGrabbing = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            clawTriggerContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            clawTriggerContact = false;
        }
    }
}
