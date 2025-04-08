using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public float textFadeInDelay;
    public float sceneSwitchDelay;
    public TMP_Text loreText;
    public Image blackScreen;
    public GameObject happinessManager;
    public bool isTransitioning;
    public int sceneToChangeTo;

    float fadeTime;
    bool loreFadedIn;

    public float fadeInTime;
    public float fadeOutTime;

    // Start is called before the first frame update
    void Start()
    {
        fadeTime = 1;
        loreFadedIn = false;
        isTransitioning = false;

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        loreText.color = new Color(1, 1, 1, 0);

        if (SceneManager.GetActiveScene().buildIndex > 0)
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
        float increaseValue = 0;
        if (blackScreen == null || loreText == null)
        {
            yield return null;
            SceneManager.LoadScene(sceneIndex);  // Switch to the desired scene
        }
        else
        {
            blackScreen.gameObject.SetActive(true);
            loreText.gameObject.SetActive(true);

            while (increaseValue < 1)
            {
                increaseValue += (Time.deltaTime / fadeInTime);
                blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, increaseValue);

                yield return new WaitForSeconds(0.01f);
            }
        }

        StartCoroutine(fadeInText(textFadeInDelay, sceneIndex));
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
    public IEnumerator fadeInText(float textFadeInDelay, int sceneIndex)
    {
        yield return new WaitForSeconds(textFadeInDelay);

        while (loreText.color.a < 1.0f)
        {
            if (!loreFadedIn)
            {
                loreText.color = new Color(1, 1, 1, loreText.color.a + Time.deltaTime);

                yield return new WaitForSeconds(0.01f);
            }
        }
        loreFadedIn = true;

        StartCoroutine(fadeOutText(textFadeInDelay, sceneIndex));
    }

    public IEnumerator fadeOutText(float sceneSwitchDelay, int sceneIndex)
    {
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
        if (!loreFadedIn) StartCoroutine(fadeToBlack(sceneIndex));
        else StartCoroutine(fadeOutText(sceneSwitchDelay, sceneIndex));
    }

    // This is the wrapper method to make it work with Unity's UI Button OnClick()
    public void TransitionToSceneWrapper(int sceneIndex)
    {
        TransitionToScene(sceneIndex);
    }
}
