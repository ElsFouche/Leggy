using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawGrabManager : MonoBehaviour
{
    [SerializeField] private DummyMovement dummyMovement;

    public ClawGrabChild claw1; // Reference to first claw
    public ClawGrabChild claw2; // Reference to second claw
    private GameObject heldObject = null; // Object currently held
    private Rigidbody heldObjectRb = null; // Rigidbody of the held object

    public bool grabParrent = false;

    private void Update()
    {
        CheckGrabbing();
        CheckIfBothClawsAreTouching();
        CheckObjectSlipping();
    }

    private void CheckGrabbing()
    {
        // Check if either both claws are grabbing the object or TriggerParrent is true
        if ((claw1.isGrabbing && claw2.isGrabbing) || dummyMovement.TriggerParrent)
        {
            GameObject obj1 = GetGrabbableInClaw(claw1);
            GameObject obj2 = GetGrabbableInClaw(claw2);

            // If both claws grab the same object and it's not already held
            if (obj1 != null && obj1 == obj2 && heldObject == null)
            {
                ParentObject(obj1);
            }
            // Or if TriggerParrent is true and the object is not already held
            else if (dummyMovement.TriggerParrent && heldObject == null)
            {
                // Parent the object when TriggerParrent is true (checking claw1 for example)
                GameObject obj = obj1 != null ? obj1 : obj2;
                if (obj != null)
                {
                    ParentObject(obj);
                }
            }
        }
        else if (heldObject != null)
        {
            ReleaseObject();
        }
    }

    private void CheckIfBothClawsAreTouching()
    {
        // If the object is held and either claw stops touching the object, release it
        if (heldObject != null)
        {
            bool claw1Touching = IsClawTouchingObject(claw1);
            bool claw2Touching = IsClawTouchingObject(claw2);

            if (!claw1Touching || !claw2Touching && !dummyMovement.TriggerParrent)
            {
                ReleaseObject();
            }
        }
    }

    private bool IsClawTouchingObject(ClawGrabChild claw)
    {
        Collider[] colliders = Physics.OverlapSphere(claw.transform.position, 0.1f); // Adjust radius if needed
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Grabbable") && col.gameObject == heldObject)
            {
                return true; // Claw is still holding the object
            }
        }
        return false; // Claw is no longer touching the object
    }

    private void CheckObjectSlipping()
    {
        if (heldObject != null && heldObjectRb != null && heldObjectRb.velocity.magnitude > 0.1f)
        {
            // If the object is moving and velocity exceeds a threshold (indicating it might slip out)
            ReleaseObject();
        }
    }

    private void ParentObject(GameObject obj)
    {
        heldObject = obj;
        heldObjectRb = heldObject.GetComponent<Rigidbody>(); // Get Rigidbody
        heldObject.transform.SetParent(transform); // Parent to the arm or main object
        heldObjectRb.useGravity = false; // Disable gravity while holding
        Debug.Log("Object grasped: " + heldObject.name);
        grabParrent = true;
    }

    private GameObject GetGrabbableInClaw(ClawGrabChild claw)
    {
        Collider[] colliders = Physics.OverlapSphere(claw.transform.position, 0.1f); // Adjust radius if needed
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Grabbable"))
            {
                return col.gameObject;
            }
        }
        return null;
    }

    private void ReleaseObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null); // Unparent the object
            if (heldObjectRb != null)
            {
                heldObjectRb.useGravity = true; // Re-enable gravity
                heldObjectRb = null;
            }
            Debug.Log("Object released: " + heldObject.name);
            heldObject = null;
            grabParrent = false;
        }
    }
}
