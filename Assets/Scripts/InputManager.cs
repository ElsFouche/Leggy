using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    //public EventSystem eventSystem;
    public GameOverManager gameOverManager;

    Vector2 directionalInput;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        gameOverManager = FindObjectOfType<GameOverManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (EventSystem.current.currentSelectedGameObject == null ||
            (EventSystem.current.currentSelectedGameObject == null && gameOverManager.gameOverMenu.activeInHierarchy &&
            SceneManager.GetActiveScene().buildIndex > 1))
                SetFirstSelectedMainMenu();
        */

        if (Input.GetButtonDown("Cancel") &&
            (SceneManager.GetActiveScene().buildIndex == 1 ||SceneManager.GetActiveScene().buildIndex == 2))
        {
            SceneManager.LoadScene(0);
        }
    }

    
    
    public void SetFirstSelectedMainMenu()
    {
        //EventSystem.current.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
}
