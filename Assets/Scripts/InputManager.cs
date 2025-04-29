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
    private ClawControls controls;

    Vector2 directionalInput;

    private void Awake()
    {
        controls = new ClawControls();
        controls.UI.Cancel.performed += ctx => ReturnToTitle(ctx);
    }

    private void OnEnable()
    {
        controls.UI.Enable();
    }
    private void OnDisable()
    {
        controls.UI.Disable();
    }

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
/*
        if (Input.GetButtonDown("Cancel") &&
            (SceneManager.GetActiveScene().buildIndex == 1 ||SceneManager.GetActiveScene().buildIndex == 2))
        {
            SceneManager.LoadScene(0);
        }
*/
    }

    private void ReturnToTitle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene(0);
        }
    }
    
    public void SetFirstSelectedMainMenu()
    {
        //EventSystem.current.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
}
