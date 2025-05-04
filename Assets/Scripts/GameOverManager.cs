using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// This is the graphical game over and menu handler.
/// Modified by Els 04/25/2025
/// </summary>

public class GameOverManager : MonoBehaviour
{
    // Private
    private float test = 0.0f;
    private EventSystem eventSystem;
    private CanvasGroup gameOverUICG;
    private GameManager gameManager;
    // private bool fullyDepressed;
    
    // Serialized
    
    // Public
    // public HappinessManager happinessManager;
    public GameObject fadeBlackTop;
    public GameObject fadeBlackBottom;
    public GameObject vignette;
    public GameObject gameOverMenu;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameOverUICG = gameOverMenu.GetComponent<CanvasGroup>();
        // happinessManager = FindObjectOfType<HappinessManager>();
        eventSystem = FindObjectOfType<EventSystem>();
        // test = 0f;
        if (eventSystem == null) { Debug.Log("No event system present in scene."); Destroy(gameObject); }

        if (fadeBlackTop == null) { yield break; }
        fadeBlackTop.GetComponent<Image>().fillAmount = 0;
        if (fadeBlackBottom == null) { yield break; }
        fadeBlackBottom.GetComponent<Image>().fillAmount = 0;
        if (vignette == null) { yield break; }
        vignette.GetComponent<Image>().color = new Color(1, 0, 0, 0);
        if (gameOverMenu == null) { yield break; }
        if (gameOverUICG == null) { yield break; }
        gameOverUICG.interactable = false;
        gameOverUICG.blocksRaycasts = false;
        gameOverMenu.SetActive(false);

        // eventSystem.GetComponent<EventSystem>().enabled = false;
        // eventSystem.GetComponent<StandaloneInputModule>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
/*
        if (happinessManager.happinessCount <= 0 && !fullyDepressed)
        {
            happinessManager.isVisible = false;
            fullyDepressed = true;
            StartCoroutine(gameOverTransition());
        }
*/
    }
    public void GameOver()
    {
        StartCoroutine(gameOverTransition());
    }

    private IEnumerator gameOverTransition()
    {
        Debug.Log("Emergency Shutdown");
        while (!Mathf.Approximately(fadeBlackTop.GetComponent<Image>().fillAmount, 1f))
        {
            fadeBlackTop.GetComponent<Image>().fillAmount = Mathf.SmoothDamp(fadeBlackTop.GetComponent<Image>().fillAmount,
                1, ref test, 0.03f);
            fadeBlackBottom.GetComponent<Image>().fillAmount = Mathf.SmoothDamp(fadeBlackBottom.GetComponent<Image>().fillAmount,
                1, ref test, 0.03f);
            yield return new WaitForSeconds(0.03f);
        }

        Debug.Log("I'm seeing red");
        while (!Mathf.Approximately(vignette.GetComponent<Image>().color.a, 0.5f))
        {
            vignette.GetComponent<Image>().color = new Color(1, 0, 0, Mathf.SmoothDamp(vignette.GetComponent<Image>().color.a,
                0.5f, ref test, 0.03f));
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.25f);
        Debug.Log("Retry?");
        EnableGameOverMenu();
        // eventSystem.GetComponent<EventSystem>().enabled = true;
        // eventSystem.GetComponent<StandaloneInputModule>().enabled = true;
        yield return null;
    }

    private void EnableGameOverMenu()
    {
        gameManager.ToggleControlMode(true);
        gameOverMenu.SetActive(true);
        gameOverUICG.interactable = true;
        gameOverUICG.blocksRaycasts = true;
        EventSystem.current.SetSelectedGameObject(gameOverMenu.GetComponentInChildren<Button>().gameObject);
    }

/*
    public void retryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void endTask()
    {
        SceneManager.LoadScene(0);
    }
*/
}
