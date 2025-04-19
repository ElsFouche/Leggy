using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is the primary interface between designer and programmer.
/// It contains and provides access to several subsystems.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Design: Each level designer will add a game manager prefab to their level to access the UI.
    /// The first created game manager will persist into a new scene. It will contribute the current
    /// happiness value to the newly created game manager, overwriting it, then delete itself. 
    /// </summary>

    // Private
    private HappinessManager happinessManager;
    private HappinessManager previousManager;
    private TransitionManager transitionManager;
    private ObjectiveTracker objectiveTracker;
    private int currHappiness = 30000;
        // Pause Function
    private bool paused;
    private GameObject mainUIHolder;
    private GameObject pauseMenuHolder;
        // Text
    public enum Speaker 
    {
        None,
        Father,
        Child,
        Lady,
        System
    }
    private FontRandomizer fontRandomizer;

    // Public
        // Happiness 
    [Header("Happines Decay")]
    [Tooltip("Lower values should be used for levels that are expected to take a long time to complete.")]
    [Range(50,500)]
    public int maxHappinessLostPerTick = 50;
    [Tooltip("The amount of time in seconds between each happiness loss tick.")]
    [Range(1,10)]
    public float happinessLossTickSpeed;
    [Tooltip("The amount of time in seconds before the maximum amount of happiness lost per tick is reached.")]
    [Range(10,120)]
    public int timeUntilMaxHappinessLoss = 10;
    [Tooltip("The amount of time in seconds before Happiness begins decreasing.")]
    [Range(0, 30)]
    public int gracePeriod;
        // Text
    [Header("Level Transition Settings")]
    [Tooltip("The amount of time in seconds before the black screen begins fading in.")]
    public float textFadeInDelay;
    [Tooltip("The amount of time in seconds before the black screen begins fading out.")]
    public float textFadeOutDelay;
    public float fadeToBlackTime;
    public float textFadeInTime;
    public float textFadeOutTime;
    public float levelTransitionDelay;
    public int nextLevelIndex;
    public Speaker speakerFont;
    public string transitionText;

    private void Awake()
    {
        Debug.Log("Game Manager created with ID: " +  transform.root.GetInstanceID());
        // Persist into new scenes.
        DontDestroyOnLoad(this);

        
        // Check for pre-existing happiness manager. Update our current happiness with its value.
        previousManager = FindObjectOfType<HappinessManager>(); 
        if (previousManager != null) { Debug.Log("Happiness manager found with ID: " +  previousManager.GetInstanceID()); }
        if (previousManager != null && previousManager.transform.root.gameObject != this.transform.root.gameObject) 
        {
            Debug.Log("Found previous happiness manager with ID: " +  previousManager.GetInstanceID());
            Debug.Log("Updating local variables with existing values.");
            currHappiness = previousManager.happinessCount;
            Debug.Log("Deconstructing old game manager.");  
            Destroy(previousManager.transform.root.gameObject);
            previousManager = null;
        }
        
        // Store reference to components
        objectiveTracker = GetComponent<ObjectiveTracker>();
            Debug.Log("Objective Manager created with ID: " + objectiveTracker.GetInstanceID());
        happinessManager = GetComponentInChildren<HappinessManager>();
            Debug.Log("Happiness Manager created with ID: " + happinessManager.GetInstanceID());
        transitionManager = GetComponentInChildren<TransitionManager>();
            Debug.Log("Transition Manager created with ID: " + transitionManager.GetInstanceID());
        fontRandomizer = GetComponentInChildren<FontRandomizer>();
            Debug.Log("Found Font Randomizer with ID: " + fontRandomizer.GetInstanceID());

        // Initialize UI 
        pauseMenuHolder = GameObject.FindGameObjectWithTag("Subsystem_PauseUI");
            Debug.Log("Found Pause Menu with ID: " + pauseMenuHolder.GetInstanceID());
        mainUIHolder = GameObject.FindGameObjectWithTag("Subsystem_MainUI");
            Debug.Log("Found Main UI with ID: " +  mainUIHolder.GetInstanceID());
    }

    void Start()
    {
        pauseMenuHolder.SetActive(false);
        // Initialize level variables if unmodified by Level Designers
        if (happinessLossTickSpeed <= 0) happinessLossTickSpeed = 1;
        if (maxHappinessLostPerTick < 50) maxHappinessLostPerTick = 50;
        if (timeUntilMaxHappinessLoss < 10) timeUntilMaxHappinessLoss = 10;

        // Initialize Happiness Subsystem
        happinessManager.happinessCount = currHappiness;
        happinessManager.maxDepressor = maxHappinessLostPerTick;
        happinessManager.timeBetweenHappinessLoss = happinessLossTickSpeed;
        happinessManager.sigmoidFunction = new SigmoidFunction(gracePeriod, timeUntilMaxHappinessLoss);

        // Initialize transition text value
        transitionManager.loreText.text = transitionText;
        transitionManager.loreText.text.Replace("\\n", "\n");
            Debug.Log("Lore text set to " + "\n\'" + transitionText + "\'");
        switch (speakerFont) 
        {
            case Speaker.None:
                break;
            case Speaker.Father:
                // Default Father Font
                var font = Resources.Load("Fonts & Materials/mansalva-latin-400-normal");
                transitionManager.loreText.font = font as TMP_FontAsset;
                break;
            case Speaker.Child:
                // Initialize Font Randomizer Script
                fontRandomizer.textObject = transitionManager.loreText as TextMeshProUGUI;
                if (fontRandomizer.fonts.Count == 0) 
                {   
                    // Default Child Fonts
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/caveat-latin-500-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/coming-soon-latin-400-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/schoolbell-latin-400-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/schoolbell-latin-400-normal") as Font);
                }
                // Randomize Text
                fontRandomizer.RandomizeText(transitionText);
                break;
            case Speaker.Lady:
                // Default Lady Font
                font = Resources.Load("Fonts & Materials/nothing-you-could-do-latin-400-normal SDF");
                transitionManager.loreText.font = font as TMP_FontAsset;
                break;
            case Speaker.System:
                // Default System Font
                font = Resources.Load("Fonts & Materials/kode-mono-latin-400-normal");
                transitionManager.loreText.font = font as TMP_FontAsset;
                break;
            default:
                break;  
        }
        transitionManager.fadeInDelay = textFadeInDelay;
        transitionManager.fadeOutDelay = textFadeOutDelay;
        transitionManager.sceneSwitchDelay = levelTransitionDelay;
        transitionManager.blackFadeInTime = fadeToBlackTime;
        transitionManager.textFadeInTime = this.textFadeInTime;
        transitionManager.textFadeOutTime = this.textFadeOutTime;
    }

    void Update()
    {
        // Replace this with new input system
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            TogglePause();
        }
    }

    // Pause functionality 
    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pauseMenuHolder.SetActive(paused);
        mainUIHolder.SetActive(!paused);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartTask()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FinishLevel()
    {
        if (objectiveTracker.CountCompletedGoals() < objectiveTracker.minNumGoalsCompleted)
        {
            Debug.Log("Losing happiness due to early level exit.");
            happinessManager.loseHappiness(objectiveTracker.earlyExitHappinessLoss);
        }

        StartCoroutine(transitionManager.TransitionToScene(nextLevelIndex));
    }
}