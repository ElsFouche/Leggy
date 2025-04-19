using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public GameObject transitionManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        StartCoroutine(transitionManager.GetComponent<TransitionManager>().TransitionToScene(1));
    }

    public void quitGame()
    {
        Debug.Log("Powering down...");
        Application.Quit();
    }
}
