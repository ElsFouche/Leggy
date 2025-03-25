using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.P))
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
