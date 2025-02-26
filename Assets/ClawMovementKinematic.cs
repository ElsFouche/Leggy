using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ClawMovementKinematic : MonoBehaviour
{
    public BoxCollider Wrist_Kinematic_Collider;
    public float moveSpeed = 1.0f;

    private bool canClose = true;
    private Vector3 position;
    private Collider clawCollider;
    private ClawParent clawParent;
    private float castDistance;
    private RaycastHit hitResult;
    private bool castHit = false;
    private float successfulGrabRange = 0.5f;

    private float distanceToCenter;
    private GameObject hitObject;

    public bool playerMovement = false;
    private bool negativeDirection = false;

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

        if (transform.localPosition.x < 0)
        {
            negativeDirection = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canClose && Input.GetKey(KeyCode.E))
        {
            position = transform.localPosition;
            position.x += (moveSpeed * Time.deltaTime) * transform.right.x;
            transform.localPosition = position;
            playerMovement = true;


        } else if (Input.GetKey(KeyCode.Q))
        {
            position = transform.localPosition;
            position.x -= (moveSpeed * Time.deltaTime) * transform.right.x;
            transform.localPosition = position;

            if

            playerMovement = true;
        }
        else
        {
            playerMovement = false;        
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
            Gizmos.DrawWireCube(transform.position + transform.right * hitResult.distance, transform.localScale);
        }
        else
        {
            Gizmos.DrawWireCube(transform.position + transform.right * castDistance, transform.localScale);
        }
    }
}