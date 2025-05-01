using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCupDetection : MonoBehaviour
{
    public bool brushInCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BasketData>().RootRigidBodyGameObject.GetComponent<PaintBrush>() != null) { brushInCollider = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BasketData>().RootRigidBodyGameObject.GetComponent<PaintBrush>() != null) { brushInCollider = false  ; }
    }
}
