using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggyAudio : MonoBehaviour
{
    public enum CameraView
    {
        none,
        TopDown,
        LeftShoulder,
        RightShoulder,
        FirstPerson
    }

    public enum LeggySFX
    {
        none,
        ArmDepth,
        ArmHeight,
        ArmLockout,
        ClawOpen,
        ClawLockout,
        Gantry,
        GantryLockout,
        Rotation,
        RotationLockout,
        WristMovement,
        Grab
    }

    [SerializeField] EventReference ArmDepth;
    [SerializeField] EventReference ArmHeight;
    [SerializeField] EventReference ArmLockout;
    [SerializeField] EventReference ClawOpen;
    [SerializeField] EventReference ClawLockout;
    [SerializeField] EventReference Gantry;
    [SerializeField] EventReference GantryLockout;
    [SerializeField] EventReference Rotation;
    [SerializeField] EventReference RotationLockout;
    [SerializeField] EventReference WristMovement;
    [SerializeField] EventReference Grab;

    private EventInstance armDepthInst;
    private EventInstance armHeightInst;
    private EventInstance armLockoutInst;
    private EventInstance clawOpenInst;
    private EventInstance clawLockoutInst;
    private EventInstance gantryInst;
    private EventInstance gantryLockoutInst;
    private EventInstance rotationInst;
    private EventInstance rotationLockoutInst;
    private EventInstance wristMovementInst;
    private EventInstance grabInst;
    private PLAYBACK_STATE currPlaybackState;

    private CameraController cameraController;
    private Camera topDownCamera, leftShoulderCamera, rightShoulderCamera, firstPersonCamera;
    private StudioListener topDownListener, leftListener, rightListener, firstPersonListener;

    private void Awake()
    {
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        cameraController = GetComponent<CameraController>();
        yield return new WaitForEndOfFrame();
        topDownCamera = cameraController.topDown;
        leftShoulderCamera = cameraController.leftShoulder;
        rightShoulderCamera = cameraController.rightShoulder;
        firstPersonCamera = cameraController.firstPerson;
        yield return new WaitForEndOfFrame();
        topDownListener = topDownCamera.gameObject.GetComponent<StudioListener>();
        leftListener = topDownCamera.gameObject.GetComponent<StudioListener>();
        rightListener = topDownCamera.gameObject.GetComponent<StudioListener>();
        firstPersonListener = topDownCamera.gameObject.GetComponent<StudioListener>();
    }

    public void SetListener(CameraView cameraView)
    {
        if (topDownListener == null
            || leftListener == null
            || rightListener == null
            || firstPersonCamera == null) { return; }

        switch (cameraView)
        {
            case CameraView.TopDown:
                topDownListener.enabled = true;
                leftListener.enabled = false;
                rightListener.enabled = false;
                firstPersonListener.enabled = false;
                break;
            case CameraView.LeftShoulder:
                leftListener.enabled = true;
                topDownListener.enabled = false;
                rightListener.enabled = false;
                firstPersonListener.enabled = false;
                break;
            case CameraView.RightShoulder:
                rightListener.enabled = true;
                topDownListener.enabled = false;
                leftListener.enabled = false;
                firstPersonListener.enabled = false;
                break;
            case CameraView.FirstPerson:
                firstPersonListener.enabled = true;
                topDownListener.enabled = false;
                leftListener.enabled = false;
                rightListener.enabled = false;
                break;
            default:
                break;
        }
    }

    public void PlaySound(LeggySFX playSFX)
    {
        switch (playSFX)
        {
            case LeggySFX.none:
                break;
            case LeggySFX.ArmDepth:
                armDepthInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED 
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; } 
                Debug.Log("Playing " + ArmDepth + ". Playback state: " + currPlaybackState);
                armDepthInst = RuntimeManager.CreateInstance(ArmDepth);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(armDepthInst, GetComponent<Transform>());
                armDepthInst.start();
                break;
            case LeggySFX.ArmHeight:
                armHeightInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                armHeightInst = RuntimeManager.CreateInstance(ArmHeight);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(armHeightInst, GetComponent<Transform>());
                armHeightInst.start();
                break;
            case LeggySFX.ArmLockout:
                armLockoutInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                armLockoutInst = RuntimeManager.CreateInstance(ArmLockout);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(armLockoutInst, GetComponent<Transform>());
                armLockoutInst.start();
                break;
            case LeggySFX.ClawOpen:
                clawOpenInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED 
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                clawOpenInst = RuntimeManager.CreateInstance(ClawOpen);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(clawOpenInst, GetComponent<Transform>());
                clawOpenInst.start();
                break;
            case LeggySFX.ClawLockout:
                clawLockoutInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED 
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                clawLockoutInst = RuntimeManager.CreateInstance(ClawLockout);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(clawLockoutInst, GetComponent<Transform>());
                clawLockoutInst.start();
                break;
            case LeggySFX.Gantry:
                gantryInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED 
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                gantryInst = RuntimeManager.CreateInstance(Gantry);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(gantryInst, GetComponent<Transform>());
                gantryInst.start();
                break;
            case LeggySFX.GantryLockout:
                gantryLockoutInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED 
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                gantryLockoutInst = RuntimeManager.CreateInstance(GantryLockout);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(gantryLockoutInst, GetComponent<Transform>());
                gantryLockoutInst.start();
                break;
            case LeggySFX.Rotation:
                rotationInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED 
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                rotationInst = RuntimeManager.CreateInstance(Rotation);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(rotationInst, GetComponent<Transform>());
                rotationInst.start();
                break;
            case LeggySFX.RotationLockout:
                rotationLockoutInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED 
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                rotationLockoutInst = RuntimeManager.CreateInstance(RotationLockout);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(rotationLockoutInst, GetComponent<Transform>());
                rotationLockoutInst.start();
                break;
            case LeggySFX.WristMovement:
                wristMovementInst.getPlaybackState(out currPlaybackState);
                if (currPlaybackState != PLAYBACK_STATE.STOPPED 
                 && currPlaybackState != PLAYBACK_STATE.STOPPING) { break; }
                wristMovementInst = RuntimeManager.CreateInstance(WristMovement);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(wristMovementInst, GetComponent<Transform>());
                wristMovementInst.start();
                break;
            case LeggySFX.Grab:
                break;
            default:
                break;
        }
    }

    public void LeggyGrabSFX()
    {
        RuntimeManager.PlayOneShot(Grab);
    }

    public void StopSound(LeggySFX stopSFX)
    {
        switch (stopSFX)
        {
            case LeggySFX.none:
                break;
            case LeggySFX.ArmDepth:
                if (armDepthInst.isValid())
                {
                    armDepthInst.stop(STOP_MODE.ALLOWFADEOUT);
                    armDepthInst.release();
                }
                break;
            case LeggySFX.ArmHeight:
                if (armHeightInst.isValid())
                {
                    armHeightInst.stop(STOP_MODE.ALLOWFADEOUT);
                    armHeightInst.release();
                }
                break;
            case LeggySFX.ArmLockout:
                if (armLockoutInst.isValid())
                {
                    armLockoutInst.stop(STOP_MODE.ALLOWFADEOUT);
                    armLockoutInst.release();
                }
                break;
            case LeggySFX.ClawOpen:
                if (clawOpenInst.isValid())
                {
                    clawOpenInst.stop(STOP_MODE.ALLOWFADEOUT);
                    clawOpenInst.release();
                }
                break;
            case LeggySFX.ClawLockout:
                if (clawLockoutInst.isValid())
                {
                    clawLockoutInst.stop(STOP_MODE.ALLOWFADEOUT);
                    clawLockoutInst.release();
                }
                break;
            case LeggySFX.Gantry:
                if (gantryInst.isValid())
                {
                    gantryInst.stop(STOP_MODE.ALLOWFADEOUT);
                    gantryInst.release();
                }
                break;
            case LeggySFX.GantryLockout:
                if (gantryLockoutInst.isValid())
                {
                    gantryLockoutInst.stop(STOP_MODE.ALLOWFADEOUT);
                    gantryLockoutInst.release();
                }
                break;
            case LeggySFX.Rotation:
                if (rotationInst.isValid())
                {
                    rotationInst.stop(STOP_MODE.ALLOWFADEOUT);
                    rotationInst.release();
                }
                break;
            case LeggySFX.RotationLockout:
                if (rotationLockoutInst.isValid())
                {
                    rotationLockoutInst.stop(STOP_MODE.ALLOWFADEOUT);
                    rotationLockoutInst.release();
                }
                break;
            case LeggySFX.WristMovement:
                if (wristMovementInst.isValid())
                {
                    wristMovementInst.stop(STOP_MODE.ALLOWFADEOUT);
                    wristMovementInst.release();
                }
                break;
            case LeggySFX.Grab:
                break;
            default:
                break;
        }
    }
}
