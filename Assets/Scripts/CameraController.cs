using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
        firstPerson.enabled = true;

        topDown.enabled = false;
        leftView.enabled = false;
        rightView.enabled = false;
        leftShoulder.enabled = false;
        rightShoulder.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8)) switchCamera("up");
        if (Input.GetKeyDown(KeyCode.Keypad2)) switchCamera("down");
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
        }

        if (directionPressed == "down")
        {
            topDown.enabled = true;

            firstPerson.enabled = false;
            leftView.enabled = false;
            rightView.enabled = false;
            leftShoulder.enabled = false;
            rightShoulder.enabled = false;
        }
    }
}
