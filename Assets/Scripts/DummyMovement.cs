using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{
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
        Debug.Log(ClawLeftRB.velocity + " : " + ClawRightRB.velocity);

        if (ClawRight.transform.localPosition.x > ClawRightOrigin.x || ClawLeft.transform.localPosition.x < ClawLeftOrigin.x)
        {
            opened = true;
            opening = false;
            closing = false;

            ClawRight.transform.localPosition = ClawRightOrigin;
            ClawLeft.transform.localPosition = ClawLeftOrigin;

            VelocityReset();
        }

        
        if (Input.GetMouseButtonDown(1) && !opened || opening && !opened)
        {
            opening = true;
            closing = false;
            ClawLeftRB.AddForce(-transform.right * speed);
            ClawRightRB.AddForce(transform.right * speed);
        }

        
        if (Input.GetMouseButtonDown(0) || closing)
        {
            opened = false;
            opening = false;
            closing = true;
            ClawLeftRB.AddForce(transform.right * speed);
            ClawRightRB.AddForce(-transform.right * speed);
        }

        
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider != null && collision.collider.tag == "LeggyClaw")
        {
            VelocityReset();
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }

    public void VelocityReset() 
    {
        ClawLeftRB.velocity = Vector3.zero;
        ClawRightRB.velocity = Vector3.zero;


        ClawLeftRB.angularVelocity = Vector3.zero;
        ClawRightRB.angularVelocity = Vector3.zero;

    }
}
