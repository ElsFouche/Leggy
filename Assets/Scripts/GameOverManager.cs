using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverManager : MonoBehaviour
{
    bool fullyDepressed;
    float test;
    public HappinessManager happinessManager;

    public GameObject fadeBlackTop;
    public GameObject fadeBlackBottom;
    public GameObject vignette;
    public GameObject gameOverMenu;

    public EventSystem eventSystem;


    // Start is called before the first frame update
    void Start()
    {
        happinessManager = FindObjectOfType<HappinessManager>();
        eventSystem = FindObjectOfType<EventSystem>();
        test = 0f;

        fadeBlackTop.GetComponent<Image>().fillAmount = 0;
        fadeBlackBottom.GetComponent<Image>().fillAmount = 0;
        vignette.GetComponent<Image>().color = new Color(1, 0, 0, 0);
        gameOverMenu.SetActive(false);

        eventSystem.GetComponent<EventSystem>().enabled = false;
        eventSystem.GetComponent<StandaloneInputModule>().enabled = false;
    }

// Update is called once per frame
void Update()
    {
        if (happinessManager.happinessCount <= 0 && !fullyDepressed)
        {
            happinessManager.isVisible = false;
            fullyDepressed = true;
            StartCoroutine(gameOverTransition());
        }
    }

    public IEnumerator gameOverTransition()
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
        gameOverMenu.SetActive(true);
        eventSystem.GetComponent<EventSystem>().enabled = true;
        eventSystem.GetComponent<StandaloneInputModule>().enabled = true;
        yield return null;

    }

    public void retryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void endTask()
    {
        SceneManager.LoadScene(0);
    }
}
