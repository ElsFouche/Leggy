using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawGrabChild : MonoBehaviour
{
    public bool isGrabbing { get; private set; } = false; // Flag to check if grabbing

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
            //Debug.Log("Claw stopped grabbing: " + collision.gameObject.name); // Debugging
        }
    }
}
