using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    // Private
    // Events
        // Instances
    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.Studio.EventInstance sfxInstance;
        // Parameters
    private FMOD.Studio.PARAMETER_ID happinessParam;
    private FMOD.Studio.PARAMETER_ID timeInLevel;
    private FMOD.Studio.PARAMETER_ID gameState;
    
    private bool areBanksLoaded = false;

    // Serialized
    [SerializeField] float bankLoadRecheckDelay = 0.1f;
    [SerializeField] EventReference mainTheme;
    [SerializeField] EventReference uiBack;
    [SerializeField] EventReference uiMove;
    [SerializeField] EventReference uiSelect;
    [SerializeField] EventReference gainHappiness;
    [SerializeField] EventReference loseHappiness;


    // Public
    public enum SFX
    {
        none,
        UI_Back,
        UI_Move,
        UI_Select,
        HappinessGain,
        HappinessLoss
    }

    // Singleton
    public static AudioHandler _AudioHandlerInstance { get; private set; }

    private void Awake()
    {
        if (_AudioHandlerInstance == null)
        {
            _AudioHandlerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        // Init
        Debug.Log("Loading banks.");
        RuntimeManager.LoadBank("Master");
        RuntimeManager.LoadBank("Master.strings");
        StartCoroutine(WaitWhileBanksLoad(bankLoadRecheckDelay));
    }

    private IEnumerator WaitWhileBanksLoad(float recheckDelay)
    {
        while (!RuntimeManager.HasBankLoaded("Master") || !RuntimeManager.HasBankLoaded("Master.strings"))
        {
            Debug.Log("Waiting for banks to load.");
            yield return new WaitForSeconds(recheckDelay);
        }
        areBanksLoaded = true;
        AfterBankLoad();
    }

    private void AfterBankLoad()
    {
        Debug.Log("Banks were loaded successfully.");
/*
        mainTheme = new EventReference();
        uiBack = new EventReference();
        uiMove = new EventReference();
        uiSelect = new EventReference();
        // Music & UI
        mainTheme = EventReference.Find("event:/Music/PlayMusic");
        // System.Guid mainThemeGUID = new System.Guid("cc98e317-026f-42a6-870d-a45dd3f3d19b");
        Debug.Log("GUID: mainTheme.Guid: " + mainTheme.Guid);

        uiBack.Path = "event:/SFX/UI/UI_Back";
        // System.Guid uiBackGUID = new System.Guid("8c11eae2-838c-453f-9d40-27b3cf621e68");
        Debug.Log("GUID: uiBack.Guid: " + uiBack.Guid);

        uiMove.Path = "event:/SFX/UI/UI_Move";
        // System.Guid uiMoveGUID = new System.Guid("a374db4e-69fa-4811-a2ae-bb504b86e5be");
        Debug.Log("GUID: uiMove.Guid: " + uiMove.Guid);

        uiSelect.Path = "event:/SFX/UI/UI_Select";
        System.Guid uiSelectGUID = new System.Guid("20746b7a-c864-4f86-bfa2-07e240796a87");
        uiSelect.Guid = new FMOD.GUID(uiSelectGUID);
*/
    }
/*
    public void PlayUISound(FMODUnity.EventReference soundEvent)
    {

    }
*/
    public void PlayMusic()
    {
        if (!areBanksLoaded) { return; }
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(mainTheme);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicInstance, this.transform);
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
        if (!areBanksLoaded) { return; }
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    public void UpdateMainTheme(float happiness, float timeSpent)
    {
        if (!areBanksLoaded) { return; }
        if (musicInstance.isValid())
        {
            RuntimeManager.StudioSystem.setParameterByID(happinessParam, happiness);
            RuntimeManager.StudioSystem.setParameterByID(timeInLevel, timeSpent);
        }
    }

    public void SetPauseMusic(bool isPaused)
    {
        if (!areBanksLoaded) { return; }
        if (musicInstance.isValid())
        {
            if (isPaused)
            {
                RuntimeManager.StudioSystem.setParameterByIDWithLabel(gameState, "Pause Menu");
            }
            else
            {
                RuntimeManager.StudioSystem.setParameterByIDWithLabel(gameState, "In Game");
            }
        }
    }

    public void PlaySFX(SFX selectedSound)
    {
        if (!areBanksLoaded) { return; }
        switch (selectedSound)
        {
            case SFX.none:
                break;
            case SFX.UI_Back:
                UnityEngine.Debug.Log("Playing UI Back sound.");
                RuntimeManager.PlayOneShot(uiBack);
                // sfxInstance = RuntimeManager.CreateInstance(uiBack);
                // sfxInstance.start();
                break;
            case SFX.UI_Move:
                UnityEngine.Debug.Log("Playing UI Move sound.");
                RuntimeManager.PlayOneShot(uiMove);
                // sfxInstance = RuntimeManager.CreateInstance(uiMove);
                // sfxInstance.start();
                break;
            case SFX.UI_Select:
                UnityEngine.Debug.Log("Playing UI Select sound.");
                RuntimeManager.PlayOneShot(uiSelect);
                // sfxInstance = RuntimeManager.CreateInstance(uiSelect);
                // sfxInstance.start();
                break;
            case SFX.HappinessGain:
                RuntimeManager.PlayOneShot(gainHappiness);
                break;
            case SFX.HappinessLoss:
                RuntimeManager.PlayOneShot(loseHappiness);
                break;
            default:
                break;
        }
        sfxInstance.release();
    }

    public void StopAudio()
    {
        FMOD.Studio.Bus mainBus = FMODUnity.RuntimeManager.GetBus("bus:/");
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
