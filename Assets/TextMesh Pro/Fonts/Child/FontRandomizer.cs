using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Els Fouché - Font Randomizer, 02/16/2025
/// This script runs in the editor. Its purpose is to modify an input string
/// so that each character of that string uses a randomly chosen font.
/// Fonts must be assigned prior to use. 
/// This script requires Text Mesh Pro and enforces its inclusion via RequireComponent.
/// </summary>

[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]

public class FontRandomizer : MonoBehaviour
{
    [Header("Enter Text")]
    public string textToRandomize;
    [Header("Set of Fonts to Randomize From")]
    public List<Font> fonts;
    private TextMeshProUGUI textObject;
    
    /// <summary>
    /// Grab a reference to the TMP component handling the text on this game object. 
    /// Runs when the component is added for the first time or is reset.
    /// </summary>
    private void Reset()
    {
        if (textObject == null)
        {
            textObject = GetComponent<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Fallback assignment in cases of prefabs. If the component is already added
    /// such as with a prefab being created in the game world this will ensure that 
    /// textObject is correctly assigned prior to any other code. 
    /// </summary>
    private void Awake()
    {
        if (textObject == null)
        {
            textObject = GetComponent<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Updates onscreen display only when fields within the script are changed.
    /// </summary>
    private void OnValidate()
    {
        RandomizeText(textToRandomize);
    }

    /// <summary>
    /// The text randomizer only proceeds if the list of fonts is populated.
    /// It functions by using the rich text editing abilities of TMPro to 
    /// append the correct rich text tags to each character using the randomly
    /// selected font's name. 
    /// Fonts MUST be placed in the folder specified by TMPro's TMP_Settings file!
    /// By default this filepath is Assets/TextMesh Pro/Resources/Fonts & Materials
    /// </summary>
    /// <param name="text"></param>
    private void RandomizeText(string text)
    {
        if (fonts.Count == 0) { return; }
        string tempText = null; 

        foreach (char c in text)
        {
            int ranFont = Random.Range(0, fonts.Count);
            tempText += "<font=\"" + fonts[ranFont].name.ToString() + "\">" + c.ToString();
        }

        UpdateTextMesh(tempText);
    }

    /// <summary>
    /// Updates TMP's text to reflect the modified string. This function is separate
    /// to allow for additional functionality to be added to this script easily. 
    /// </summary>
    /// <param name="text"></param>
    private void UpdateTextMesh(string text)
    {
        if (textObject != null)
        {
            textObject.text = text;
        }
    }
}
