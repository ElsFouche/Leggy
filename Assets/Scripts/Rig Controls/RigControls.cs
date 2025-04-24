using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigControls : MonoBehaviour
{
    public GameObject ArmIK_target;
    public GameObject parentGameObject;
    public GameObject armRotationObject;
    public GameObject ArmIK;

    [Header("Reset Interval For IK Target")]
    [SerializeField] IkTargetFallback ikTargetFallback;
    public GameObject ArmIKFallback;
    public float ArmIK_target_ResetInterval = 1.0f;
    private bool ArmFallbackTriggered = false;


    public float moveSpeed = 1.0f;
    public float rotationSpeed = 100f;
    public float baseRotationSpeed = 50f;
    public float ikVerticalMoveSpeed = 0.5f;
    public float bodyRotationSpeed = 50f;

    public float ikMinRotationX = -45f;
    public float ikMaxRotationX = 45f;
    public float ikMinRotationZ = -45f;
    public float ikMaxRotationZ = 45f;

    public float ikMinZ = -1.53f;
    public float ikMaxZ = -0.2f;
    public float ikMinY;
    public float ikMaxY;
    private float ikModifiedMinX;
    private float ikModifiedMaxX;

    public float baseMinRotation = -45f;
    public float baseMaxRotation = 45f;

    public float gantryMinX = -2.0f; 
    public float gantryMaxX = 2.0f;

    public float clawVerticalInput = 0f;

    private Vector3 localPosition;
    private Vector2 leftStickInput;
    private Vector2 rightStickInput;
    private float bodyRotationInput;
    private ClawControls controls;

    private bool isResetting = false;
    public float holdTime = 2.0f;
    private float timeHeld = 0f;

    public GameObject circleMeter;

    public float hightMultiplier;

    private void Awake()
    {
        controls = new ClawControls();

        controls.Player.Move.performed += ctx => leftStickInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => leftStickInput = Vector2.zero;

        controls.Player.MoveRightStick.performed += ctx => rightStickInput = new Vector2(-ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y);
        controls.Player.MoveRightStick.canceled += ctx => rightStickInput = Vector2.zero;

        controls.Player.RotateLeft.performed += ctx => bodyRotationInput = -1f;
        controls.Player.RotateLeft.canceled += ctx => bodyRotationInput = 0f;

        controls.Player.RotateRight.performed += ctx => bodyRotationInput = 1f;
        controls.Player.RotateRight.canceled += ctx => bodyRotationInput = 0f;

        controls.Player.ClawVerticalUp.performed += ctx => clawVerticalInput = 1f;
        controls.Player.ClawVerticalUp.canceled += ctx => clawVerticalInput = 0f;

        controls.Player.ClawVerticalDown.performed += ctx => clawVerticalInput = -1f;
        controls.Player.ClawVerticalDown.canceled += ctx => clawVerticalInput = 0f;

        controls.Player.ResetLevel.performed += ctx => StartHoldReset();
        controls.Player.ResetLevel.canceled += ctx => StopHoldReset();

        armRotationObject = GameObject.Find("Base_twist_jnt");
        ArmIK_target = GameObject.Find("ArmIK_target");
        ArmIK = GameObject.Find("ArmIK");
        if (ArmIK_target == null) Debug.LogError("ArmIK_target not found!");

        if (parentGameObject == null) Debug.LogError("Parent GameObject is not assigned!");
    }

    private void Start()
    {
        circleMeter.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Update()
    {
        hightMultiplier = ArmIK_target.transform.localPosition.y / ikMaxY;
        Debug.Log(hightMultiplier + " " + ArmIK_target.transform.localPosition.y + " " + ikMaxY);
        ikModifiedMinX = ikMinRotationX * hightMultiplier;
        ikModifiedMaxX = ikMaxRotationX * hightMultiplier;


        if (ArmIK_target == null || parentGameObject == null) return;

        if (!ArmFallbackTriggered && !ikTargetFallback.IK_Target_Still_In_Range)
        {
            ArmFallbackTriggered = true;
            StartCoroutine(ResetArmIK());  
        }

        ArmIK.transform.rotation = armRotationObject.transform.rotation;

        // Move gantry left/right with clamping
        float newX = parentGameObject.transform.position.x + (-leftStickInput.x * moveSpeed * Time.deltaTime);
        newX = Mathf.Clamp(newX, gantryMinX, gantryMaxX);
        parentGameObject.transform.position = new Vector3(newX, parentGameObject.transform.position.y, parentGameObject.transform.position.z);

        // Move IK target forward/backward
        localPosition = ArmIK_target.transform.localPosition;
        localPosition.z -= leftStickInput.y * moveSpeed * Time.deltaTime;
        localPosition.z = Mathf.Clamp(localPosition.z, ikMinZ, ikMaxZ);
        localPosition.y = Mathf.Clamp(localPosition.y, ikMinY, ikMaxY);
        ArmIK_target.transform.localPosition = localPosition;

        // Rotate IK target
        Quaternion currentRotation = ArmIK_target.transform.localRotation;

        float targetRotationX = currentRotation.eulerAngles.x + (rightStickInput.y * rotationSpeed * Time.deltaTime);
        if (targetRotationX > 180f) targetRotationX -= 360f;
        //bool reachedXLimit = targetRotationX <= ikMinRotationX || targetRotationX >= ikMaxRotationX;
        targetRotationX = Mathf.Clamp(targetRotationX, ikModifiedMinX, ikModifiedMaxX);

        float targetRotationZ = currentRotation.eulerAngles.z + (rightStickInput.x * rotationSpeed * Time.deltaTime);
        if (targetRotationZ > 180f) targetRotationZ -= 360f;
        //bool reachedZLimit = targetRotationZ <= ikMinRotationZ || targetRotationZ >= ikMaxRotationZ;
        targetRotationZ = Mathf.Clamp(targetRotationZ, ikMinRotationZ, ikMaxRotationZ);

        ArmIK_target.transform.localRotation = Quaternion.Euler(targetRotationX, currentRotation.y, targetRotationZ);

        /* Base rotation adjustments if IK reaches limits
        if (reachedZLimit && rightStickInput.x != 0)
        {
            float rotationDirection = -Mathf.Sign(rightStickInput.x);
            armRotationObject.transform.Rotate(Vector3.up, rotationDirection * baseRotationSpeed * Time.deltaTime); // may be funky
        }*/

        if (clawVerticalInput != 0)
        {
            localPosition = ArmIK_target.transform.localPosition;
            float heightDirection = clawVerticalInput;
            localPosition.y += heightDirection * ikVerticalMoveSpeed * Time.deltaTime;
            localPosition.y = Mathf.Clamp(localPosition.y, ikMinY, ikMaxY);
            ArmIK_target.transform.localPosition = localPosition;
        }

        // Rotate body
        if (bodyRotationInput != 0)
        {
            float newRotation = armRotationObject.transform.eulerAngles.y + bodyRotationInput * bodyRotationSpeed * Time.deltaTime;
            if (newRotation > 180f) newRotation -= 360f;
            newRotation = Mathf.Clamp(newRotation, baseMinRotation, baseMaxRotation);
            
            armRotationObject.transform.rotation = Quaternion.Euler(0, newRotation, 0);
        }

        // Reset level if button is held
        if (isResetting)
        {
            timeHeld += Time.deltaTime;
            circleMeter.GetComponent<UnityEngine.UI.Image>().fillAmount += (Time.deltaTime / holdTime);
            if (timeHeld >= holdTime)
            {
                ResetLevel();
                timeHeld = 0f;
                circleMeter.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
            }
        }
    }

    private void StartHoldReset()
    {
        isResetting = true;
        timeHeld = 0f;
        circleMeter.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
    }

    private void StopHoldReset()
    {
        isResetting = false;
        timeHeld = 0f;
        circleMeter.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
    }

    private void ResetLevel()
    {
        Debug.Log("Resetting the level...");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public IEnumerator ResetArmIK()
    {
        ArmIK_target.transform.position = ArmIKFallback.transform.position;
        yield return new WaitForSeconds(ArmIK_target_ResetInterval);
        ArmFallbackTriggered = false;
    }
}
