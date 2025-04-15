using System.Security.Cryptography;
using UnityEngine;

public class LockSystem : MonoBehaviour
{
    public GameObject lid; // Assign the lid in the Inspector
    public string requiredKeyName = "KeyObject"; // The name of the key object
    public bool isUnlocked = false; // Public bool to track lock state
    public GameObject KeyTransformPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == requiredKeyName) // Check if the correct key enters the trigger
        {
            Transform BasketParrentTemp = other.transform.parent;
            BasketParrentTemp.transform.parent = null;
            BasketParrentTemp.tag = "Untagged";
            if (KeyTransformPosition != null)
            {
                BasketParrentTemp.gameObject.transform.position = KeyTransformPosition.transform.position;
            }
            else
            {

                BasketParrentTemp.gameObject.transform.position = transform.position;
            }

            BasketParrentTemp.rotation = transform.rotation;
            Rigidbody KeyRigid = BasketParrentTemp.GetComponent<Rigidbody>();
            KeyRigid.isKinematic = true;
            KeyRigid.useGravity = false;

            UnlockLid();
        }
    }

    private void UnlockLid()
    {
        if (lid != null)
        {
            lid.tag = "Grabbable";

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
