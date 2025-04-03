using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    private Vector3 position;
    private Vector3 rotation;

    public float minX = -10f, maxX = 10f;
    public float minY = 0f, maxY = 5f;
    public float minZ = -5f, maxZ = 5f;

    private Vector2 moveInput;
    private float rotateInput;

    private ClawControls controls;

    public float holdTime = 2.0f;
    private float timeHeld = 0f;

    private bool isResetting = false;

    private void Awake()
    {
        controls = new ClawControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.RotateLeft.performed += ctx => rotateInput = 1;
        controls.Player.RotateLeft.canceled += ctx => rotateInput = 0;
        controls.Player.RotateRight.performed += ctx => rotateInput = -1;
        controls.Player.RotateRight.canceled += ctx => rotateInput = 0;

        controls.Player.ResetLevel.performed += ctx => StartHoldReset();
        controls.Player.ResetLevel.canceled += ctx => StopHoldReset();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Start() => position = transform.position;

    void Update()
    {
        position.x = Mathf.Clamp(position.x + (moveInput.x * moveSpeed * Time.deltaTime), minX, maxX);
        position.z = Mathf.Clamp(position.z + (moveInput.y * moveSpeed * Time.deltaTime), minZ, maxZ);
        transform.position = position;

        rotation = transform.localEulerAngles;
        rotation.z += rotateInput * moveSpeed * 10 * Time.deltaTime;
        transform.localEulerAngles = rotation;

        if (isResetting)
        {
            timeHeld += Time.deltaTime;
            if (timeHeld >= holdTime)
            {
                ResetLevel();
                timeHeld = 0f;
            }
        }

        float rightStickY = controls.Player.MoveRightStick.ReadValue<Vector2>().y;
        position.y = Mathf.Clamp(position.y + (rightStickY * moveSpeed * Time.deltaTime), minY, maxY);
        transform.position = position;
    }

    private void StartHoldReset()
    {
        isResetting = true;
        timeHeld = 0f;
    }

    private void StopHoldReset()
    {
        isResetting = false;
        timeHeld = 0f;
    }

    private void ResetLevel()
    {
        Debug.Log("Resetting the level...");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
