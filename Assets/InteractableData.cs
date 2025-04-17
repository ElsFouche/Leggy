using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractableData : MonoBehaviour
{
    //[HideInInspector]

    public Vector3 originalScale;
    public Quaternion originalRotation;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }
}
