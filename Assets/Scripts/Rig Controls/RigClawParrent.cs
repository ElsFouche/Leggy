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
    private float overpressureMultiplier = 0.2f;

    private void Update()
    {
        // Only detect objects when grip pressure is NOT too high (claw isn't wide open)
        if (WristMouth.ObjectDetected != null && clawController.gripPreassure <= 0.9f)
        {
            detectedBasket = WristMouth.ObjectDetected.GetComponent<BasketData>();
        }
        else
        {
            detectedBasket = null; 
        }

        if (detectedBasket != null)
        {
            float overpressureThreshold = detectedBasket.requiredGripPressure * (1 + overpressureMultiplier);

            if (grabbedObject == null && clawController.gripPreassure <= detectedBasket.requiredGripPressure)
            {
                Debug.Log("Grabbing object: " + detectedBasket.name);
                GrabObject(detectedBasket);
            }
            else if (grabbedObject != null && clawController.gripPreassure > detectedBasket.requiredGripPressure) 
            {
                Debug.Log("Releasing object: " + grabbedObject.name);
                ReleaseObject();
            }
            else if (grabbedObject != null && clawController.gripPreassure < detectedBasket.requiredGripPressure - overpressureThreshold) // Adjusted ejection threshold
            {
                Debug.Log("Ejecting object: " + grabbedObject.name);
                EjectObject();
            }
        }
    }

    private void GrabObject(BasketData basket)
    {
        if (grabbedObject == null && WristMouth.ObjectInClawMouth && clawController.gripPreassure <= basket.requiredGripPressure)
        {
            grabbedObject = basket.transform;

            // Store world position/rotation before parenting
            Vector3 worldPosition = grabbedObject.position;
            Quaternion worldRotation = grabbedObject.rotation;
            Vector3 originalScale = grabbedObject.localScale; 

            grabbedObject.SetParent(heldItemParent);

            // Restore world position/rotation and maintain original scale
            grabbedObject.position = worldPosition;
            grabbedObject.rotation = worldRotation;
            grabbedObject.localScale = originalScale; 

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

            if (grabbedObject.TryGetComponent(out BasketData basket))
            {
                if (basket.objectRigidbody != null)
                {
                    basket.objectRigidbody.isKinematic = false;
                }
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

            if (grabbedObject.TryGetComponent(out BasketData basket))
            {
                if (basket.objectRigidbody != null)
                {
                    basket.objectRigidbody.isKinematic = false;
                    basket.objectRigidbody.AddForce(transform.forward * 10f, ForceMode.Impulse);
                }
            }

            grabbedObject = null;
            detectedBasket = null;
        }
    }
}
