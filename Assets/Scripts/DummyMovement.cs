using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{
    [SerializeField] private ClawGrabChild LeggyLeftClaw;
    [SerializeField] private ClawGrabChild LeggyRightClaw;

    public float speed = 2f;
    private bool opened = true;
    private bool closing = false;
    private bool opening = false;

    public GameObject ClawLeft;
    private Vector3 ClawLeftOrigin;
    private Rigidbody ClawLeftRB;


    public GameObject ClawRight;
    private Vector3 ClawRightOrigin;
    private Rigidbody ClawRightRB;

    public GameObject clawEmptyParent;

    public GameObject clawLeftClip;
    public GameObject clawRightClip;

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

        if (ClawRight.transform.localPosition.x < -0.1 || ClawRight.transform.localPosition.x > ClawRightOrigin.x || ClawLeft.transform.localPosition.x > 0.1 || ClawLeft.transform.localPosition.x < ClawLeftOrigin.x)
        {
            ResetClawPosition();
        }
    }



    private void HandleClawMovement()
    {
        /*
        if (ClawRight.transform.localPosition.x > ClawRightOrigin.x || ClawLeft.transform.localPosition.x < ClawLeftOrigin.x)
        {
            ResetClawPosition();
        }*/

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

        ClawLeftRB.isKinematic = false;
        ClawRightRB.isKinematic = false;

        ClawLeftRB.AddForce(-transform.right * speed, ForceMode.Impulse);
        ClawRightRB.AddForce(transform.right * speed, ForceMode.Impulse);
    }

    private void CloseClaw()
    {
        opened = false;
        opening = false;
        closing = true;

        ClawLeftRB.isKinematic = false;
        ClawRightRB.isKinematic = false;
        Debug.Log("closing claw: " + closing);

        ClawLeftRB.AddForce(transform.right * speed, ForceMode.Impulse);
        ClawRightRB.AddForce(-transform.right * speed, ForceMode.Impulse);

        if (LeggyLeftClaw.clawTriggerContact && !LeggyRightClaw.clawTriggerContact) 
        {
            ClawLeftRB.AddForce(transform.right * speed);
        }
        if(!LeggyLeftClaw.clawTriggerContact && LeggyRightClaw.clawTriggerContact)
        {

            ClawRightRB.AddForce(-transform.right * speed);
        }
    }

    
    public void VelocityReset()
    {
        opened = true;
        opening = false;
        closing = false;

        ClawLeftRB.isKinematic = true;
        ClawRightRB.isKinematic = true;
        /*
        ClawLeftRB.velocity = Vector3.zero;
        ClawRightRB.velocity = Vector3.zero;

        ClawLeftRB.angularVelocity = Vector3.zero;
        ClawRightRB.angularVelocity = Vector3.zero;
        */
    }

    /* old version of trigger enter parrenting system which would kick in if the rigidbodies didnt register objects | Unreliable ):
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
    */

    void EnforceSymmetricClawMovement()
    {
        
        float leftOffset = ClawLeft.transform.localPosition.x - ClawLeftOrigin.x;
        ClawRight.transform.localPosition = new Vector3(ClawRightOrigin.x - leftOffset, ClawRight.transform.localPosition.y, ClawRight.transform.localPosition.z);
        
    }
}
