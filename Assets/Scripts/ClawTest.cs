using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ClawTest : MonoBehaviour
{
    public Rigidbody ClawLeftPivotRigid;
    public Rigidbody ClawRightPivotRigid;

    public float gripForce = 10f;
    public float openTorque = 5f;
    public float closeTorque = 15f;
    public bool isGripping = false;

    private Vector3 initialLeftLocalPos;
    private Vector3 initialRightLocalPos;

    void Start()
    {
        // Store initial positions
        initialLeftLocalPos = ClawLeftPivotRigid.transform.localPosition;
        initialRightLocalPos = ClawRightPivotRigid.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (isGripping)
        {
            // Apply continuous torque to keep claw closed
            ClawLeftPivotRigid.AddForce(Vector3.left * closeTorque, ForceMode.Force);
            ClawRightPivotRigid.AddForce(Vector3.right * closeTorque, ForceMode.Force);
            Debug.Log("Claw Left Velocity: " + ClawLeftPivotRigid.velocity);
        }
        else
        {
            // Apply torque to open claw
            Debug.Log("1");
            ClawLeftPivotRigid.AddForce(Vector3.right * openTorque, ForceMode.Force);
            ClawRightPivotRigid.AddForce(Vector3.left * openTorque, ForceMode.Force);

            if (ClawLeftPivotRigid.transform.localPosition.x > initialLeftLocalPos.x)
            {
                ClawLeftPivotRigid.transform.localPosition = new Vector3( initialLeftLocalPos.x, ClawLeftPivotRigid.transform.localPosition.y, ClawLeftPivotRigid.transform.localPosition.z);
                ClawLeftPivotRigid.velocity = Vector3.zero; // Stop excess movement
            }

            if (ClawRightPivotRigid.transform.localPosition.x < initialRightLocalPos.x)
            {
                ClawRightPivotRigid.transform.localPosition = new Vector3(initialRightLocalPos.x, ClawRightPivotRigid.transform.localPosition.y, ClawRightPivotRigid.transform.localPosition.z);
                ClawRightPivotRigid.velocity = Vector3.zero; // Stop excess movement
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Close claw
        {
            isGripping = true;
        }
        if (Input.GetMouseButtonDown(1)) // Open claw
        {
            isGripping = false;
        }
    }
}
