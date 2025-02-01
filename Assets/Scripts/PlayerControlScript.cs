using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{
    public GameObject tester;

    public GameObject ClawLeftPivot;
    public GameObject ClawRightPivot;

    public GameObject BaseJoint;



    private Quaternion ClawLeftPivotClosedRotation;
    private Quaternion ClawRightPivotClosedRotation;
    private Rigidbody ClawLeftPivotRigid;
    private Rigidbody ClawRightPivotRigid;


    private bool available = true;
    private float timer;
    public float torque;

    public float speed;
    public float actionDuration = 2f;
    public int frameSkips = 3;
    public float inbetweenActionDelay = .5f;
    private float inActionDelay = .000000000000000000000000000000000000000000000001f;

    // Start is called before the first frame update    
    void Start()
    {
        ClawLeftPivotClosedRotation = ClawLeftPivot.transform.localRotation;
        ClawRightPivotClosedRotation = ClawRightPivot.transform.localRotation;

        ClawLeftPivotRigid = ClawLeftPivot.GetComponent<Rigidbody>();
        ClawRightPivotRigid = ClawRightPivot.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && available)
        {
            Debug.Log("Closing");
            //StartCoroutine(ClawPinching(true));
        }

        if (Input.GetMouseButtonDown(1) && available)
        {
            Debug.Log("Opening");
            //StartCoroutine(ClawPinching(false));
        }
    }

    void FixedUpdate()
    {
        //Torso Rotation: Raycast to find pos of mouse, then calc dist from mouse to torso, clamps x , z rotation making only y rotation.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.transform.name);
            Debug.DrawLine(ray.origin, hit.point);

            tester.transform.position = hit.point;
        }

        //float turn = Input.GetAxis("Horizontal");
        if (ClawLeftPivot)
        {

        }
        ClawLeftPivotRigid.AddTorque(transform.up * torque); // * turn);
        ClawRightPivotRigid.AddTorque(transform.up * torque); // * turn);



        /*
        var lookPos = hit.point - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        BaseJoint.transform.rotation = Quaternion.Slerp(BaseJoint.transform.rotation, rotation, Time.fixedDeltaTime * 30);
        */

    }



    private IEnumerator ClawPinching(bool state)
    {
        available = false;
        float smoothing;
        int temp = 0;

        //if enum runs with state true closes
        if (state)
        {

            
                

            /*
            Debug.Log("1");

            while (Quaternion.Angle(ClawLeftPivot.transform.rotation, Quaternion.Euler(0f, 0f, 0f)) > 0.1f && Quaternion.Angle(ClawRightPivot.transform.rotation, Quaternion.Euler(0f, 0f, 0f)) > 0.1f)
            {
                timer += Time.fixedDeltaTime;
                smoothing = Mathf.SmoothStep(0f, .1f, timer / actionDuration);

                Debug.Log("1.1");
                ClawLeftPivot.transform.rotation = Quaternion.Slerp(ClawLeftPivot.transform.rotation, Quaternion.Euler(0f, 0f, 0f), smoothing);
                ClawRightPivot.transform.rotation = Quaternion.Slerp(ClawRightPivot.transform.rotation, Quaternion.Euler(0f, 0f, 0f), smoothing);
                
                temp++;
                if (temp > frameSkips)
                {
                    yield return new WaitForSeconds(inActionDelay);
                    temp = 0;
                }
            }


            ClawLeftPivot.transform.rotation = Quaternion.Euler(0f, 0f, 0);
            ClawRightPivot.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            timer = 0;
            smoothing = 0;
            available = true;

            Debug.Log("1.3");

            yield break;
            */
        }

        //closes if enum runs as is false
        if (!state)
        {
            Debug.Log("2.1");
            while (Quaternion.Angle(ClawLeftPivot.transform.rotation, Quaternion.Euler(0f, 0f, 30f)) > 0.1f && Quaternion.Angle(ClawRightPivot.transform.rotation, Quaternion.Euler(0f, 0f, -30f)) > 0.1f)
            {
                timer += Time.fixedDeltaTime;
                smoothing = Mathf.SmoothStep(0f, .1f, timer / actionDuration);
                Debug.Log("2.2");

                ClawLeftPivot.transform.rotation = Quaternion.Lerp(ClawLeftPivot.transform.rotation, Quaternion.Euler(0f, 0f, 30f), smoothing);
                ClawRightPivot.transform.rotation = Quaternion.Lerp(ClawRightPivot.transform.rotation, Quaternion.Euler(0f, 0f, -30f), smoothing);
                temp++;
                if (temp > frameSkips)
                {
                    yield return new WaitForSeconds(inActionDelay);
                    temp = 0;
                }
            }


            ClawLeftPivot.transform.rotation = Quaternion.Euler(0f, 0f, 30f);
            ClawRightPivot.transform.rotation = Quaternion.Euler(0f, 0f, -30f);

            timer = 0;
            smoothing = 0;
            available = true;
            Debug.Log("2.3");
            yield break;
        }

        //redundant but incase available doesn't trigger 
        available = true;
    }
}
