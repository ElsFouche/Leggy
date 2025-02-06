using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{

    public float speed = 2f;
    private bool opened = true;
    private bool closed = false;
    private bool closing = false;
    private bool opening = false;

    public GameObject ClawLeft;
    private Vector3 ClawLeftOrigin;
    private Rigidbody ClawLeftRB;

    public GameObject ClawRight;
    private Vector3 ClawRightOrigin;
    private Rigidbody ClawRightRB;

    public ClawGrabChild LeftClaw;
    public ClawGrabChild RightClaw;
    public GameObject clawEmptyParent; 

    public bool TriggerParrent = false;

    public float slipSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        ClawLeftRB = ClawLeft.GetComponent<Rigidbody>();
        ClawRightRB = ClawRight.GetComponent<Rigidbody>();

        ClawLeftOrigin = ClawLeft.transform.localPosition;
        ClawRightOrigin = ClawRight.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleClawMovement();
        EnforceSymmetricClawMovement();
        ClampClawPosition();
        //Debug.Log(ClawLeftRB.velocity + "a");
    }



    private void HandleClawMovement()
    {
        if (ClawRight.transform.localPosition.x > ClawRightOrigin.x || ClawLeft.transform.localPosition.x < ClawLeftOrigin.x)
        {
            ResetClawPosition();
        }

        if (Input.GetMouseButtonDown(1) && !opened || opening)
        {
            OpenClaw();
        }

        if (Input.GetMouseButtonDown(0) || closing)
        {
            CloseClaw();
        }
    }

    private void ClampClawPosition()
    {
        ClawLeft.transform.localPosition = new Vector3(ClawLeft.transform.localPosition.x, 0f, 0f);
        ClawRight.transform.localPosition = new Vector3(ClawRight.transform.localPosition.x, 0f, 0f);
    }

    private void ResetClawPosition()
    {
        opened = true;
        opening = false;
        closed = false;
        closing = false;

        ClawRight.transform.localPosition = ClawRightOrigin;
        ClawLeft.transform.localPosition = ClawLeftOrigin;

        VelocityReset();
    }

    private void OpenClaw()
    {
        opening = true;
        closing = false;
        opened = false;
        closed = false;

        ClawLeftRB.AddForce(-transform.right * speed);
        ClawRightRB.AddForce(transform.right * speed);
    }

    private void CloseClaw()
    {
        opened = false;
        opening = false;
        closing = true;
        Debug.Log("closing claw: " + closing);

        ClawLeftRB.AddForce(transform.right * speed);
        ClawRightRB.AddForce(-transform.right * speed);
        if (ClawLeftRB.velocity.x < -1 || ClawLeftRB.velocity.x > 1)
        {
            closed = true;
        }

    }

    
    public void VelocityReset()
    {
        opened = true;
        opening = false;
        closing = false;
        ClawLeftRB.velocity = Vector3.zero;
        ClawRightRB.velocity = Vector3.zero;

        ClawLeftRB.angularVelocity = Vector3.zero;
        ClawRightRB.angularVelocity = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {

        Debug.Log("contact: " + closing);
        if (other.CompareTag("Grabbable") && closing)
        {
            TriggerParrent = true;
            Debug.Log("entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            TriggerParrent = false;
            Debug.Log("exited");
        }
    }

    void EnforceSymmetricClawMovement()
    {
        float leftOffset = ClawLeft.transform.localPosition.x - ClawLeftOrigin.x;
        ClawRight.transform.localPosition = new Vector3(ClawRightOrigin.x - leftOffset, ClawRight.transform.localPosition.y, ClawRight.transform.localPosition.z);
    }
}
