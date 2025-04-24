using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempGoal : MonoBehaviour
{
    public TransitionManager transitionManager;

    public GameObject gameover;

    public List<GameObject> objectiveObjectsList = new List<GameObject>(); // Required objects
    public List<GameObject> objectsInGoalList = new List<GameObject>(); // Objects currently in the goal

    public bool goalCompleted = false; // Flag to indicate goal completion

    private void Start()
    {
        gameover.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsInGoalList.Contains(other.gameObject))
        {
            objectsInGoalList.Add(other.gameObject);
            CheckGoalCompletion();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInGoalList.Contains(other.gameObject))
        {
            objectsInGoalList.Remove(other.gameObject);
            CheckGoalCompletion(); // Recheck goal status if something leaves
        }
    }

    private void CheckGoalCompletion()
    {
        if (IsGoalComplete())
        {
            CompleteGoal();
        }
    }

    private bool IsGoalComplete()
    {
        // Ensure all objective objects are in the goal list
        foreach (GameObject obj in objectiveObjectsList)
        {
            if (!objectsInGoalList.Contains(obj)) // If even one is missing, goal is not complete
            {
                return false;
            }
        }
        return true; // All required objects are in the goal
    }

    private void CompleteGoal()
    {
        goalCompleted = true;
        Debug.Log("Goal Completed!");
        transitionManager.PressToStart();

        // Add any completion logic here (e.g., UI update, level progression, etc.)
        gameover.SetActive(true);
    }
}
