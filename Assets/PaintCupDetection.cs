using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCupDetection : MonoBehaviour
{
    public bool brushInCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PaintBrush>() != null) { }
    }
}
