using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using UnityEngine;

/// <summary>
/// The purpose of this script is to be attached to a goal zone object, 
/// detect when an object has entered the goal, and
/// update the relevant code as needed. 
/// 
/// For example: 
/// a cube sorted into the cube sorting zone would update 
/// our score with the best possible result. 
/// A cylinder sorted into the cube sorting zone would update 
/// our score with a positive but not great result.
/// Should an object leave the goal zone, our score should be 
/// updated accordingly. 
/// 
/// Revisions to this script made by Els Fouché on 03/31/2025
/// </summary>

[RequireComponent(typeof(TagManager))]
[RequireComponent(typeof(Collider))]

public class GoalZone : MonoBehaviour
{
    private TagManager tagManager;
    private HappinessManager happinessManager;
    private int matchingCollisionNumber = 0;
    private int generalCollisionNumber = 0;
    private HashSet<int> objectIDs = new HashSet<int>();
    private ObjectiveTracker.GoalState goalState;
    private ObjectiveTracker tracker;
    private int instanceID;
    private bool collisionEnterChecking = false;
    private bool collisionExitChecking = false;

    public enum HappinessValues
    {
        _000 = 0,
        _050 = 50,  
        _100 = 100,
        _150 = 150,
        _200 = 200,
        _250 = 250,
        _300 = 300,
        _350 = 350,
        _400 = 400,
        _450 = 450,
        _500 = 500
    }

    [Tooltip("Happiness gained from correct objects.")]
    public HappinessValues matchingObjectHappiness = HappinessValues._250; // This allows the designer to determine how much happiness a matching object gives.
    [Tooltip("Happiness gained from incorrect objects.")]
    public HappinessValues generalObjectHappiness = HappinessValues._100;
    [Tooltip("Number of matching objects required to gain happiness.")]
    [Range(1, 10)]
    public int minNumMatchingObjects; // The minimum number of objects that must match before getting happiness.
    [Tooltip("Number of mismatched objects required to gain happiness.")]
    [Range(1, 10)]
    public int minNumGeneralObjects; // As above but for general objects.

    // Setter
    public void SetTrackerRef(ObjectiveTracker objectiveTracker)
    {
        tracker = objectiveTracker;
    }

    void Start()
    {
        // If a level designer has not adjusted the slider values they will stay 0
        // even if set during the initialization above. Initialize here instead:
        if (minNumMatchingObjects <= 0) minNumMatchingObjects = 1;
        if (minNumGeneralObjects <= 0) minNumGeneralObjects = 1;

        tagManager = this.GetComponent<TagManager>();
        // This needs to be changed.
        instanceID = this.GetInstanceID();

        StartCoroutine(DelayForSeconds(0.2f));
    }

    private IEnumerator DelayForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AfterDelayLogic();
    }

    private void AfterDelayLogic()
    {
        happinessManager = FindObjectOfType<HappinessManager>();
        
        if (!tracker)
        {
            Debug.Log("Objective tracker not found! Are you sure you loaded this goal zone into the objective tracker?");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TagManager hitTags = null;
/*
        // This has a potential bug when the object has been had its parent set as Leggy. 
        if (other.gameObject.transform.root.GetComponent<TagManager>() == null) 
        {
            // If the object we hit has no TagManager, exit this code block. 
            return; 
        }
        else
        {
            hitTags = other.gameObject.transform.root.GetComponent<TagManager>();
            if (!objectIDs.Add(other.gameObject.transform.root.GetInstanceID()))
            {
                return;
                // Exit
            }
            else
            {
                Debug.Log("Adding: " + other.gameObject.transform.root.GetInstanceID());
                // Continue
            }
        }
*/
        // Check for lock
        if (collisionEnterChecking) return;
        collisionEnterChecking = true;
        // If the tag manager is at the level of the collider, assign it
        if (other.gameObject.GetComponent<TagManager>() != null)
        {
            hitTags = other.transform.GetComponent<TagManager>();
            Debug.Log("Tag manager found: " + hitTags.GetInstanceID());
            if (objectIDs.Add(other.transform.GetInstanceID()))
            {
                Debug.Log("Adding: " + other.gameObject.name + " with ID: " + other.transform.GetInstanceID());
                // Continue
            }
            else
            {
                Debug.Log("Adding object failed.");
                collisionEnterChecking = false;
                return;
                // Exit
            }
        }
        else
        {
            Transform hierarchyPosition;
            hierarchyPosition = other.transform.parent;
            // Otherwise, iterate up the hierarchy
            while (hitTags == null && hierarchyPosition != null)
            {
                Debug.Log("Loop | Checking " + hierarchyPosition.name + " for tag manager.");
                if (hierarchyPosition.GetComponent<TagManager>() != null)
                {
                    hitTags = hierarchyPosition.GetComponent<TagManager>();
                    if (objectIDs.Add(hierarchyPosition.GetInstanceID()))
                    {
                        Debug.Log("Adding: " + hierarchyPosition.GetInstanceID());
                        // Continue
                    }
                    else
                    {
                        Debug.Log("Adding object failed.");
                        collisionEnterChecking = false;
                        return;
                        // Exit
                    }
                } else
                {
                    hierarchyPosition = hierarchyPosition.transform.parent;
                }
            }
        }

        if (hitTags == null) { collisionEnterChecking = false;  return; }

        // Begin sortable object logic
        if (hitTags.mainTag == TagManager.MainTag.ObjectToSort && (int)hitTags.zoneTag == (int)tagManager.zoneTag) 
        {
            matchingCollisionNumber++;
            Debug.Log("Matching Objects: " + matchingCollisionNumber);

            if (minNumMatchingObjects != 0 && matchingCollisionNumber % minNumMatchingObjects == 0)
            {
                Debug.Log("Gain Happiness: " + (int)matchingObjectHappiness);
                gainHappiness((int)matchingObjectHappiness);
                goalState = ObjectiveTracker.GoalState.Perfect;
                if (tracker) tracker.SetGoal(instanceID, goalState);
            }

        }
        else if (hitTags.mainTag == TagManager.MainTag.ObjectToSort)
        {
            generalCollisionNumber++;
            Debug.Log("Mismatched Objects: " +  generalCollisionNumber);

            if (minNumGeneralObjects != 0 && generalCollisionNumber % minNumGeneralObjects == 0) 
            {
                Debug.Log("Gain Happiness: " + (int)generalObjectHappiness);
                gainHappiness((int)generalObjectHappiness);
                if (goalState != ObjectiveTracker.GoalState.Perfect)
                {
                    goalState = ObjectiveTracker.GoalState.Satisfied;
                    if (tracker) tracker.SetGoal(instanceID, goalState);
                }
            }
        }
        // Unlock
        collisionEnterChecking = false;
    }

    private void OnTriggerExit(Collider other)
    {
        TagManager hitTags = null;
        // Debug.Log("Other Object ID Exited: " + other.gameObject.transform.root.GetInstanceID());
/*
        if (other.gameObject.transform.root.GetComponent<TagManager>() == null) 
        {
            return; 
        }
        else
        {
            hitTags = other.gameObject.transform.root.GetComponent<TagManager>();
            if (!objectIDs.Remove(other.gameObject.transform.root.GetInstanceID()))
            {
                return; 
                // Exit
            }
            else 
            { 
                Debug.Log("Removing: " + other.gameObject.transform.root.GetInstanceID());
                // Continue
            }
        }
*/
        
        // Check for lock
        if (collisionExitChecking) return;
        collisionExitChecking= true;
        // If the tag manager is at the level of the collider, assign it
        if (other.gameObject.GetComponent<TagManager>() != null)
        {
            hitTags = other.transform.GetComponent<TagManager>();
            Debug.Log("Tag manager found: " + hitTags.GetInstanceID());
            if (objectIDs.Remove(other.transform.GetInstanceID()))
            {
                Debug.Log("Removing: " + other.gameObject.name + " with ID: " + other.transform.GetInstanceID());
                // Continue
            }
            else
            {
                collisionExitChecking= false;
                Debug.Log("Failed to remove object.");
                return;
                // Exit
            }
        }
        else
        {
            Transform hierarchyPosition;
            hierarchyPosition = other.transform.parent;
            // Otherwise, iterate up the hierarchy
            while (hitTags == null && hierarchyPosition != null)
            {
                Debug.Log("Loop | Checking " +  hierarchyPosition.name + " for tag manager.");
                if (hierarchyPosition.GetComponent<TagManager>() != null)
                {
                    hitTags = hierarchyPosition.GetComponent<TagManager>();
                    if (objectIDs.Remove(hierarchyPosition.GetInstanceID()))
                    {
                        Debug.Log("Removing: " + hierarchyPosition.GetInstanceID());
                        // Continue
                    }
                    else
                    {
                        collisionExitChecking= false;
                        Debug.Log("Failed to remove object.");
                        return;
                        // Exit
                    }
                }
                else
                {
                    hierarchyPosition = hierarchyPosition.transform.parent;
                }
            }
        }

        if (hitTags == null) { collisionExitChecking= false;  return; }

        if ((int)hitTags.zoneTag == (int)tagManager.zoneTag) 
        {
            matchingCollisionNumber--;
            Debug.Log("Matching Objects: " + matchingCollisionNumber);

            if (matchingCollisionNumber % minNumMatchingObjects == minNumMatchingObjects - 1)
            {
                Debug.Log("Lose Happiness: " + (int)matchingObjectHappiness); 
                loseHappiness((int)matchingObjectHappiness);
                if (generalCollisionNumber % minNumGeneralObjects == minNumGeneralObjects - 1
                    && matchingCollisionNumber < minNumMatchingObjects)
                { 
                    goalState = ObjectiveTracker.GoalState.Satisfied;
                    if (tracker) tracker.SetGoal(instanceID, goalState);
                } else
                {
                    goalState = ObjectiveTracker.GoalState.Incomplete;
                    if (tracker) tracker.SetGoal(instanceID, goalState);
                }
            }
        }
        else if (hitTags.zoneTag != TagManager.ZoneTag.None)
        {
            generalCollisionNumber--;

            if (generalCollisionNumber % minNumGeneralObjects == minNumGeneralObjects - 1)
            {
                Debug.Log("Lose Happiness: " + (int)generalObjectHappiness);
                loseHappiness((int)generalObjectHappiness);
                if (goalState != ObjectiveTracker.GoalState.Perfect && generalCollisionNumber < minNumGeneralObjects)
                {
                    goalState = ObjectiveTracker.GoalState.Incomplete;
                    if (tracker) tracker.SetGoal(instanceID, goalState);
                }
            }
        }
        // Unlock
        collisionExitChecking= false;
    }

    private void gainHappiness(int happinessToGain)
    {
        happinessManager.gainHappiness(happinessToGain);
    }

    private void loseHappiness(int happinessToLose)
    {
        happinessManager.loseHappiness(happinessToLose);
    }
}