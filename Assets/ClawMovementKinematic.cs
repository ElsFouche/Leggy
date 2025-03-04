using UnityEngine;
using UnityEngine.InputSystem;

public class ClawMovementKinematic : MonoBehaviour
{
    public GameObject hitobjectFlag;
    public BoxCollider Wrist_Kinematic_Collider;
    public float moveSpeed = 1.0f;
    private float stopDistance = 0.5f;
    public bool canClose = true;

    private Vector3 position;
    private Collider clawCollider;
    private ClawParent clawParent;
    private float castDistance;
    private RaycastHit hitResult;
    private bool castHit = false;
    private float successfulGrabRange = 0.5f;
    private float distanceToCenter;
    public GameObject hitObject;
    public bool playerMovement = false;
    private bool negativeDirection = false;
    private GameObject clawL;
    private GameObject clawR;
    private float clawWidth;

    // Input System
    private InputAction moveClawAction;
    private InputAction openClawAction;
    private InputAction closeClawAction;

    private float moveInput;
    private bool openClawInput;
    private bool closeClawInput;

    private void Awake()
    {
        var controls = new ClawControls();
        openClawAction = controls.Player.OpenClaw;
        closeClawAction = controls.Player.CloseClaw;

        openClawAction.performed += ctx => openClawInput = true;
        openClawAction.canceled += ctx => openClawInput = false;

        closeClawAction.performed += ctx => closeClawInput = true;
        closeClawAction.canceled += ctx => closeClawInput = false;
    }

    private void OnEnable()
    {
        openClawAction.Enable();
        closeClawAction.Enable();
    }

    private void OnDisable()
    {
        openClawAction.Disable();
        closeClawAction.Disable();
    }

    void Start()
    {
        clawParent = transform.parent.GetComponent<ClawParent>();
        clawCollider = transform.GetComponent<Collider>();
        position = transform.localPosition;
        castDistance = clawParent.castDistance;
        successfulGrabRange = clawParent.maxGrabRange;
        GameObject WristKinematic = GameObject.Find("Wrist_Kinematic");
        Wrist_Kinematic_Collider = WristKinematic.GetComponent<BoxCollider>();

        clawL = GameObject.Find("Claw_L");
        clawR = GameObject.Find("Claw_R");
        clawWidth = clawL.transform.localScale.x;

        if (transform.localPosition.x < 0)
        {
            negativeDirection = true;
            stopDistance = gameObject.transform.localScale.x / 2f;
        }
        else
        {
            stopDistance = -gameObject.transform.localScale.x / 2f;
        }
    }

    void Update()
    {
        // Move claw with Left/Right Stick or Q/E keys
        if (moveInput != 0)
        {
            position = transform.localPosition;
            position.x = Mathf.Clamp(position.x + (moveInput * moveSpeed * Time.deltaTime), -stopDistance, 2);
            transform.localPosition = position;
            playerMovement = true;
        }
        else
        {
            playerMovement = false;
        }

        // Opening and closing the claw with bumpers
        if (openClawInput)
        {
            position = transform.localPosition;
            position.x = Mathf.Clamp(position.x + (moveSpeed * Time.deltaTime), -stopDistance, 2);
            transform.localPosition = position;
        }
        else if (closeClawInput && canClose)
        {
            position = transform.localPosition;
            position.x = Mathf.Clamp(position.x - (moveSpeed * Time.deltaTime), -stopDistance, 2);
            transform.localPosition = position;
        }

        //perhaps issue with this having positional bias?
        if (Mathf.Abs(clawL.transform.localPosition.x) != Mathf.Abs(clawR.transform.localPosition.x))
        {
            clawL.transform.localPosition = new Vector3(-clawR.transform.localPosition.x, 0, 0);
        }

        // Begin box cast
        castHit = Physics.BoxCast(clawCollider.bounds.center,
                                  transform.localScale * 0.5f,
                                  transform.right,
                                  out hitResult,
                                  transform.localRotation,
                                  castDistance);

        if (castHit && hitResult.transform.CompareTag("Grabbable"))
        {
            hitObject = hitResult.transform.gameObject;
            distanceToCenter = Vector3.Distance(hitObject.GetComponent<Rigidbody>().centerOfMass, clawCollider.bounds.center);

            if (!gameObject.CompareTag(hitObject.tag))
            {
                canClose = false;
            }

            if (!(hitObject.CompareTag("ClawL") || hitObject.CompareTag("ClawR")))
            {
                hitobjectFlag.transform.position = hitObject.transform.position;

                if (openClawInput && distanceToCenter < successfulGrabRange)
                {
                    clawParent.clawIsGrabbing(hitObject.gameObject);
                }
            }

            if (distanceToCenter > successfulGrabRange)
            {
                Debug.Log("Too far from target.");
                clawParent.clawIsGrabbing();
            }
        }
        else
        {
            canClose = true;
            hitObject = null;
        }
    }
}
