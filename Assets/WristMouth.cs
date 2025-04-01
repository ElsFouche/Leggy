using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristMouth : MonoBehaviour
{
    public bool ObjectInClawMouth = false;
    public GameObject ObjectDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            ObjectInClawMouth = true;
            ObjectDetected = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            ObjectInClawMouth = true;
            ObjectDetected = other.gameObject;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            ObjectInClawMouth = false;
            ObjectDetected = null;
        }
    }
}
