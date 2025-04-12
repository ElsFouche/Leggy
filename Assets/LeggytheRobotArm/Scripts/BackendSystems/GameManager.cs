using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is persistant across the game and tracks data
/// relevant to the player. It is a data-only singleton class.
/// It manages:
/// Pausing
/// Player Happiness - HappinessManager.cs
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    // Private

    // Must persist
    private HappinessManager HappinessManager { get; set; }
    private SigmoidFunction _sigmoidFunction;

    // Public
    public int MaxHappinessLoss = 500;

    // Singleton-style duplication checking for parent game object.
    private void Awake()
    {
        HappinessManager tempHManager = FindObjectOfType<HappinessManager>();
        // Destroy any Happiness Manager object that's created after the first one. 
        if (tempHManager != null && tempHManager.transform.root.gameObject != this.transform.root.gameObject) 
        {
            Debug.Log("Updating local variables with existing values.");
            MaxHappinessLoss = tempHManager.maxDepressor;
            Debug.Log("New HM instance self-destructing.");  
            Destroy(this); 
            return; 
        }
        
        Debug.Log("Game Manager created with ID: " +  transform.root.GetInstanceID());
        HappinessManager = transform.root.gameObject.GetComponentInChildren<HappinessManager>();
        Debug.Log("Happiness Manager created with ID: " + HappinessManager.GetInstanceID());
    }

 
    private bool paused;

    private GameObject mainGameHolder;
    private GameObject pauseMenuHolder;
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
        //if (paused) : Time.timeScale = 1 : 
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
