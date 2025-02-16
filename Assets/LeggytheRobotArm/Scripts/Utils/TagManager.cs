using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

/// <summary>
/// Els Fouché - 02/16/2025
/// This simple script is a slightly more robust tag manager than Unity's 
/// built-in version. It is easily extensible and allows for tagged objects
/// to be searched for via a single component. Tagged objects can then be
/// loaded into an array and sorted by their unique tag identifiers. 
/// 
/// This script may or may not prove useful for this project but it is a 
/// standard part of the scripts I begin work with. 
/// </summary>
public class TagManager : MonoBehaviour
{
    public enum MainTag
    {
        None,
        Player,
        EnvironmentStatic,
        Interactable
    }

    public enum InteractableTag
    {
        None,
        ObjecttoMove,
        GoalZone,
        KeyObject,
        Destructible
    }

    public MainTag mainTag;
    public InteractableTag interactableTag;
}
