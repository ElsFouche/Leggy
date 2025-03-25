using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TagManager))]
[RequireComponent(typeof(Collider))]

/* This script is supposed to be the point of contact for LDers.
 * It should implement logic based on the various combinations of 
 * sortable objects, sort locations, etc. 
 * It should make use of interfaces or possibly Unity's event system
 * to ask for certain outcomes based on the interactions. 
 */
public class ObjectSortingChecker : MonoBehaviour
{
    TagManager otherObjectTags;
    TagManager ourTags;

    private void Start()
    {
        ourTags = GetComponent<TagManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Collider>() == null)
        {
            return;
        }

        if (other.gameObject.GetComponent<TagManager>() != null)
        {
            otherObjectTags = other.gameObject.GetComponent<TagManager>();
        } else { return; }

        switch (ourTags.mainTag) 
        {
            case TagManager.MainTag.None:
                break;
            case TagManager.MainTag.PlayerClaw:
                break;
            case TagManager.MainTag.PlayerBody:
                break;
            case TagManager.MainTag.SortLocation:
                if (otherObjectTags.mainTag == TagManager.MainTag.ObjectToSort)
                {
                    // Logic
                }
                break;
            case TagManager.MainTag.SpecialLocation:
                if (otherObjectTags.mainTag == TagManager.MainTag.ObjectToUse)
                {
                    // Logic
                }
                break;
            case TagManager.MainTag.ObjectToSort:
                if (otherObjectTags.mainTag == TagManager.MainTag.SortLocation)
                {
                    // Logic
                }
                break;
            case TagManager.MainTag.ObjectToUse:
                if (otherObjectTags.mainTag == TagManager.MainTag.SpecialLocation)
                {
                    // Logic
                }
                break;
            case TagManager.MainTag.Destructible:
                if (otherObjectTags.mainTag == TagManager.MainTag.ObjectToUse)
                {
                    // Logic
                }
                break;
            default:
                break;
        }
    }
}
