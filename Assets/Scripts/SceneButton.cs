using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This script exposes functionality normally found in the transition
/// manager in a separate script for UI buttons. 
/// </summary>
public class SceneButton : MonoBehaviour
{
    private TransitionManager transitionManager;
    private FontRandomizer fontRandomizer;
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
    [Header("Scene Transition Settings")]
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
    [TextArea]
    public string transitionText;
    public Speaker font = Speaker.System;

    private void Start()
    {
        transitionManager = FindObjectOfType<TransitionManager>();
        fontRandomizer = transitionManager.GetComponent<FontRandomizer>();

        switchSceneAfter += textFadeIn + textFadeOut + displayTextFor + fadeToBlackTime;
        switch (font)
        {
            case Speaker.None:
                break;
            case Speaker.Father:
                // Default Father Font
                var font = Resources.Load("Fonts & Materials/mansalva-latin-400-normal");
                transitionManager.loreText.font = font as TMP_FontAsset;
                break;
            case Speaker.Child:
                // Initialize Font Randomizer Script
                fontRandomizer.textObject = transitionManager.loreText as TextMeshProUGUI;
                if (fontRandomizer.fonts.Count == 0)
                {
                    // Default Child Fonts
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/caveat-latin-500-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/coming-soon-latin-400-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/schoolbell-latin-400-normal") as Font);
                    fontRandomizer.fonts.Add(Resources.Load("Fonts/Child/schoolbell-latin-400-normal") as Font);
                }
                // Randomize Text
                fontRandomizer.RandomizeText(transitionText);
                break;
            case Speaker.Lady:
                // Default Lady Font
                font = Resources.Load("Fonts & Materials/nothing-you-could-do-latin-400-normal SDF");
                transitionManager.loreText.font = font as TMP_FontAsset;
                break;
            case Speaker.System:
                // Default System Font
                font = Resources.Load("Fonts & Materials/kode-mono-latin-400-normal");
                transitionManager.loreText.font = font as TMP_FontAsset;
                break;
            default:
                break;
        }
    }
    public void OnButtonPress()
    {
        // Get the TransitionManager and call the method with the scene index
        if (transitionManager != null)
        {
            if (transitionManager.loreText != null) 
            { 
                transitionManager.loreText.SetText(transitionText);
            }
            transitionManager.fadeToBlackTime = fadeToBlackTime;
            transitionManager.displayTextFor = displayTextFor;
            transitionManager.switchSceneAfter = switchSceneAfter;
            transitionManager.textFadeIn = textFadeIn;
            transitionManager.textFadeOut = textFadeOut;

            if (sceneIndex < 0)
            {
                transitionManager.TransitionToSceneWrapper(SceneManager.GetActiveScene().buildIndex + 1);
            } else
            {
                transitionManager.TransitionToSceneWrapper(sceneIndex);
            }
        }
    }
}
