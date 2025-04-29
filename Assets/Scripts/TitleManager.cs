using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    public GameObject transitionManager;
    
    private AudioHandler audioHandler;
    private ClawControls controls;

    private void Awake()
    {
        controls = new ClawControls();
        controls.UI.Navigate.performed += ctx => PlayUISFX(ctx, AudioHandler.SFX.UI_Move);
        controls.UI.Navigate.started += ctx => PlayUISFX(ctx, AudioHandler.SFX.UI_Move);
        controls.UI.Navigate.canceled += ctx => PlayUISFX(ctx, AudioHandler.SFX.UI_Move);
        controls.UI.Submit.started += ctx => PlayUISFX(ctx, AudioHandler.SFX.UI_Select);
        controls.UI.Submit.canceled += ctx => PlayUISFX(ctx, AudioHandler.SFX.UI_Select);
        controls.UI.Cancel.started += ctx => PlayUISFX(ctx, AudioHandler.SFX.UI_Back);
        controls.UI.Cancel.canceled += ctx => PlayUISFX(ctx, AudioHandler.SFX.UI_Back);
    }
    

    private void OnEnable()
    {
        controls.Player.Disable();
        controls.UI.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
        controls.UI.Disable();
    }
    private void OnDestroy()
    {
        controls.Disable();
    }
    void Start()
    {
        audioHandler = AudioHandler._AudioHandlerInstance;
        audioHandler.PlayMusic();
    }

    public void startGame()
    {
        StartCoroutine(transitionManager.GetComponent<TransitionManager>().TransitionToScene(1));
    }

    public void quitGame()
    {
        Debug.Log("Powering down...");
        audioHandler.StopAudio();
        Application.Quit();
    }

    private void PlayUISFX(InputAction.CallbackContext context, AudioHandler.SFX sfxName)
    {
        if (audioHandler == null) {  return; }

        if (context.started || Mathf.Abs(context.ReadValue<Vector2>().y) > 0.1f)
        {
            audioHandler.PlaySFX(sfxName);
        }
    }
}
