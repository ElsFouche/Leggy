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
/// </summary>
public class TagManager : MonoBehaviour
{
    /// <summary>
    /// The Main Tag defines what the object is.
    /// PlayerClaw refers to Leggy's claws.
    /// PlayerBody refers to Leggy's actual arm rather than its claw tips.
    /// Sort Locations aka goal zones are where a player should move an object to.
    /// Special Locations aka target zone are where a player should use a special object on.
    /// Objects to Sort are objects that need to be moved to a specific area.
    /// Objects to use aka key objects are objects that must be 'used' on a specific area 
    ///     but do not need to remain in the area for progress to be made on the task.
    /// Destructible objects are capable of being broken. 
    /// </summary>
    public enum MainTag
    {
        None,
        PlayerClaw,
        PlayerBody,
        SortLocation,
        SpecialLocation,
        ObjectToSort,
        ObjectToUse,
        Destructible
    }

    /// <summary>
    /// LocationTag is used to mark a sort location or special location.
    /// Sort locations only grant bonus happiness when an object with the 
    /// ObjectToSort tag and the correct ObjectTag is placed in them. 
    /// Other ObjectToSort objects only grant default happiness.
    /// 
    /// Special locations advance progress on a task, granting happiness when a certain
    /// amount of progress is made. Progress is only made when an object with the 
    /// ObjectToUse tag and the correct ObjectTag is used on them. 
    /// </summary>
    public enum LocationTag
    {
        None,
        Zone0,
        Zone1,
        Zone2, 
        Zone3,
        Zone4,
        Zone5,
        Zone6,
        Zone7,
        Zone8,
        Zone9
    }

    /// <summary>
    /// The ObjectTag is used in conjunction with the LocationTag to determine if
    /// an object is the correct one for a given task. ObjectToSort objects will
    /// have this tag checked to determine if they should grant bonus happiness.
    /// ObjectToUse objects will have this tag checked to determine if they're
    /// being used in the correct location.
    /// </summary>
    public enum ObjectTag
    {
        None,
        Zone0,
        Zone1,
        Zone2,
        Zone3,
        Zone4,
        Zone5,
        Zone6,
        Zone7,
        Zone8,
        Zone9
    }

    [Tooltip("The Main Tag defines what the game object is.")]
    public MainTag mainTag;
    [Tooltip("The Location Tag defines which location is which. Set this for goal zones.")]
    public LocationTag locationTag;
    [Tooltip("The Object Tag defines where an object should be moved or used. Set this for objects.")]
    public ObjectTag objectTag;
}
