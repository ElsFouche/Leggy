using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public TagManager tagManager;

    public int matchingCollisionNumber;
    public int generalCollisionNumber;

    public GameObject happinessManager;
    public GameObject objectiveSetter;

    // Start is called before the first frame update
    void Start()
    {
        tagManager = gameObject.GetComponent<TagManager>();

        Debug.Log(tagManager.locationTag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<TagManager>().objectTag.ToString()
            == tagManager.locationTag.ToString())
        {
            matchingCollisionNumber++;
        }
        if (collision.gameObject.GetComponent<TagManager>().objectTag.ToString() != "None")
            generalCollisionNumber++;

        if (generalCollisionNumber == 3)
        {
            if (generalCollisionNumber == matchingCollisionNumber)
                happinessManager.GetComponent<HappinessManager>().maxHappy();
            else happinessManager.GetComponent<HappinessManager>().normalHappy();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
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
    }
}
