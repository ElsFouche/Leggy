using UnityEngine;

public class BasketData : MonoBehaviour
{
    public float requiredGripPressure = 0.5f; // Adjust per object
    public GameObject RootRigidBodyGameObject;
    public Rigidbody objectRigidbody; // Reference to the object's Rigidbody

    private void Awake()
    {
        RootRigidBodyGameObject = transform.root.gameObject;

        if (objectRigidbody == null)
            objectRigidbody = this.transform.root.GetComponentInChildren<Rigidbody>();
    }
}