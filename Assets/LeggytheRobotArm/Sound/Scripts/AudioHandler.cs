using FMODUnity;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    // Private
        // Events
    private EventReference mainTheme;
    private EventReference uiBack;
    private EventReference uiMove;
    private EventReference uiSelect;
        // Instances
    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.Studio.EventInstance sfxInstance;
        // Parameters
    private FMOD.Studio.PARAMETER_ID happinessParam;
    private FMOD.Studio.PARAMETER_ID timeInLevel;
    private FMOD.Studio.PARAMETER_ID gameState;

    // Public
    public enum SFX
    {
        none,
        UI_Back,
        UI_Move,
        UI_Select
    }

    private void Start()
    {
        // Init
        mainTheme = new EventReference();
        uiBack = new EventReference();
        uiMove = new EventReference();
        uiSelect = new EventReference();
        // Music & UI
        mainTheme.Path = "event:/Music/PlayMusic";
        System.Guid mainThemeGUID = new System.Guid("cc98e317-026f-42a6-870d-a45dd3f3d19b");
        mainTheme.Guid = new FMOD.GUID(mainThemeGUID);

        uiBack.Path = "event:/SFX/UI/UI_Back";
        System.Guid uiBackGUID = new System.Guid("8c11eae2-838c-453f-9d40-27b3cf621e68");
        uiBack.Guid = new FMOD.GUID(uiBackGUID);

        uiMove.Path = "event:/SFX/UI/UI_Move";
        System.Guid uiMoveGUID = new System.Guid("a374db4e-69fa-4811-a2ae-bb504b86e5be");
        uiMove.Guid = new FMOD.GUID(uiMoveGUID);


        uiSelect.Path = "event:/SFX/UI/UI_Select";
        System.Guid uiSelectGUID = new System.Guid("20746b7a-c864-4f86-bfa2-07e240796a87");
        uiSelect.Guid = new FMOD.GUID(uiSelectGUID);
    }

    public void PlayUISound(FMODUnity.EventReference soundEvent)
    {

    }

    public void PlayMusic()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(mainTheme);
        musicInstance.start();

        // Parameters
        FMOD.Studio.EventDescription musicEventDescription;
        musicInstance.getDescription(out musicEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION happinessParamDescription;
        musicEventDescription.getParameterDescriptionByName("Happiness", out happinessParamDescription);
        happinessParam = happinessParamDescription.id;

        FMOD.Studio.PARAMETER_DESCRIPTION timeInLevelParamDescription;
        musicEventDescription.getParameterDescriptionByName("TimeSpent", out timeInLevelParamDescription);
        timeInLevel = timeInLevelParamDescription.id;

        FMOD.Studio.PARAMETER_DESCRIPTION gameStateParamDescription;
        musicEventDescription.getParameterDescriptionByName("GameState", out gameStateParamDescription);
        gameState = gameStateParamDescription.id;
    }
    public void PlayMusic(FMODUnity.EventReference musicEvent)
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
        musicInstance.release();
    }

    public void UpdateMainTheme(float happiness, float timeSpent)
    {
        if (musicInstance.isValid())
        {
            musicInstance.setParameterByID(happinessParam, happiness);
            musicInstance.setParameterByID(timeInLevel, timeSpent);
        }
    }

    public void MenuMusicToggle(bool toggle)
    {
        if (musicInstance.isValid())
        {
            if (toggle)
            {
                musicInstance.setParameterByID(gameState, 1);
            }
            else
            {
                musicInstance.setParameterByID(gameState, 0);
            }
        }
    }

    public void PlaySFX(SFX selectedSound)
    {
        switch (selectedSound)
        {
            case SFX.none:
                break;
            case SFX.UI_Back:
                UnityEngine.Debug.Log("Playing UI Back sound.");
                // RuntimeManager.PlayOneShot(uiBack);
                sfxInstance = RuntimeManager.CreateInstance(uiBack);
                sfxInstance.start();
                break;
            case SFX.UI_Move:
                UnityEngine.Debug.Log("Playing UI Move sound.");
                // RuntimeManager.PlayOneShot(uiMove);
                sfxInstance = RuntimeManager.CreateInstance(uiMove);
                sfxInstance.start();
                break;
            case SFX.UI_Select:
                UnityEngine.Debug.Log("Playing UI Select sound.");
                RuntimeManager.PlayOneShot(uiSelect);
                break;
            default:
                break;
        }
        sfxInstance.release();
    }

    public void StopAudio()
    {
        FMOD.Studio.Bus mainBus = FMODUnity.RuntimeManager.GetBus("bus:/Master Bus");
        mainBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void StopAudio(FMOD.Studio.Bus bus)
    {
        bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnDisable()
    {
        StopAudio();
    }
    private void OnDestroy()
    {
        StopAudio();
    }
}
