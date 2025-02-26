using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public TagManager tagManager;

    public int matchingCollisionNumber;
    public int generalCollisionNumber;

    public HappinessManager happinessManager;
    public GameObject objectiveSetter;

    public int matchingObjectHappiness; // This allows the designer to determine how much happiness a matching object gives.
    public int generalObjectHappiness; // As above but for general objects.
    public int minNumMatchingObjects; // The minimum number of objects that must match before getting happiness.
    public int minNumGeneralObjects; // As above but for general objects.

    // Start is called before the first frame update
    void Start()
    {
        happinessManager = FindObjectOfType<HappinessManager>();

        Debug.Log(tagManager.locationTag);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Setup phase
        TagManager hitTags = null; // Create temp variable to hold reference to collision object's tags.

        if (collision.gameObject.GetComponent<TagManager>() == null) // Confirm hit object has a TagManager.
        {
            return; // If the object we hit has no TagManager, exit this code. 
        }
        else
        {
            hitTags = collision.gameObject.GetComponent<TagManager>(); // Store reference to hit object's tags.
        }

        // Check for Matching Collisions
        if ((int)hitTags.objectTag == (int)tagManager.locationTag) // Use explicit int casts to compare tag values.
        {
            // Store the number of matching objects for later.
            matchingCollisionNumber++;
            
            if (matchingCollisionNumber > minNumMatchingObjects)
            {
                 // Call function to gain happiness
                 // gainHappiness(matchingObjectHappines);
            }
            
        }
        // Check for Non-Matching Collisions only if the object doesn't match. 
        // Enumerator comparison can be done directly like this. 
        else if (hitTags.objectTag != TagManager.ObjectTag.None)
        {
            // Store the number of sortable objects for later. 
            generalCollisionNumber++;

            // Check if the minimum number of general objects has been reached. If so, add happiness.
            if  (generalCollisionNumber > minNumGeneralObjects)
            {
                 // Call function to gain happiness
                 // gainHappiness(generalObjectHappines);
            }
        }

        // Check for Condition Met - Unnecessary 
        // Hardcoding values is bad practice.  
        if (generalCollisionNumber == 3) // This value needs to be accessible to designers. 
        {
            // This part should be rewritten entirely. 
            if (generalCollisionNumber == matchingCollisionNumber)
                happinessManager.maxHappy();
            else happinessManager.normalHappy();
        }
    }

    // This function will be called whn an object is sorted. It will implement
    // what happens when the player should gain happiness. There should be a 
    // corresponding function for managing what happens when the player loses happiness. 
    /*private void gainHappiness(int happinessToGain)
    {
        happinessManager.GetComponent<happinessManager>().gainHappiness(happinessToGain);
    }*/

    private void OnCollisionExit(Collision collision)
    {








        /*
        if (collision.gameObject.GetComponent<TagManager>().objectTag.ToString()
            == tagManager.locationTag.ToString())
        {
            matchingCollisionNumber--;
        }
        if (collision.gameObject.GetComponent<TagManager>().objectTag.ToString() != "None")
            generalCollisionNumber--;

        if (generalCollisionNumber < 3)
        {
            happinessManager.GetComponent<HappinessManager>().getDepressed();
        }
        */
    }
}
