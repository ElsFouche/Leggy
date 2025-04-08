using System.Security.Cryptography;
using System.Transactions;
using UnityEngine;

public class LockSystem : MonoBehaviour
{
    public GameObject lid; // Assign the lid in the Inspector
    public GameObject dummyLid;
    public string requiredKeyName = "KeyObject"; // The name of the key object
    public bool isUnlocked = false; // Public bool to track lock state
    public GameObject KeyTransformPosition;
    public GameObject KeyFloatPoint;

    public bool isInternalObject = true;
    public GameObject InternalGameobject;

    public bool gameEndGoal = false;



    public TransitionManager transitionManager;

    public GameObject gameover;


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
            lid.gameObject.SetActive(true);
            dummyLid.SetActive(false);
            lid.tag = "Grabbable";

            Rigidbody lidRb = lid.GetComponent<Rigidbody>();
            if (lidRb != null)
            {
                lidRb.isKinematic = false; // Enable physics movement
                lidRb.useGravity = true; // Re-enable gravity
            }


            isUnlocked = true; // Update lock state\
            if (isInternalObject)
            {
                InternalGameobject.GetComponent<Rigidbody>().isKinematic = true;
                InternalGameobject.transform.position = KeyFloatPoint.transform.position;
            }
            Debug.Log("Lid Unlocked!");


            if (gameEndGoal)
            {
                CompleteGoal();
            }
        }
    }

    private void CompleteGoal()
    {
        Debug.Log("Goal Completed!");
        transitionManager.PressToStart();

        // Add any completion logic here (e.g., UI update, level progression, etc.)
        gameover.SetActive(true);
    }
}
