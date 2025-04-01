using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawGrabManager : MonoBehaviour
{
    [SerializeField] private DummyMovement dummyMovement;
    [SerializeField] private ClawGrabChild LeggyLeftClaw;
    [SerializeField] private ClawGrabChild LeggyRightClaw;

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float moveSpeed = 1f;

    private GameObject heldObject = null;
    private Rigidbody heldObjectRb = null;

    public bool grabParrent = false;

    private Rigidbody selfRB;
    private void Start()
    {
        selfRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGrabbing();
        CheckIfBothClawsAreTouching();
        CheckObjectSlipping();

        if (heldObject == null && dummyMovement.closing)  // Checks for no held obj
        {
            GameObject closestObject = GetClosestObjectInBoxCollider();
            if (closestObject != null)
            {
                // Moves obj to center of claw mouth when closing
                if (IsClawsClosing())
                {
                    MoveObjectToCenter(closestObject);
                }
            }
        }
    }

    private void MoveObjectToCenter(GameObject obj)
    {
        // Move the object towards the center of the BoxCollider
        Vector3 center = boxCollider.transform.position;
        obj.transform.position = Vector3.MoveTowards(obj.transform.position, center, moveSpeed * Time.fixedDeltaTime);
    }

    private GameObject GetClosestObjectInBoxCollider()
    {
        Collider[] colliders = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size / 2, Quaternion.identity); // Get all colliders in the BoxCollider
        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Grabbable"))  // Only consider grabbable objects
            {
                float distance = Vector3.Distance(col.transform.position, boxCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestObject = col.gameObject;
                    closestDistance = distance;
                }
            }
        }
        return closestObject;
    }

    private void CheckGrabbing()
    {
        // Check if either both claws are grabbing the object or TriggerParrent is true
        if ((LeggyLeftClaw.isGrabbing && LeggyRightClaw.isGrabbing) || (LeggyLeftClaw.clawTriggerContact && LeggyRightClaw.clawTriggerContact))
        {
            GameObject obj1 = GetGrabbableInClaw(LeggyLeftClaw);
            GameObject obj2 = GetGrabbableInClaw(LeggyRightClaw);

            // If both claws grab the same object and it's not already held
            //if (((obj1 != null && obj1 == obj2) || (LeggyLeftClaw.clawTriggerContact && LeggyRightClaw.clawTriggerContact && heldObject)) && heldObject == null)
            if ((obj1 != null && obj1 == obj2) && heldObject == null)
            {
                ParentObject(obj1);
            }
        }
        else if (heldObject != null)
        {
            //ReleaseObject();
        }
    }

    private void CheckIfBothClawsAreTouching()
    {
        // If the object is held and either claw stops touching the object, release it
        if (heldObject != null)
        {
            bool LeggyLeftClawTouching = IsClawTouchingObject(LeggyLeftClaw);
            bool LeggyRightClawTouching = IsClawTouchingObject(LeggyRightClaw);

            if (!LeggyLeftClawTouching || !LeggyRightClawTouching && !dummyMovement.TriggerParrent)
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

    private bool IsClawsClosing()
    {
        // Check if either claw is closing (i.e., moving towards the object)
        return LeggyLeftClaw.isGrabbing || LeggyRightClaw.isGrabbing;
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

        GameObject closestObject = GetClosestObjectInBoxCollider();
        MoveObjectToCenter(closestObject);

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
            // Unparrents and enables gravity again
            heldObject.transform.SetParent(null);
            if (heldObjectRb != null)
            {
                heldObjectRb.useGravity = true;
                heldObjectRb = null;
            }
            
            Debug.Log("Object released: " + heldObject.name);
            heldObject = null;
            grabParrent = false;
        }
    }
}
