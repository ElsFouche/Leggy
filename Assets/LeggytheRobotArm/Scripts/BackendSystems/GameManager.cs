using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is persistant across the game and tracks data
/// relevant to the player. It is a data-only singleton class.
/// It manages:
/// Pausing - OFFLOAD TO A DIFFERENT SCRIPT
/// Player Happiness - HappinessManager.cs
/// 
/// </summary>

public class GameManager
{
    private static GameManager instance;

    // Construction Script
    private GameManager() 
    {
        Debug.Log("GameManager singleton has been instantiated.");
    }
    public static GameManager getInstance()
    {
        if (instance == null) instance = new GameManager();
        return instance;
    }

    /* Pause Menu Functionality to be offloaded to new script
    
    public bool paused;

    public GameObject mainGameHolder;
    public GameObject pauseMenuHolder;
    void Start()
    {
        // pauseMenuHolder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            togglePause();
        }

        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void togglePause()
    {
        paused = !paused;
        pauseMenuHolder.SetActive(paused);
        mainGameHolder.SetActive(!paused);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void restartTask()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    */
}
