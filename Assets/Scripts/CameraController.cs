using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Camera topDown;
    public Camera firstPerson;
    public Camera leftShoulder;
    public Camera rightShoulder;

    public bool onLeftView;
    public bool onLeftShoulder;

    private ClawControls controls; // Reference to the input controls

    // Time variables for handling the delay between camera switches
    public float switchDelay = 0.5f; // Delay between camera switches in seconds
    private float lastSwitchTime; // Last time the camera was switched

    // Start is called before the first frame update
    void Awake()
    {
        controls = new ClawControls(); // Initialize controls here

        // Initially enable the first person view and disable others
        firstPerson.enabled = true;
        topDown.enabled = false;
        leftShoulder.enabled = false;
        rightShoulder.enabled = false;

        onLeftView = false;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    // Update is called once per frame
    void Update()
    {
        // Get the D-pad input
        Vector2 dpadInput = controls.Player.Dpad.ReadValue<Vector2>();

        // Check if enough time has passed since the last camera switch
        if (Time.time - lastSwitchTime >= switchDelay)
        {
            // Handle the camera switching
            if (dpadInput.y > 0) SwitchCamera("up");   // D-pad up
            else if (dpadInput.y < 0) SwitchCamera("down");  // D-pad down
            else if (dpadInput.x < 0) SwitchCamera("left");  // D-pad left
            else if (dpadInput.x > 0) SwitchCamera("right"); // D-pad right
        }
    }

    // Switch the camera based on the direction pressed
    public void SwitchCamera(string directionPressed)
    {
        lastSwitchTime = Time.time; // Update the last switch time

        if (directionPressed == "up")
        {
            firstPerson.enabled = true;

            topDown.enabled = false;
            leftShoulder.enabled = false;
            rightShoulder.enabled = false;
        }

        if (directionPressed == "down")
        {
            topDown.enabled = true;
            firstPerson.enabled = false;
            leftShoulder.enabled = false;
            rightShoulder.enabled = false;
        }

        if (directionPressed == "left")
        {
            topDown.enabled = false;
            firstPerson.enabled = false;
            leftShoulder.enabled = true;
            rightShoulder.enabled = false;
        }

        if (directionPressed == "right")
        {
            topDown.enabled = false;
            firstPerson.enabled = false;
            leftShoulder.enabled = false;
            rightShoulder.enabled = true;
        }
    }
}
