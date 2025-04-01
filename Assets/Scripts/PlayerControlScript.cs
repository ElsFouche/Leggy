using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerControlScript : MonoBehaviour
{
    public Button RestartButton;

    public TextMeshProUGUI WristInput;
    public GameObject WristCCSprite;
    public GameObject WristCSprite;
    private float WristRotation;

    public TextMeshProUGUI GantryInput;
    public GameObject RightSprite;
    public GameObject LeftSprite;

    public TextMeshProUGUI DepthInput;
    public GameObject UpSprite;
    public GameObject DownSprite;

    public TextMeshProUGUI RestartPrompt; // UI Text to show restart confirmation

    public GameObject tester;
    public GameObject BaseJoint;

    public float rotationSpeed = 100f;
    public float rotationStep = 5f;
    private Quaternion baseJointRotation;
    private float wristZRotation = 0f;

    private bool available = true;
    private float timer;
    public float torque;
    public float speed;
    public float actionDuration = 2f;
    public int frameSkips = 3;
    public float inbetweenActionDelay = 0.5f;
    private float inActionDelay = 0.000000000000000000000000000000000000000000000001f;

    private bool restartPromptActive = false;

    public float minX = -10f, maxX = 10f; // Min/Max for X movement
    public float minY = 0f, maxY = 5f;   // Min/Max for Y movement (controlled by Up/Down arrow keys)
    public float minZ = -5f, maxZ = 5f;   // Min/Max for Z movement (controlled by W/S keys)

    void Start()
    {
        baseJointRotation = BaseJoint.transform.rotation;

        WristCCSprite.gameObject.SetActive(false);
        WristCSprite.gameObject.SetActive(false);
        LeftSprite.gameObject.SetActive(false);
        RightSprite.gameObject.SetActive(false);
        UpSprite.gameObject.SetActive(false);
        DownSprite.gameObject.SetActive(false);

        RestartPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleMovement();

        // Handle hiding the restart prompt if any key other than 'R' is pressed
        if (restartPromptActive && (Input.anyKeyDown && !Input.GetMouseButtonDown(0))) // Exclude left mouse click
        {
            restartPromptActive = false;
            if (RestartPrompt != null)
            {
                RestartPrompt.gameObject.SetActive(false);  // Hide the restart prompt
            }
        }
    }

    // Getter to expose wristZRotation to other scripts
    public float GetWristZRotation()
    {
        return wristZRotation;
    }

    public void OnRestartButtonClick()
    {
        if (restartPromptActive)
        {
            // If the restart prompt is active, restart the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Show the restart prompt
            restartPromptActive = true;
            if (RestartPrompt != null)
            {
                RestartPrompt.text = "Press restart again to confirm, or any other button to cancel.";
                RestartPrompt.gameObject.SetActive(true);
            }
        }
    }

    void HandleMovement()
    {
        // Rotate wrist directly instead of applying incremental quaternion multiplications
        if (Input.GetKey(KeyCode.Q))
        {
            wristZRotation += rotationStep * Time.deltaTime;
            WristCSprite.gameObject.SetActive(false);
            WristCCSprite.gameObject.SetActive(true);
            WristInput.text = "Q";
        }
        if (Input.GetKey(KeyCode.E))
        {
            wristZRotation -= rotationStep * Time.deltaTime;
            WristCSprite.gameObject.SetActive(true);
            WristCCSprite.gameObject.SetActive(false);
            WristInput.text = "E";
        }

        // Update position for Z axis with W/S keys
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 newPosition = BaseJoint.transform.localPosition + new Vector3(0f, 0f, speed * Time.deltaTime);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
            BaseJoint.transform.localPosition = newPosition;
            UpSprite.gameObject.SetActive(false);
            DownSprite.gameObject.SetActive(false);

            DepthInput.text = "W";
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 newPosition = BaseJoint.transform.localPosition + new Vector3(0f, 0f, -speed * Time.deltaTime);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
            BaseJoint.transform.localPosition = newPosition;
            UpSprite.gameObject.SetActive(false);
            DownSprite.gameObject.SetActive(false);

            DepthInput.text = "S";
        }

        // Update position for Y axis with Up/Down Arrow keys
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 newPosition = BaseJoint.transform.localPosition + new Vector3(0f, speed * Time.deltaTime, 0f);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            BaseJoint.transform.localPosition = newPosition;
            UpSprite.gameObject.SetActive(true);
            DownSprite.gameObject.SetActive(false);

            DepthInput.text = "Up Arrow";
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 newPosition = BaseJoint.transform.localPosition + new Vector3(0f, -speed * Time.deltaTime, 0f);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            BaseJoint.transform.localPosition = newPosition;
            UpSprite.gameObject.SetActive(false);
            DownSprite.gameObject.SetActive(true);

            DepthInput.text = "Down Arrow";
        }

        // Control X movement with A/D keys (already part of the code)
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 newPosition = BaseJoint.transform.localPosition + new Vector3(-speed * Time.deltaTime, 0f, 0f);
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            BaseJoint.transform.localPosition = newPosition;
            LeftSprite.gameObject.SetActive(true);
            RightSprite.gameObject.SetActive(false);
            GantryInput.text = "A";
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 newPosition = BaseJoint.transform.localPosition + new Vector3(speed * Time.deltaTime, 0f, 0f);
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            BaseJoint.transform.localPosition = newPosition;
            LeftSprite.gameObject.SetActive(false);
            RightSprite.gameObject.SetActive(true);
            GantryInput.text = "D";
        }

        // Reset UI when no input is detected
        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
        {
            WristInput.text = " ";
            WristCSprite.gameObject.SetActive(false);
            WristCCSprite.gameObject.SetActive(false);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            GantryInput.text = " ";
            LeftSprite.gameObject.SetActive(false);
            RightSprite.gameObject.SetActive(false);
        }
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            DepthInput.text = " ";
            UpSprite.gameObject.SetActive(false);
            DownSprite.gameObject.SetActive(false);
        }
    }

    // Draw the bounds in the scene view
    void OnDrawGizmos()
    {
        if (BaseJoint == null) return;

        Gizmos.color = Color.green;

        // Convert local min/max to world space
        Vector3 minPos = BaseJoint.transform.TransformPoint(new Vector3(minX, minY, minZ));
        Vector3 maxPos = BaseJoint.transform.TransformPoint(new Vector3(maxX, maxY, maxZ));

        // Draw the boundary cube
        Gizmos.DrawWireCube((minPos + maxPos) / 2, maxPos - minPos);

        // Draw min and max points
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(minPos, 0.2f);
        Gizmos.DrawSphere(maxPos, 0.2f);
    }
}
