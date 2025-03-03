using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class ClawMovementKinematic : MonoBehaviour
{
    public GameObject hitobjectFlag;

    public BoxCollider Wrist_Kinematic_Collider;
    public float moveSpeed = 1.0f;
    private float stopDistance = 0.5f;

    public bool canClose = true;
    private Vector3 position;
    private Collider clawCollider;
    private ClawParent clawParent;
    private float castDistance;
    private RaycastHit hitResult;
    private bool castHit = false;
    private float successfulGrabRange = 0.5f;

    private float distanceToCenter;
    public GameObject hitObject;

    public bool playerMovement = false;
    private bool negativeDirection = false;

    private GameObject clawL;
    private GameObject clawR;
    private float clawWidth;

    // Start is called before the first frame update
    void Start()
    {
        clawParent = transform.parent.GetComponent<ClawParent>();
        clawCollider = transform.GetComponent<Collider>();
        position = transform.localPosition;
        castDistance = clawParent.castDistance;
        successfulGrabRange = clawParent.maxGrabRange;
        GameObject WristKinematic = GameObject.Find("Wrist_Kinematic");
        Wrist_Kinematic_Collider = WristKinematic.GetComponent<BoxCollider>();

        clawL = GameObject.Find("Claw_L");
        clawR = GameObject.Find("Claw_R");

        clawWidth = clawL.transform.localScale.x;

        if (transform.localPosition.x < 0)
        {
            negativeDirection = true;
            stopDistance =  gameObject.transform.localScale.x / 2f;
        }
        else
        {
            stopDistance = -gameObject.transform.localScale.x / 2f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.E))
        {
            position = transform.localPosition; position.x = Mathf.Clamp(position.x + (moveSpeed * Time.deltaTime), -stopDistance, 2);
            transform.localPosition = position;
            playerMovement = true;
        }
        else if (canClose && Input.GetKey(KeyCode.Q))
        {
            position = transform.localPosition;position.x = Mathf.Clamp(position.x - (moveSpeed * Time.deltaTime), -stopDistance, 2);
            transform.localPosition = position;
            playerMovement = true;
        }
        else
        { 
            playerMovement = false;        
        }

        
        if (Mathf.Abs(clawL.transform.localPosition.x) != Mathf.Abs(clawR.transform.localPosition.x))
        {
            clawL.transform.localPosition = new Vector3(-clawR.transform.localPosition.x, 0, 0);
        }

        // Begin box cast
        castHit = Physics.BoxCast(clawCollider.bounds.center,
                                  transform.localScale * 0.5f,
                                  transform.right,
                                  out hitResult,
                                  transform.localRotation,
                                  castDistance);
        

        if (castHit && hitResult.transform.CompareTag("Grabbable"))
        {
            hitObject = hitResult.transform.gameObject;
            distanceToCenter = Vector3.Distance(hitObject.GetComponent<Rigidbody>().centerOfMass, clawCollider.bounds.center);
            

            // Prevent intersections
            if (!gameObject.CompareTag(hitObject.tag))
            {
                canClose = false;
            }

            // Do this if the box cast is not hitting a claw
            if (!(hitObject.CompareTag("ClawL") || hitObject.CompareTag("ClawR")))
            {
                hitobjectFlag.transform.position = hitObject.transform.position;
                
                if (Input.GetKey(KeyCode.E))
                {
                    if (distanceToCenter < successfulGrabRange)
                    {
                        clawParent.clawIsGrabbing(hitObject.gameObject);
                        // Debug.Log("Notify ClawParent that claw is touching " + hitObject.name);
                    }
                   
                }
            }

            // Object has slid too far away and is dropped
            if (distanceToCenter > (successfulGrabRange))
            {
                Debug.Log("Too far from target.");
                clawParent.clawIsGrabbing();
            }
        } else { canClose = true; hitObject = null; } 

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (castHit && hitResult.transform != null && !(gameObject.CompareTag(hitResult.transform.gameObject.tag)))
        {
            //Gizmos.matrix = Matrix4x4.TRS(new Vector3(0,0,0), transform.rotation, transform.localScale);
            Gizmos.DrawWireCube(transform.position + transform.right * hitResult.distance, transform.localScale);
        }
        else
        {
            //Gizmos.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), transform.rotation, transform.localScale);
            Gizmos.DrawWireCube(transform.position + transform.right * castDistance, transform.localScale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Grabbable")
        {
            canClose = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canClose = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Grabbable"))
        {
            canClose = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Grabbable"))
        {
            canClose = true;
        }
    }

}