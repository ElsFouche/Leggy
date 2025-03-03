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
    private GameObject grabbedObject = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        //dynamic sizing of the trigger for things in claw
        if (Claw_L_CS.playerMovement || Claw_R_CS.playerMovement)
        {
            GetComponent<BoxCollider>().size = new Vector3 (
                MathF.Abs(Claw_L_CS.gameObject.transform.localPosition.x * 2) - Claw_L_CS.transform.localScale.x, 
                Claw_L_CS.gameObject.transform.localScale.y,
                Claw_L_CS.gameObject.transform.localScale.z);
        }

        if (!Claw_L_CS.canClose && !Claw_R_CS.canClose)
        {
            if (Claw_L_CS.hitObject == Claw_R_CS.hitObject && objectsInClaw.Contains(Claw_L_CS.hitObject))
            {
                clawIsGrabbing(Claw_L_CS.hitObject);
            }
        }

        if (Claw_L_CS.canClose && Claw_R_CS.canClose && objectsInClaw.Contains(grabbedObject))
        {
            UnparentObject(grabbedObject);
        }
    }

    public void clawIsGrabbing(GameObject objectToGrab = null)
    {
        if ((clawGrabbing == false && objectToGrab == null || objectsInClaw.Contains(objectToGrab)) && Claw_L_CS.canClose && Claw_R_CS.canClose) 
        {
            UnparentObject(grabbedObject);
            // Debug.Log("clawGrabbing = false & objectToGrab = null.");
        }
        else if (clawGrabbing == false && objectToGrab != null)
        {
            clawGrabbing = true;
            // Debug.Log("clawGrabbing = false & objectToGrab exists.");
        } else if (clawGrabbing == true && objectToGrab == null || Claw_L_CS.canClose && Claw_R_CS.canClose)
        {
            clawGrabbing = false; 
            // Debug.Log("clawGrabbing = true & objectToGrab = null.");
        } else 
        { 
            ParentGrabbedObject(objectToGrab);
            // Debug.Log("clawGrabbing = true & objectToGrab exists."); 
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
}