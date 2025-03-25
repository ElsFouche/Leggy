using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClawMovement : MonoBehaviour
{
    public Vector3 clawForce = new Vector3(0,0,0);
    private Rigidbody claw_rb;
    private bool canOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        claw_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            claw_rb.AddRelativeForce(clawForce, ForceMode.Force);
            // Debug.Log("Adding Force to " + this.gameObject.name + "\n Force: " + claw_rb.GetAccumulatedForce());
        }else if (Input.GetKey(KeyCode.Q) && canOpen)
        {
            claw_rb.AddRelativeForce(-clawForce, ForceMode.Force);
        } else { StopVelocity(); }
        
        /*if (Input.GetKeyUp(KeyCode.E))
        {
            StopVelocity();
            // Debug.Log("Setting velocity for " + this.gameObject.name + ". \n Velocity: " + claw_rb.velocity);
            //claw_rb.AddForce(-claw_rb.GetAccumulatedForce(), ForceMode.Force);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            StopVelocity();
        }*/

        if (Mathf.Abs(transform.localPosition.x) > 1.5f)
        {
            StopVelocity();
            canOpen = false;
            Debug.Log("Can open? : " + canOpen);
        } else { canOpen = true; }
    }

    private void StopVelocity()
    {
        claw_rb.velocity = Vector3.zero;
        claw_rb.angularVelocity = Vector3.zero;
    }
}
