using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detected: " + other);

    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Lingering: " + other);
        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Object Left: " + other);
        
    }
}

