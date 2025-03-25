using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RigClawController : MonoBehaviour
{
    public Image gaugeIndicatorObject; 
    public float gaugeMin, gaugeMax;

    public GameObject B_ClawIK_target;
    public GameObject T_ClawIK_target;

    public float gripPreassure = 0f;
    public float moveSpeed = 1.0f;
    public bool canClose = true;

    private bool openClawInput;
    private bool closeClawInput;

    private Vector3 TopTargetMin = new Vector3(0f, -0.4891f, -0.071f);
    private Vector3 TopTargetMax = new Vector3(0f, -0.150f, -0.071f);
    private Vector3 BottomTargetMin = new Vector3(0f, -0.4931f, 0.062f);
    private Vector3 BottomTargetMax = new Vector3(0f, -0.150f, 0.062f);
    private Vector3 InitialTopPos;
    private Vector3 InitialBottomPos;

    private void Awake()
    {
        B_ClawIK_target = GameObject.Find("B_ClawIK_target");
        T_ClawIK_target = GameObject.Find("T_ClawIK_target");

<<<<<<< Updated upstream
        //Bottom_Claw = GameObject.Find("RightTrigger");
        //Top_Claw = GameObject.Find("LeftTrigger");

=======
>>>>>>> Stashed changes
        var controls = new ClawControls();
        controls.Player.OpenClaw.performed += ctx => openClawInput = true;
        controls.Player.OpenClaw.canceled += ctx => openClawInput = false;
        controls.Player.CloseClaw.performed += ctx => closeClawInput = true;
        controls.Player.CloseClaw.canceled += ctx => closeClawInput = false;

        controls.Enable();

        if (gaugeIndicatorObject != null)
        {
            float gaugeStartPos = gaugeIndicatorObject.transform.position.x;
            gaugeMin = Mathf.Abs(gaugeStartPos * 2.265f);
            gaugeMax = gaugeStartPos;
        }
    }

    void Start()
    {
        InitialBottomPos = B_ClawIK_target.transform.localPosition;
        InitialTopPos = T_ClawIK_target.transform.localPosition;

        // Calculate the initial grip pressure based on the claw's starting positions
        float bottomGrip = Mathf.InverseLerp(BottomTargetMin.y, BottomTargetMax.y, InitialBottomPos.y);
        float topGrip = Mathf.InverseLerp(TopTargetMin.y, TopTargetMax.y, InitialTopPos.y);

        gripPreassure = (bottomGrip + topGrip) / 2f;
        Debug.Log($"Initial Grip Pressure: {gripPreassure}");
    }

    void Update()
    {
        gripPreassure = Mathf.Clamp(gripPreassure, 0, 1);

        if (closeClawInput && canClose)
        {
            if (gripPreassure > 0.001f)
            {
                gripPreassure = Mathf.MoveTowards(gripPreassure, 0, moveSpeed * Time.deltaTime);
                B_ClawIK_target.transform.localPosition = Vector3.Lerp(BottomTargetMin, BottomTargetMax, gripPreassure);
                T_ClawIK_target.transform.localPosition = Vector3.Lerp(TopTargetMin, TopTargetMax, gripPreassure);
            }
            else
            {
                canClose = false;
            }
        }
        else if (openClawInput)
        {
            gripPreassure = Mathf.MoveTowards(gripPreassure, 1, moveSpeed * Time.deltaTime);
            B_ClawIK_target.transform.localPosition = Vector3.Lerp(BottomTargetMin, BottomTargetMax, gripPreassure);
            T_ClawIK_target.transform.localPosition = Vector3.Lerp(TopTargetMin, TopTargetMax, gripPreassure);

<<<<<<< Updated upstream
            Debug.Log($"Opening: B_Claw {B_ClawIK_target.transform.localPosition.y}, T_Claw {T_ClawIK_target.transform.localPosition.y}");

            playerMovement = true;
            canClose = true;
=======
            if (gripPreassure >= 1.0f)
            {
                canClose = true;
            }
>>>>>>> Stashed changes
        }

        // Move the gauge UI element based on the gripPreassure
        if (gaugeIndicatorObject != null)
        {
            // Map gripPreassure (0 to 1) to the gauge's min/max positions
            float gaugePosition = Mathf.Lerp(gaugeMin, gaugeMax, gripPreassure);

            // Move the gauge sprite directly by adjusting the x position
            Vector3 newGaugePosition = gaugeIndicatorObject.transform.position;
            newGaugePosition.x = gaugePosition;

            // Apply the new position
            gaugeIndicatorObject.transform.position = newGaugePosition;
        }
    }
}