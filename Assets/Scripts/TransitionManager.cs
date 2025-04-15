using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public float fadeInDelay;
    public float fadeOutDelay;
    public float sceneSwitchDelay;
    public TMP_Text loreText;
    public Image blackScreen;
    public GameObject happinessManager;
    public bool isTransitioning;
    public int sceneToChangeTo;

    float fadeTime;
    bool loreFadedIn;

    // Start is called before the first frame update
    void Start()
    {
        fadeTime = 1;
        loreFadedIn = false;
        isTransitioning = false;

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        loreText.color = new Color(1, 1, 1, 0);

        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            StartCoroutine(blackFadeOut());
            happinessManager = GameObject.FindGameObjectWithTag("HappyManager");
        }
        else
        {
            blackScreen.gameObject.SetActive(false);
            loreText.gameObject.SetActive(false);
        }
    }

    // Start the fade to black animation
    public IEnumerator fadeToBlack(int sceneIndex)
    {
        blackScreen.gameObject.SetActive(true);
        loreText.gameObject.SetActive(true);
        while (blackScreen.GetComponent<Image>().color.a < 1.0f)
        {
            blackScreen.GetComponent<Image>().color = new Color
                (0, 0, 0, blackScreen.GetComponent<Image>().color.a + Time.deltaTime);

            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(fadeText(fadeInDelay, fadeOutDelay, sceneSwitchDelay, sceneIndex));
    }

    // Fade out the black screen
    public IEnumerator blackFadeOut()
    {
        while (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, fadeTime);

            yield return new WaitForSeconds(0.01f);
        }
        happinessManager.GetComponent<HappinessManager>().isVisible = true;
    }

    // Handle the text fade animation
    public IEnumerator fadeText(float fadeInDelay, float fadeOutDelay, float sceneSwitchDelay, int sceneIndex)
    {
        yield return new WaitForSeconds(fadeInDelay);

        while (loreText.color.a < 1.0f)
        {
            if (!loreFadedIn)
            {
                loreText.color = new Color(1, 1, 1, loreText.color.a + Time.deltaTime);

                yield return new WaitForSeconds(0.01f);
            }
        }
        loreFadedIn = true;
        yield return new WaitForSeconds(fadeOutDelay);

        while (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            loreText.color = new Color(fadeTime, fadeTime, fadeTime, 1);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(sceneSwitchDelay);
        SceneManager.LoadScene(sceneIndex);  // Switch to the desired scene
    }

    public void PressToStart()
    {
        isTransitioning = true;
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            happinessManager.GetComponent<HappinessManager>().isVisible = false;
        }
        StartCoroutine(fadeToBlack(0));
    }

    // General method to start the scene transition
    public void TransitionToScene(int sceneIndex)
    {
        isTransitioning = true;
        StartCoroutine(fadeToBlack(sceneIndex));
    }

    // This is the wrapper method to make it work with Unity's UI Button OnClick()
    public void TransitionToSceneWrapper(int sceneIndex)
    {
        TransitionToScene(sceneIndex);
    }
}
