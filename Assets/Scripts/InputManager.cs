using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private ClawControls controls;
    private TransitionManager transitionManager;
    private AudioHandler audioHandler;

    private void Awake()
    {
        controls = new ClawControls();
        controls.UI.Cancel.performed += ctx => ReturnToTitle(ctx);
        controls.UI.Cancel.started += ctx => ReturnToTitle(ctx);
        controls.UI.Cancel.canceled += ctx => ReturnToTitle(ctx);

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
        controls.UI.Enable();
    }
    private void OnDisable()
    {
        controls.UI.Disable();
    }

    void Start()
    {
        Cursor.visible = false;
        transitionManager = FindObjectOfType<TransitionManager>();
        audioHandler = AudioHandler._AudioHandlerInstance;
        StartCoroutine(AfterStart(0.2f));
    }

    private IEnumerator AfterStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioHandler.PlayMusic();
    }

    private void ReturnToTitle(InputAction.CallbackContext context)
    {
        if (context.started && transitionManager != null)
        {
            if (transitionManager.loreText != null)
            {
                transitionManager.loreText.SetText("");
            }
            transitionManager.fadeToBlackTime = 1.0f;
            transitionManager.displayTextFor = 0.0f;
            transitionManager.switchSceneAfter = 0.0f;
            transitionManager.textFadeIn = 0.0f;
            transitionManager.textFadeOut = 0.0f;
            transitionManager.TransitionToSceneWrapper(0);
        } else if (context.started && transitionManager == null)
        {
            SceneManager.LoadScene(0);
        }
    }
    
    public void SetFirstSelectedMainMenu()
    {
        //EventSystem.current.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
    private void PlayUISFX(InputAction.CallbackContext context, AudioHandler.SFX sfxName)
    {
        if (audioHandler == null) { return; }

        if (context.action.WasPressedThisFrame())
        {
            audioHandler.PlaySFX(sfxName);
        }
    }
}
