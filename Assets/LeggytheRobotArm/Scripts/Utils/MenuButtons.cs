using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Els Fouché, 04/22/2025
/// This script exposes functionality in the game manager via buttons. 
/// </summary>
public class MenuButtons : MonoBehaviour
{
    private float delayAfterStart = 0.3f;
    private GameManager gameManager;

    private void Start()
    {
        StartCoroutine(AfterStart());
    }

    private IEnumerator AfterStart()
    {
        yield return new WaitForSeconds(delayAfterStart);
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Pause or resume play.
    /// </summary>
    public void PauseResumeGame()
    {
        if (gameManager != null) { Debug.Log("Toggling pause."); gameManager.TogglePause(); }
    }

    /// <summary>
    /// Return to scene index 0. 
    /// </summary>
    public void ReturnToMainMenu()
    {
        if (gameManager != null) { Debug.Log("Returning to main menu."); gameManager.ReturnToMainMenu(); }
    }

    /// <summary>
    /// Restart the current level. 
    /// </summary>
    public void RestartTask()
    {
        if (gameManager != null) { Debug.Log("Restarting task."); gameManager.RestartTask(); }
    }

    /// <summary>
    /// Quit the application. 
    /// </summary>
    public void ExitGame()
    {
        if (gameManager != null) { Debug.Log("Exiting game."); gameManager.QuitGame(); }
    }
}
