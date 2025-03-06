using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParentOnCollision : MonoBehaviour
{
    public string childTag = "Grabbable"; // Set this to the tag of the objects you want to parent

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(childTag))
        {
            collision.transform.parent = transform;
        }
    }
}
