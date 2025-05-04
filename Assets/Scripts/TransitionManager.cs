using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using System.Transactions;

public class TransitionManager : MonoBehaviour
{
    public enum Speaker
    {
        None,
        Father,
        Child,
        Lady,
        System
    }
    [Header("Next Scene")]
    [Tooltip("Leave as -1 to auto-advance to next scene index.")]
    public int sceneIndex = -1;  // Scene index to load when button is pressed
    [Header("Scene Start Transition Settings")]
    [Tooltip("How long it takes in seconds for the scene to fade in from black")]
    public float blackFadeOutTime = 0.0f;
    [Header("Scene End Transition Settings")]
    [Tooltip("How long in seconds it takes for the black screen to fade in.")]
    public float fadeToBlackTime = 1.0f;
    [Tooltip("How long it takes in seconds for the text to fade in.")]
    public float textFadeIn = 1.0f;
    [Tooltip("How long it takes in seconds for the text to fade out.")]
    public float textFadeOut = 0.5f;
    [Tooltip("Display transition text for this long in seconds before fading out.")]
    public float displayTextFor = 1.0f;
    [Tooltip("Additional delay before changing scene after text fades all the way out.")]
    public float switchSceneAfter = 0.0f;
    [Header("Transition Text Settings")]
    [Tooltip("Reference to the text display field.")]
    public TMP_Text loreText;
    [Tooltip("Reference to the black screen image overlay")]
    public Image blackScreen;
    [TextArea]
    [Tooltip("This text will be displayed if the player completes the level properly.")]
    public string transitionText;
    public Speaker font = Speaker.System;
    [TextArea]
    [Tooltip("This text will be displayed if the player exits early.")]
    public string endEarlyText;
    public Speaker earlyExitFont = Speaker.System;

    // Deprecated
    // public float fadeInDelay = 0.0f;
    // public float fadeOutDelay;
    // public float sceneSwitchDelay;
    // public float textFadeInTime = 1.0f;
    // public float textFadeOutTime = 1.0f;
    // public GameObject happinessManager;
    // public int sceneToChangeTo; 

    private bool isTransitioning;
    private bool loreFadedIn;
    private FontRandomizer fontRandomizer;

    void Start()
    {
        loreFadedIn = false;    // Seemingly unused
        isTransitioning = false; 
        fontRandomizer = GetComponentInChildren<FontRandomizer>();

        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(false);
            // blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            if (blackFadeOutTime > 0.0f) 
            {
                StartCoroutine(FadeInFromBlack(blackFadeOutTime)); 
            } else
            {
                blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
        }

        if (loreText != null) 
        { 
            loreText.gameObject.SetActive(false); 
            loreText.color = new Color(1, 1, 1, 0);
            loreText.SetText(transitionText);
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

    // Scene Start Transition Functions
    public IEnumerator FadeInFromBlack(float timeToFadeIn)
    {
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }
        else
        {
            Debug.Log("Black screen object not found.");
            // Kill coroutine
            yield break;
        }

        float tempFadeTime = timeToFadeIn;

        while (tempFadeTime > 0.0f)
        {
            tempFadeTime -= Time.deltaTime;
            blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, tempFadeTime / timeToFadeIn);
            yield return new WaitForSeconds(Time.deltaTime);
            Debug.Log("Fading in.");
        }
    }

    // Scene End Transition Functions
    public IEnumerator FadeInText (float fadeTime = 1.0f) 
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
/*
    public void PressToStart()
    {
        isTransitioning = true;
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            happinessManager.GetComponent<HappinessManager>().isVisible = false;
        }
        // StartCoroutine(fadeToBlack(0));
    }
*/
    
    // General method to start the scene transition
    public IEnumerator TransitionToScene(int sceneIndex)
    {
        if (isTransitioning)
        {
            yield break;
        }
        isTransitioning = true;
        // StartCoroutine(fadeToBlack(sceneIndex));
        // Debug.Log("Beginning fade to black.");
        yield return FadeToBlack(fadeToBlackTime);
            // Debug.Log("Beginning text fade in.");
        yield return FadeInText(textFadeIn);
            // Debug.Log("Beginning text fade out.");
        yield return FadeOutText(displayTextFor, textFadeOut);
            // Debug.Log("Loading next level at index: " +  sceneIndex);
        yield return new WaitForSeconds(switchSceneAfter);
        SceneManager.LoadScene(sceneIndex);
    }

    // This is the wrapper method to make it work with Unity's UI Button OnClick()
    public void TransitionToSceneWrapper(int sceneIndex)
    {
        StartCoroutine(TransitionToScene(sceneIndex));
    }

    public void SetTextFont(bool isEarlyExit = false)
    {
        Speaker tempFont;
        string tempText;
        if (isEarlyExit)
        {
            tempFont = earlyExitFont;
            tempText = endEarlyText;
        }
        else 
        { 
            tempFont = font;
            tempText = transitionText;
        }

        // Load Fonts
        switch (tempFont)
        {
            case Speaker.None:
                break;
            case Speaker.Father:
                // Default Father Font
                var font = Resources.Load("Fonts & Materials/mansalva-latin-400-normal");
                loreText.font = font as TMP_FontAsset;
                break;
            case Speaker.Child:
                // Initialize Font Randomizer Script
                fontRandomizer.textObject = loreText as TextMeshProUGUI;
                if (fontRandomizer.fonts.Count == 0)
                {
                    // Default Child Fonts
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/caveat-latin-500-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/coming-soon-latin-400-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/schoolbell-latin-400-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/schoolbell-latin-400-normal") as Font);
                }
                // Randomize Text
                fontRandomizer.RandomizeText(tempText);
                break;
            case Speaker.Lady:
                // Default Lady Font
                font = Resources.Load("Fonts & Materials/nothing-you-could-do-latin-400-normal SDF");
                loreText.font = font as TMP_FontAsset;
                break;
            case Speaker.System:
                // Default System Font
                font = Resources.Load("Fonts & Materials/kode-mono-latin-400-normal");
                loreText.font = font as TMP_FontAsset;
                break;
            default:
                break;
        }
    }
}
