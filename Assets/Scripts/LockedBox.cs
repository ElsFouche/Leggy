using System.Threading;
using UnityEngine;

public class LockSystem : MonoBehaviour
{
    public GameObject lid; // Assign the lid in the Inspector
    public GameObject DummyLid;
    public string requiredKeyName = "KeyObject"; // The name of the key object
    public bool isUnlocked = false; // Public bool to track lock state
    public GameObject KeyTransformPosition;

    public bool keyInside = true;
    public GameObject internalKey;
    public GameObject internalKeyMoveTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == requiredKeyName) // Check if the correct key enters the trigger
        {
            Transform BasketParrentTemp = other.transform.parent.transform.parent;
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

            DummyLid.SetActive(false);

            Rigidbody lidRb = lid.GetComponent<Rigidbody>();
            if (lidRb != null)
            {
                lidRb.isKinematic = false; // Enable physics movement
                lidRb.useGravity = true; // Re-enable gravity
            }

            if (internalKey != null && keyInside)
            { 
                internalKey.transform.position = internalKeyMoveTo.transform.position;
                internalKey.tag = "Grabbable";
                internalKey.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (!keyInside)
            {
                TransitionManager transitionManager = new TransitionManager();
                transitionManager.TransitionToScene(0);
            }

            isUnlocked = true; // Update lock state
            Debug.Log("Lid Unlocked!");
        }
    }
}
