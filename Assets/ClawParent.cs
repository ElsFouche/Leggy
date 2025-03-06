using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class ClawParent : MonoBehaviour
{
    [SerializeField] ClawMovementKinematic Claw_L_CS;
    [SerializeField] ClawMovementKinematic Claw_R_CS;
    public List<GameObject> objectsInClaw;


    [Header("Boxcast Data")]
    public float castDistance = 1.0f;
    public float maxGrabRange = 1.0f;

    // private List<GameObject> objectsToGrab;
    private bool clawGrabbing = false;
    public GameObject grabbedObject = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        // Dynamically adjust trigger size based on claw positions
        //if (Claw_L_CS.playerMovement || Claw_R_CS.playerMovement)
        {
            Debug.Log("claws moving");
            // Adjust the box collider size of the trigger area (assuming box collider is on ClawParent)
            GetComponent<BoxCollider>().size = new Vector3(
                Mathf.Abs(Claw_L_CS.gameObject.transform.localPosition.x - Claw_R_CS.gameObject.transform.localPosition.x),
                GetComponent<BoxCollider>().size.y,
                GetComponent<BoxCollider>().size.z);
        }

        // Perform grab check
        if (!Claw_L_CS.canClose && !Claw_R_CS.canClose && Claw_L_CS.hitObject == Claw_R_CS.hitObject && objectsInClaw.Contains(Claw_L_CS.hitObject))
        {
            clawIsGrabbing(Claw_L_CS.hitObject);
            clawGrabbing = true;
        }

        if (Claw_L_CS.canClose || Claw_R_CS.canClose)
        {
            if (grabbedObject != null)
            {
                UnparentObject(grabbedObject);
            }
            else
            {
                CheckForFloater();
            }
        }

    }

    private void FixedUpdate()
    {
        CheckForFloater();
    }


    public void clawIsGrabbing(GameObject objectToGrab = null)
    {
        // If neither claw is closing or we're trying to grab an object that's already being grabbed
        if (clawGrabbing == false && objectToGrab == null && (Claw_L_CS.canClose || Claw_R_CS.canClose))
        {
            // If the claws are open and we don't have anything grabbed, stop grabbing
            UnparentObject(grabbedObject);
        }
        else if (clawGrabbing == false && objectToGrab != null)
        {
            // If we are not grabbing anything and the object exists, grab it
            clawGrabbing = true;
            Debug.Log("Grabbing: " + objectToGrab);
            ParentGrabbedObject(objectToGrab);
        }
        else if (clawGrabbing == true && objectToGrab == null && Claw_L_CS.canClose && Claw_R_CS.canClose)
        {
            // If both claws are closing and nothing is grabbed, stop grabbing
            clawGrabbing = false;
            UnparentObject(grabbedObject);
        }
        else
        {
            // If we are grabbing something and both claws are open, release it
            ParentGrabbedObject(objectToGrab);
        }
    }


    private void ParentGrabbedObject(GameObject newChild)
    {
        if (newChild != null)
        {
            newChild.transform.parent = gameObject.transform;
            newChild.GetComponent<Rigidbody>().useGravity = false;
            newChild.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject = newChild;
        }
    }

    private void UnparentObject(GameObject childToRemove)
    {
        if (childToRemove != null)
        {
            childToRemove.GetComponent<Rigidbody>().useGravity = true;
            childToRemove.GetComponent<Rigidbody>().isKinematic = false;
            childToRemove.transform.parent = null;
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Claw_L" && other.name != "Claw_R") 
        {
            objectsInClaw.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != "Claw_L" && other.name != "Claw_R")
        {
            objectsInClaw.Remove(other.gameObject);
        }
    }

    private void CheckForFloater()
    {
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.tag == "Grabbable" && !objectsInClaw.Contains(child.gameObject))
            {
                UnparentObject(child.gameObject);
            }
            else if (grabbedObject != null && grabbedObject != child.gameObject && child.gameObject.tag == "Grabbable")
            {
                UnparentObject(child.gameObject);
                grabbedObject = null;
            }
        }
    }
}