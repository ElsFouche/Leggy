using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BackAndForth movementAnchor;

    public Camera topDown;
    public Camera firstPerson;
    public Camera leftView;
    public Camera rightView;
    public Camera leftShoulder;
    public Camera rightShoulder;

    public bool onLeftView;
    public bool onLeftShoulder;
    

    
    // Start is called before the first frame update
    void Start()
    {
        movementAnchor = FindObjectOfType<BackAndForth>();
        
        firstPerson.enabled = true;

        topDown.enabled = false;
        leftView.enabled = false;
        rightView.enabled = false;
        leftShoulder.enabled = false;
        rightShoulder.enabled = false;

        onLeftView = false;
    }

    // Update is called once per frame
    void Update()
    {
        leftShoulder.transform.position = movementAnchor.transform.position + (Vector3.left * 3) + Vector3.back + (Vector3.up * 0.5f);
        rightShoulder.transform.position = movementAnchor.transform.position + (Vector3.right * 3) + Vector3.back + (Vector3.up * 0.5f);

        if (Input.GetKeyDown(KeyCode.UpArrow)) switchCamera("up");
        if (Input.GetKeyDown(KeyCode.DownArrow)) switchCamera("down");
        if (Input.GetKeyDown(KeyCode.LeftArrow)) switchCamera("left");
        if (Input.GetKeyDown(KeyCode.RightArrow)) switchCamera("right");
    }

    public void switchCamera(string directionPressed)
    {
        if (directionPressed == "up")
        {
            firstPerson.enabled = true;

            topDown.enabled = false;
            leftView.enabled = false;
            rightView.enabled = false;
            leftShoulder.enabled = false;
            rightShoulder.enabled = false;

            onLeftShoulder = false;
            onLeftView = false;
        }

        if (directionPressed == "down")
        {
            topDown.enabled = true;

            firstPerson.enabled = false;
            leftView.enabled = false;
            rightView.enabled = false;
            leftShoulder.enabled = false;
            rightShoulder.enabled = false;

            onLeftShoulder = false;
            onLeftView = false;
        }

        if (directionPressed == "left")
        {
            topDown.enabled = false;
            firstPerson.enabled = false;
            leftView.enabled = false;
            rightView.enabled = false;
            leftShoulder.enabled = false;
            rightShoulder.enabled = false;

            if (!onLeftView)
            {
                leftView.enabled = true;
            }
            else
            {
                rightView.enabled = true;
            }

            onLeftView = !onLeftView;
            onLeftShoulder = false;
        }

        if (directionPressed == "right")
        {
            topDown.enabled = false;
            firstPerson.enabled = false;
            leftView.enabled = false;
            rightView.enabled = false;
            leftShoulder.enabled = false;
            rightShoulder.enabled = false;

            if (!onLeftShoulder)
            {
                leftShoulder.enabled = true;
            }
            else
            {
                rightShoulder.enabled = true;
            }

            onLeftShoulder = !onLeftShoulder;
            onLeftView = false;
        }
    }
    
}
