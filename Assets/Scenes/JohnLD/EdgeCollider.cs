using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCollider : MonoBehaviour
{
    public bool triggered;
    public string targetObject;

    private void OnTriggerEnter(Collider other)
    {
        if(targetObject != null)
        {
            if(other.gameObject.name == targetObject)
            {
                triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetObject != null)
        {
            if (other.gameObject.name == targetObject)
            {
                triggered = false;
            }
        }
    }
}
