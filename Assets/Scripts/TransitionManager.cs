using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class TransitionManager : MonoBehaviour
{
    public float fadeInDelay;
    public float fadeOutDelay;
    public float sceneSwitchDelay;
    public float blackFadeInTime = 1.0f;
    public float textFadeInTime = 1.0f;
    public float textFadeOutTime = 1.0f;
    
    public TMP_Text loreText;
    public Image blackScreen;
    public GameObject happinessManager;
    public bool isTransitioning;
    public int sceneToChangeTo;

    bool loreFadedIn;

    // Start is called before the first frame update
    void Start()
    {
        loreFadedIn = false;
        isTransitioning = false;

        if (blackScreen != null)
        {
            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            loreText.color = new Color(1, 1, 1, 0);
        }

        // Els: Modified 04/19/2025
        // Unnecessary check - title screen & level select won't have a
        // game manager and thus no transition manager.
        /*
                if (SceneManager.GetActiveScene().buildIndex > 1)
                {
                    StartCoroutine(blackFadeOut());
                    happinessManager = GameObject.FindGameObjectWithTag("HappyManager");
                }
                else
                {
                    if (blackScreen != null)
                    {
                        blackScreen.gameObject.SetActive(false);
                        loreText.gameObject.SetActive(false);
                    }
                }
        */

        if (blackScreen != null) 
        { 
            blackScreen.gameObject.SetActive(false);
        } else {
            Debug.Log("Black screen object not found.");
        }

        if (loreText != null) 
        { 
            loreText.gameObject.SetActive(false); 
        } else { 
            Debug.Log("Lore text object not found."); 
        }
    }

    // Els: Modified 04/19/2025
    /// <summary>
    /// Begins the fade to black animation.
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <returns></returns>
    public IEnumerator FadeToBlack(float screenFadeTime = 1.0f)
    {
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
        } else {
            Debug.Log("Black screen object not found.");
            // Kill coroutine
            yield break;
        }
/*        
        // Fade the black screen in. 
        while (blackScreen.GetComponent<Image>().color.a < 1.0f)
        {   
            blackScreen.GetComponent<Image>().color = new Color
                (0, 0, 0, blackScreen.GetComponent<Image>().color.a + Time.deltaTime);
            
            // Magic number should be changed. 
            yield return new WaitForSeconds(0.01f);
        }
*/
        float tempFadeTime = 0.0f;

        while (tempFadeTime < screenFadeTime)
        {
            tempFadeTime += Time.deltaTime;
            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, tempFadeTime / screenFadeTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Begin text fade only after black screen is fully opaque. 
        // StartCoroutine(fadeText(fadeInDelay, fadeOutDelay, sceneSwitchDelay, sceneIndex));
    }

    // Els: I don't know that this is actually called from anywhere. 
    /// <summary>
    /// Fade out the black screen
    /// </summary>
    /// <returns></returns>
/*
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
*/


    // Els: This seems to both fade the text in and out in a single function.
    // Els: Begin break into two functions.
    // Handle the text fade animation
/*
    public IEnumerator fadeText(float fadeInDelay, float fadeOutDelay, float sceneSwitchDelay, int sceneIndex)
    {
        yield return new WaitForSeconds(fadeInDelay);

        if (loreText != null)
        {
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
        }
        SceneManager.LoadScene(sceneIndex);  // Switch to the desired scene
    }
*/

    public IEnumerator FadeInText (float fadeDelay = 0.0f, float fadeTime = 1.0f) 
    {
        // Kill coroutine if lore text is empty.
        if (loreText == null) { Debug.Log("Lore text not found.");  yield break; }
        loreText.gameObject.SetActive(true);

        float tempFadeInTime = 0.0f;

        while (tempFadeInTime < fadeTime) 
        {
            tempFadeInTime += Time.deltaTime;
            loreText.color = new Color(loreText.color.r, loreText.color.g, loreText.color.b, tempFadeInTime / fadeTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        loreFadedIn = true;
    }

    
    public IEnumerator FadeOutText(float fadeDelay = 0.0f, float fadeTime = 1.0f) 
    {
        // Kill coroutine if lore text is empty.
        if (loreText == null) { Debug.Log("Lore text not found."); yield break; }
        loreText.gameObject.SetActive(true);

        float tempFadeOutTime = fadeTime;

        yield return new WaitForSeconds(fadeDelay);
        
        while (tempFadeOutTime > 0) 
        {
            tempFadeOutTime -= Time.deltaTime;
            loreText.color = new Color(loreText.color.r, loreText.color.g, loreText.color.b, tempFadeOutTime / fadeTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        loreFadedIn = false;
    }

    /// <summary>
    /// Els: Do not call this function. 
    /// </summary>
    public void PressToStart()
    {
        isTransitioning = true;
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            happinessManager.GetComponent<HappinessManager>().isVisible = false;
        }
        // StartCoroutine(fadeToBlack(0));
    }

    // General method to start the scene transition
    public IEnumerator TransitionToScene(int sceneIndex)
    {
        isTransitioning = true;
        // StartCoroutine(fadeToBlack(sceneIndex));
            // Debug.Log("Beginning fade to black.");
        yield return FadeToBlack(blackFadeInTime);
            // Debug.Log("Beginning text fade in.");
        yield return FadeInText(textFadeInTime, textFadeInTime);
            // Debug.Log("Beginning text fade out.");
        yield return FadeOutText(textFadeOutTime, textFadeOutTime);
            // Debug.Log("Loading next level at index: " +  sceneIndex);
        yield return new WaitForSeconds(sceneSwitchDelay);
        SceneManager.LoadScene(sceneIndex);
    }

    // This is the wrapper method to make it work with Unity's UI Button OnClick()
    public void TransitionToSceneWrapper(int sceneIndex)
    {
        StartCoroutine(TransitionToScene(sceneIndex));
    }
}
