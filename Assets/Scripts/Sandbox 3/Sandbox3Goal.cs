using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbox3Goal : MonoBehaviour
{
    public List<GameObject> objectiveObjectsList = new List<GameObject>();
    public List<GameObject> objectsInGoalList = new List<GameObject>();
    public bool goalCompleted = false;
    public float lerpSpeed = 5f;

    private void Update()
    {
        foreach (GameObject obj in objectsInGoalList)
        {
            if (obj != null && objectiveObjectsList.Contains(obj))
            {
                if (!obj.GetComponent<SnapStatus>().isSnapped)
                {
                    obj.transform.position = Vector3.Lerp(obj.transform.position, transform.position, lerpSpeed * Time.deltaTime);
                    obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, transform.rotation, lerpSpeed * Time.deltaTime);

                    if (Vector3.Distance(obj.transform.position, transform.position) < 0.1f && Quaternion.Angle(obj.transform.rotation, transform.rotation) < 1f)
                    {
                        obj.GetComponent<SnapStatus>().isSnapped = true;
                    }
                }
                else
                {
                    obj.transform.position = transform.position;
                    obj.transform.rotation = transform.rotation;
                }
            }
        }
    }

    private bool IsGoalComplete()
    {
        foreach (GameObject obj in objectiveObjectsList)
        {
            if (!objectsInGoalList.Contains(obj))
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (objectiveObjectsList.Contains(other.gameObject) && !objectsInGoalList.Contains(other.gameObject) && !goalCompleted)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            other.gameObject.tag = "Untagged";
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            objectsInGoalList.Add(other.gameObject);
            other.gameObject.AddComponent<SnapStatus>().isSnapped = false;

            goalCompleted = IsGoalComplete();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInGoalList.Contains(other.gameObject))
        {
            objectsInGoalList.Remove(other.gameObject);
            goalCompleted = IsGoalComplete();
        }
    }
}

public class SnapStatus : MonoBehaviour
{
    public bool isSnapped = false;
}
