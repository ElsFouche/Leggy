using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused;

    public GameObject mainGameHolder;
    public GameObject pauseMenuHolder;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuHolder.SetActive(false);
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
}
