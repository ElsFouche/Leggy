using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControlScript : MonoBehaviour
{
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

    private bool available = true;
    private float timer;
    public float torque;
    public float speed;
    public float actionDuration = 2f;
    public int frameSkips = 3;
    public float inbetweenActionDelay = 0.5f;
    private float inActionDelay = 0.000000000000000000000000000000000000000000000001f;

    private bool restartPromptActive = false;

    void Start()
    {
        baseJointRotation = BaseJoint.transform.rotation;

        WristCCSprite.gameObject.SetActive(false);
        WristCSprite.gameObject.SetActive(false);
        LeftSprite.gameObject.SetActive(false);
        RightSprite.gameObject.SetActive(false);
        UpSprite.gameObject.SetActive(false);
        DownSprite.gameObject.SetActive(false);

        if (RestartPrompt != null)
            RestartPrompt.gameObject.SetActive(false); // Hide restart prompt at start
    }

    void Update()
    {
        HandleMovement();
        HandleRestart();
    }

    void HandleRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (restartPromptActive)
            {
                // If the player presses R again while the prompt is active, restart the level
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                // Show the restart prompt
                restartPromptActive = true;
                if (RestartPrompt != null)
                {
                    RestartPrompt.text = "Press R again to restart, or any other key to cancel.";
                    RestartPrompt.gameObject.SetActive(true);
                }
            }
        }

        // If the player presses any other key, cancel the restart prompt
        if (restartPromptActive && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.R))
        {
            restartPromptActive = false;
            if (RestartPrompt != null)
                RestartPrompt.gameObject.SetActive(false);
        }
    }

    void HandleMovement()
    {
        // Rotation and movement logic
        if (Input.GetKey(KeyCode.Q))
        {
            BaseJoint.transform.rotation *= Quaternion.Euler(0f, 0f, rotationStep * Time.deltaTime);
            WristCSprite.gameObject.SetActive(false);
            WristCCSprite.gameObject.SetActive(true);
            WristInput.text = "Q";
            WristCCSprite.transform.eulerAngles += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            BaseJoint.transform.rotation *= Quaternion.Euler(0f, 0f, -rotationStep * Time.deltaTime);
            WristCSprite.gameObject.SetActive(true);
            WristCCSprite.gameObject.SetActive(false);
            WristInput.text = "E";
            WristCSprite.transform.eulerAngles += new Vector3(0, 0, -rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            BaseJoint.transform.localPosition += new Vector3(0f, 0f, speed * Time.deltaTime);
            UpSprite.gameObject.SetActive(true);
            DownSprite.gameObject.SetActive(false);
            DepthInput.text = "W";
        }
        if (Input.GetKey(KeyCode.S))
        {
            BaseJoint.transform.localPosition += new Vector3(0f, 0f, -speed * Time.deltaTime);
            UpSprite.gameObject.SetActive(false);
            DownSprite.gameObject.SetActive(true);
            DepthInput.text = "S";
        }

        if (Input.GetKey(KeyCode.A))
        {
            BaseJoint.transform.position += new Vector3(-speed * Time.deltaTime, 0f, 0f);
            LeftSprite.gameObject.SetActive(true);
            RightSprite.gameObject.SetActive(false);
            GantryInput.text = "A";
        }
        if (Input.GetKey(KeyCode.D))
        {
            BaseJoint.transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
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
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            DepthInput.text = " ";
            UpSprite.gameObject.SetActive(false);
            DownSprite.gameObject.SetActive(false);
        }
    }
}
