using UnityEngine;

public class BasketData : MonoBehaviour
{
    public float requiredGripPressure = 0.5f; // Adjust per object
    public Rigidbody objectRigidbody; // Reference to the object's Rigidbody

    private void Awake()
    {
        if (objectRigidbody == null)
            objectRigidbody = GetComponentInChildren<Rigidbody>();
    }
}