using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigClawParrent : MonoBehaviour
{
    [SerializeField] WristMouth WristMouth;

    public Collider wristCollider; // Collider that detects objects
    public RigClawController clawController; // Reference to claw controller for grip pressure

    public Transform heldItemParent; // Empty GameObject to hold grabbed items

    private BasketData detectedBasket;
    private Transform grabbedObject;

    private void Update()
    {
        if (detectedBasket != null)
        {
            if (clawController.gripPreassure >= detectedBasket.requiredGripPressure)
            {
                GrabObject(detectedBasket);
            }
            else if (grabbedObject != null && clawController.gripPreassure < detectedBasket.requiredGripPressure)
            {
                ReleaseObject();
            }
            else if (clawController.gripPreassure > detectedBasket.requiredGripPressure + 0.2f)
            {
                EjectObject();
            }
        }
    }

    private void GrabObject(BasketData basket)
    {
        if (grabbedObject == null && WristMouth.ObjectInClawMouth)
        {
            grabbedObject = basket.transform;
            grabbedObject.SetParent(heldItemParent); // Parent to the empty GameObject
            grabbedObject.localPosition = Vector3.zero;
            grabbedObject.localRotation = Quaternion.identity;

            if (basket.objectRigidbody != null)
            {
                basket.objectRigidbody.isKinematic = true;
            }
        }
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.SetParent(null);

            if (detectedBasket.objectRigidbody != null)
            {
                detectedBasket.objectRigidbody.isKinematic = false;
            }

            grabbedObject = null;
            detectedBasket = null;
        }
    }

    private void EjectObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.SetParent(null);

            if (detectedBasket.objectRigidbody != null)
            {
                detectedBasket.objectRigidbody.isKinematic = false;
                detectedBasket.objectRigidbody.AddForce(transform.forward * 10f, ForceMode.Impulse);
            }

            grabbedObject = null;
            detectedBasket = null;
        }
    }
}
