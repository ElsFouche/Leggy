using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggyCarrotOnStick : MonoBehaviour
{
    public GameObject LeggyWristPivot; // The pivot point for rotation
    public GameObject LeggyClawCenter; // Claw's main object (not used for rotation anymore)
    public PlayerControlScript playerControl;

    [Header("Rotation Limits")]
    public float minXRotation = -30f; // Minimum X rotation limit
    public float maxXRotation = 30f;  // Maximum X rotation limit
    public float minYRotation = -45f; // Minimum Y rotation limit
    public float maxYRotation = 45f;  // Maximum Y rotation limit

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float xMin = -5f, xMax = 5f;
    public float yMin = 1f, yMax = 10f;

    private Vector3 targetPosition;
    private bool isMouseLocked = false; // Tracks if the cursor is locked

    void Start()
    {
        // Initialize targetPosition to the position of the GameObject this script is attached to
        targetPosition = transform.position;
    }

    void Update()
    {
        // Toggle mouse lock with "L" key
        if (Input.GetKeyDown(KeyCode.L))
        {
            isMouseLocked = !isMouseLocked;
            ToggleMouseLock(isMouseLocked);
        }
    }

    void FixedUpdate()
    {
        if (isMouseLocked)
        {
            // Get mouse input for movement
            float moveX = Input.GetAxis("Mouse X");
            float moveY = Input.GetAxis("Mouse Y");

            // Apply movement to the target position, allowing Z movement to be free
            targetPosition += new Vector3(moveX, moveY, 0) * moveSpeed * Time.fixedDeltaTime;

            // Clamp X and Y movements, but leave Z axis movement unrestricted
            targetPosition.x = Mathf.Clamp(targetPosition.x, xMin, xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, yMin, yMax);

            // Update the actual position of the GameObject this script is attached to
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z); // Preserve the original Z
        }

        // Rotate the wrist pivot towards the carrot while keeping the player's Z-rotation intact
        RotateWristTowardsCarrot();
    }


    void RotateWristTowardsCarrot()
    {
        // Calculate the direction from the wrist pivot to the target object
        Vector3 directionToCarrot = transform.position - LeggyWristPivot.transform.position;

        // Get the wrist Z rotation directly from PlayerControlScript
        float wristZRotation = playerControl.GetWristZRotation();

        // Clamp wrist Z rotation to avoid excessive rotation
        //wristZRotation = Mathf.Clamp(wristZRotation, -90f, 90f); // Example limits, adjust as needed

        // Calculate the look-at rotation for the wrist, ignoring Z rotation
        Quaternion lookAtRotation = Quaternion.LookRotation(directionToCarrot);

        // Extract the current Euler angles of the look-at rotation
        Vector3 lookAtEuler = lookAtRotation.eulerAngles;

        // Combine the Z rotation of the LookAt and the player's input
        float combinedZRotation = Mathf.LerpAngle(lookAtEuler.z, wristZRotation, 0.5f); 
        lookAtEuler.z = combinedZRotation;
        lookAtRotation = Quaternion.Euler(lookAtEuler);
        LeggyWristPivot.transform.localRotation = lookAtRotation;
    }




    void ToggleMouseLock(bool lockMouse)
    {
        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
