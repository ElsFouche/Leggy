using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggyCarrotOnStick : MonoBehaviour
{
    public GameObject LeggyWristPivot; // The pivot point for rotation
    public GameObject LeggyClawCenter; // Claw's main object (not used for rotation anymore)

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
        targetPosition = LeggyWristPivot.transform.position;
        transform.position = targetPosition;
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
            // Get mouse input
            float moveX = Input.GetAxis("Mouse X");
            float moveY = Input.GetAxis("Mouse Y");

            // Apply movement
            targetPosition += new Vector3(moveX, moveY, 0) * moveSpeed * Time.fixedDeltaTime;

            // Clamp within limits
            targetPosition.x = Mathf.Clamp(targetPosition.x, xMin, xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, yMin, yMax);

            // Apply position
            transform.position = targetPosition;
        }

        // Rotate the wrist pivot towards the carrot
        RotateWristTowardsCarrot();
    }

    void RotateWristTowardsCarrot()
    {
        Vector3 directionToCarrot = transform.position - LeggyWristPivot.transform.position;

        if (directionToCarrot != Vector3.zero) // Prevent NaN errors
        {
            // Get target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToCarrot);

            // Convert to Euler to clamp X and Y angles
            Vector3 eulerRotation = targetRotation.eulerAngles;

            // Convert to -180 to 180 range for proper clamping
            eulerRotation.x = (eulerRotation.x > 180) ? eulerRotation.x - 360 : eulerRotation.x;
            eulerRotation.y = (eulerRotation.y > 180) ? eulerRotation.y - 360 : eulerRotation.y;

            // Clamp angles using public values
            eulerRotation.x = Mathf.Clamp(eulerRotation.x, minXRotation, maxXRotation);
            eulerRotation.y = Mathf.Clamp(eulerRotation.y, minYRotation, maxYRotation);

            // Convert back to Quaternion and apply rotation
            LeggyWristPivot.transform.rotation = Quaternion.Euler(eulerRotation);
        }
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
