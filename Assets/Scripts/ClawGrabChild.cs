using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClawGrabChild : MonoBehaviour
{
    public bool isGrabbing { get; private set; } = false; // Flag to check if grabbing
    public bool clawTriggerContact = false;

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

    private void OnTriggerStay(Collider other)
    {

        Debug.Log("contact: " + other.gameObject);
        if (other.CompareTag("Grabbable"))
        {
            clawTriggerContact = true;
            Debug.Log("entered claw contact of: " + this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            clawTriggerContact = false;
            Debug.Log("exited claw contact of: " + this);
        }
    }
}
