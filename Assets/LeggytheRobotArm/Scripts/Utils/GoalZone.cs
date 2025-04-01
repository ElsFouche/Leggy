using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public enum happinessValues
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
    public happinessValues matchingObjectHappiness = happinessValues._250; // This allows the designer to determine how much happiness a matching object gives.
    [Tooltip("Happiness gained from incorrect objects.")]
    public happinessValues generalObjectHappiness = happinessValues._100;
    [Tooltip("Number of matching objects required to gain happiness.")]
    [Range(1, 10)]
    public int minNumMatchingObjects; // The minimum number of objects that must match before getting happiness.
    [Tooltip("Number of mismatched objects required to gain happiness.")]
    [Range(1, 10)]
    public int minNumGeneralObjects; // As above but for general objects.

    void Start()
    {
        tagManager = this.GetComponent<TagManager>();
        // This needs to be changed.
        happinessManager = FindObjectOfType<HappinessManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        TagManager hitTags = null; 

        if (other.gameObject.GetComponent<TagManager>() == null) 
        {
            // If the object we hit has no TagManager, exit this code block. 
            return; 
        }
        else
        {
            hitTags = other.gameObject.GetComponent<TagManager>(); 
        }

        if ((int)hitTags.zoneTag == (int)tagManager.zoneTag) 
        {
            matchingCollisionNumber++;
            Debug.Log("Matching Objects: " + matchingCollisionNumber);

            if (matchingCollisionNumber % minNumMatchingObjects == 0)
            {
                Debug.Log("Gain Happiness: " + (int)matchingObjectHappiness);
                gainHappiness((int)matchingObjectHappiness);
            }

        }
        else if (hitTags.mainTag == TagManager.MainTag.ObjectToSort)
        {
            generalCollisionNumber++;
            Debug.Log("Mismatched Objects: " +  generalCollisionNumber);

            if (generalCollisionNumber % minNumGeneralObjects == 0)
            {
                Debug.Log("Gain Happiness: " + (int)generalObjectHappiness);
                gainHappiness((int)generalObjectHappiness);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TagManager hitTags = null;

        if (other.gameObject.GetComponent<TagManager>() == null) 
        {
            return; 
        }
        else
        {
            hitTags = other.gameObject.GetComponent<TagManager>(); 
        }

        // These need to change.
        if ((int)hitTags.zoneTag == (int)tagManager.zoneTag) 
        {
            matchingCollisionNumber--;
            Debug.Log("Matching Objects: " + matchingCollisionNumber);

            if (matchingCollisionNumber % minNumMatchingObjects == minNumMatchingObjects - 1)
            {
                Debug.Log("Lose Happiness: " + (int)matchingObjectHappiness); 
                loseHappiness((int)matchingObjectHappiness);
            }
        }
        else if (hitTags.zoneTag != TagManager.ZoneTag.None)
        {
            generalCollisionNumber--;

            if (generalCollisionNumber % minNumGeneralObjects == minNumGeneralObjects - 1)
            {
                Debug.Log("Lose Happiness: " + (int)generalObjectHappiness);
                loseHappiness((int)generalObjectHappiness);
            }
        }
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