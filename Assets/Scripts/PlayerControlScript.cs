using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControlScript : MonoBehaviour
{
    public GameObject tester;

    public GameObject ClawLeftPivot;
    public GameObject ClawRightPivot;

    public GameObject BaseJoint;

    public float rotationSpeed = 100f;
    public float rotationStep = 5f;
    private Quaternion baseJointRotation;

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
        baseJointRotation = BaseJoint.transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        bool rotated = false; // Flag to track if any rotation key was pressed

        // Handle rotation input
        if (Input.GetKey(KeyCode.S))
        {
            BaseJoint.transform.rotation *= Quaternion.Euler(rotationStep * Time.deltaTime, 0f, 0f); // Rotate Up
            rotated = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            BaseJoint.transform.rotation *= Quaternion.Euler(-rotationStep * Time.deltaTime, 0f, 0f); // Rotate Down
            rotated = true;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            BaseJoint.transform.rotation *= Quaternion.Euler(0f, -rotationStep * Time.deltaTime, 0f); // Rotate Left
            rotated = true;
        }
        if (Input.GetKey(KeyCode.E))
        {
            BaseJoint.transform.rotation *= Quaternion.Euler(0f, rotationStep * Time.deltaTime, 0f); // Rotate Right
            rotated = true;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            BaseJoint.transform.rotation *= Quaternion.Euler(0f, 0f, rotationStep * Time.deltaTime); // Rotate Z-axis
            rotated = true;
        }
        if (Input.GetKey(KeyCode.C))
        {
            BaseJoint.transform.rotation *= Quaternion.Euler(0f, 0f, -rotationStep * Time.deltaTime); // Rotate Z-axis (opposite)
            rotated = true;
        }

        // Handle movement input
        if (Input.GetKey(KeyCode.A))
        {
            BaseJoint.transform.position += new Vector3(-speed * Time.deltaTime, 0f, 0f); // Move Left on Z-axis
        }
        if (Input.GetKey(KeyCode.D))
        {
            BaseJoint.transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f); // Move Right on Z-axis
        }
    }

    void FixedUpdate()
    {
        // Raycast for mouse position tracking
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(ray.origin, hit.point);
            tester.transform.position = hit.point;
        }
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
