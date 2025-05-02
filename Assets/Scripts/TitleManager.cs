using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    public GameObject transitionManager;
    [SerializeField] float afterStartDelay = 0.2f;
    private AudioHandler audioHandler;
    private ClawControls controls;
    private const int MAXNUMREATTEMPTS = 5;
    private int numAttempts = 0;

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
        StartCoroutine(AfterStart(afterStartDelay));
    }

    public void startGame()
    {
        StartCoroutine(transitionManager.GetComponent<TransitionManager>().TransitionToScene(1));
    }

    private IEnumerator AfterStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioHandler.BanksLoaded())
        {
            audioHandler.PlayMusic();
            audioHandler.UpdateMainTheme(0.0f, 0.0f);
            audioHandler.PlayMusic();
        } else
        {
            ReattemptAfter(0.5f);
        }
    }

    private IEnumerator ReattemptAfter(float seconds, int numTries = MAXNUMREATTEMPTS)
    {
        yield return new WaitForSeconds(seconds);
        numAttempts++;
        // Reattempt logic
        if (audioHandler.BanksLoaded())
        {
            audioHandler.PlayMusic();
            audioHandler.UpdateMainTheme(0.0f, 0.0f);
            audioHandler.PlayMusic();
        }
        else if (numAttempts <= MAXNUMREATTEMPTS)
        {
            StartCoroutine(ReattemptAfter(seconds));
        }
        else if (numAttempts > MAXNUMREATTEMPTS)
        {
            numAttempts = 0;
        }
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

        if (context.started || 
            (Mathf.Abs(context.ReadValue<Vector2>().y) > 0.3f && context.action.WasPressedThisFrame()))
        {
            audioHandler.PlaySFX(sfxName);
        }
    }
}