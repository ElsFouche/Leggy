using UnityEngine;

public class LockSystem : MonoBehaviour
{
    public GameObject lid; // Assign the lid in the Inspector
    public string requiredKeyName = "KeyObject"; // The name of the key object
    public bool isUnlocked = false; // Public bool to track lock state

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == requiredKeyName) // Check if the correct key enters the trigger
        {
            UnlockLid();
        }
    }

    private void UnlockLid()
    {
        if (lid != null)
        {
            Rigidbody lidRb = lid.GetComponent<Rigidbody>();
            if (lidRb != null)
            {
                lidRb.isKinematic = false; // Enable physics movement
                lidRb.useGravity = true; // Re-enable gravity
            }

            isUnlocked = true; // Update lock state
            Debug.Log("Lid Unlocked!");
        }
    }
}
