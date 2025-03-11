using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class RigClawController : MonoBehaviour
{
    public GameObject B_ClawIK_target;
    public GameObject T_ClawIK_target;

    public GameObject Bottom_Claw;
    public GameObject Top_Claw;

    public float gripPreassure = 0f;

    public float moveSpeed = 1.0f;
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


    // Input System
    private InputAction moveClawAction;
    private InputAction openClawAction;
    private InputAction closeClawAction;

    private bool openClawInput;
    private bool closeClawInput;
    private float stopMin, stopMax;

    private Vector3 TopTargetMin = new Vector3(0f, -0.4891f, -0.071f), TopTargetMax = new Vector3(0f, -0.150f, -0.071f);
    private Vector3 BottomTargetMin = new Vector3(0f, -0.4931f, 0.062f), BottomTargetMax = new Vector3(0f, -0.150f, .062f);
    private Vector3 InitialTopPos;
    private Vector3 InitialBottomPos;
    private float botYRatio;
    private float topYRatio;

    private void Awake()
    {
        B_ClawIK_target = GameObject.Find("B_ClawIK_target");
        T_ClawIK_target = GameObject.Find("T_ClawIK_target");

        Bottom_Claw = GameObject.Find("RightTrigger");
        Top_Claw = GameObject.Find("LeftTrigger");

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
    }

    void Start()
    {
        InitialBottomPos = B_ClawIK_target.gameObject.transform.localPosition;
        InitialTopPos = T_ClawIK_target.gameObject.transform.localPosition;
        botYRatio= InitialBottomPos.y / BottomTargetMin.y;
        topYRatio = InitialTopPos.y / TopTargetMin.y;

        gripPreassure = (botYRatio + topYRatio) / 2;

        Debug.Log("bot Grip %: " + botYRatio + " : top Grip % " + topYRatio);
    }

    void Update()
    {
        gripPreassure = Mathf.Clamp(gripPreassure, 0, 1);

        if (closeClawInput && canClose)
        {
            if (gripPreassure > 0.001)
            {
                gripPreassure -= gripPreassure * moveSpeed;
                B_ClawIK_target.transform.localPosition = new Vector3
                    (B_ClawIK_target.transform.localPosition.x, 
                    Mathf.Clamp(B_ClawIK_target.transform.localPosition.x * gripPreassure * botYRatio, BottomTargetMin.y, BottomTargetMin.y), 
                    B_ClawIK_target.transform.localPosition.z);
                Debug.Log("B_Claw " + B_ClawIK_target.transform.localPosition.y);
                T_ClawIK_target.transform.localPosition = new Vector3
                    (T_ClawIK_target.transform.localPosition.x,
                    Mathf.Clamp(T_ClawIK_target.transform.localPosition.x * gripPreassure * botYRatio, TopTargetMin.y, TopTargetMin.y),
                    T_ClawIK_target.transform.localPosition.z);
                Debug.Log("T_Claw " + T_ClawIK_target.transform.localPosition.y);

                playerMovement = true;
            }
            else { canClose = false; }

        }
        else if (openClawInput)
        {
            if (negativeDirection)
                position.x = Mathf.Clamp(position.x - (moveSpeed * Time.deltaTime), stopMin, stopMax);
            else
                position.x = Mathf.Clamp(position.x + (moveSpeed * Time.deltaTime), stopMin, stopMax);
            playerMovement = true;
            transform.localPosition = position;
            //clawParent.clawIsGrabbing();

        }

    }
}
