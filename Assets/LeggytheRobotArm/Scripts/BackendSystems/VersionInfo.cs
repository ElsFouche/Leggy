using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

/// <summary>
/// This script displays the version information on the title screen.
/// </summary>
public class VersionInfo : MonoBehaviour
{
    private TMP_Text versionDisplay;

    private void Start()
    {
        versionDisplay = GetComponent<TMP_Text>();
        if (versionDisplay) versionDisplay.text = "v" + PlayerSettings.bundleVersion;
    }
}
