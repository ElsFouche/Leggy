using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        eventSystem = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            SetFirstSelectedMainMenu();
    }

    
    
    public void SetFirstSelectedMainMenu()
    {
        EventSystem.current.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
}
