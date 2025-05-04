using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RigClawController : MonoBehaviour
{
    public Image gaugeIndicatorObject;
    public float gaugeMin, gaugeMax;
    private float gaugeOffset = 50f;

    public GameObject B_ClawIK_target;
    public GameObject T_ClawIK_target;

    public float gripPreassure = 0f;
    public float moveSpeed = 1.0f;
    public bool canClose = true;

    private bool openClawInput;
    private bool closeClawInput;
    private bool playerMovement = false;

    private Vector3 TopTargetMin = new Vector3(0f, -0.4891f, -0.071f);
    private Vector3 TopTargetMax = new Vector3(0f, -0.150f, -0.071f);
    private Vector3 BottomTargetMin = new Vector3(0f, -0.4931f, 0.062f);
    private Vector3 BottomTargetMax = new Vector3(0f, -0.150f, 0.062f);
    private Vector3 InitialTopPos;
    private Vector3 InitialBottomPos;

    private InputAction openClawAction;
    private InputAction closeClawAction;

    private LeggyAudio leggyAudio;
    private GameManager gameManager;
    private bool isPlayingAudio;

    private void Awake()
    {
        B_ClawIK_target = GameObject.Find("B_ClawIK_target");
        T_ClawIK_target = GameObject.Find("T_ClawIK_target");

        var controls = new ClawControls();
        openClawAction = controls.Player.OpenClaw;
        closeClawAction = controls.Player.CloseClaw;

        openClawAction.performed += ctx => openClawInput = true;
        openClawAction.canceled += ctx => openClawInput = false;
        openClawAction.started += ctx => ClawOpenCloseSFX(ctx);
        openClawAction.canceled += ctx => ClawOpenCloseSFX(ctx);

        closeClawAction.performed += ctx => closeClawInput = true;
        closeClawAction.canceled += ctx => closeClawInput = false;
        closeClawAction.started += ctx => ClawOpenCloseSFX(ctx);
        closeClawAction.canceled += ctx => ClawOpenCloseSFX(ctx);

        controls.Enable();

        if (gaugeIndicatorObject != null)
        {
            float gaugeStartPos = gaugeIndicatorObject.transform.localPosition.x;
            Debug.Log("Gauge start position: " + gaugeStartPos);
            gaugeMin = gaugeStartPos;
            gaugeMax = gaugeStartPos * -1.0f;
            Debug.Log("Gauge min: " + gaugeMin);
            Debug.Log("Gauge max: " + gaugeMax);
        }
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
        InitialBottomPos = B_ClawIK_target.transform.localPosition;
        InitialTopPos = T_ClawIK_target.transform.localPosition;

        float bottomGrip = Mathf.InverseLerp(BottomTargetMin.y, BottomTargetMax.y, InitialBottomPos.y);
        float topGrip = Mathf.InverseLerp(TopTargetMin.y, TopTargetMax.y, InitialTopPos.y);
        gripPreassure = (bottomGrip + topGrip) / 2f;

        Debug.Log($"Initial Grip Pressure: {gripPreassure}");

        leggyAudio = GetComponent<LeggyAudio>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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
                playerMovement = true;
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
            playerMovement = true;
            canClose = true;
        }

        if (gaugeIndicatorObject != null)
        {
            float gaugePosition = Mathf.Lerp(gaugeMax, gaugeMin, gripPreassure);
            Vector3 newGaugePosition = gaugeIndicatorObject.transform.localPosition;
            newGaugePosition.x = gaugePosition;
            gaugeIndicatorObject.transform.localPosition = newGaugePosition;
        }
    }
    private void ClawOpenCloseSFX(InputAction.CallbackContext callback)
    {
        if (gameManager.IsInMenu()) { return; }
        if (leggyAudio == null) { Debug.Log("Audio component not found."); return; }

        if (callback.started)
        {
            if (isPlayingAudio)
            {
                StopClawSound();
            }
            leggyAudio.PlaySound(LeggyAudio.LeggySFX.ClawOpen);
            isPlayingAudio = true;
        }
        
        if (callback.canceled)
        {
            StopClawSound();
        }
    }

    private void StopClawSound()
    {
        if (leggyAudio == null) { Debug.Log("Audio component not found."); return; }
        isPlayingAudio = false;
        leggyAudio.StopSound(LeggyAudio.LeggySFX.ClawOpen);
    }
}
