using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
/// <summary>
/// Els Fouché - 04/15/2025
/// This script handles objective tracking in a level.
/// Level Designers will populate the list of goal zones with 
/// the goal zone prefabs they've added to a level.
/// These goal zones report to this tracker which handles the
/// relevant on-screen text reminders and will end the level 
/// when all goals are reporting complete. 
/// </summary>
public class ObjectiveTracker : MonoBehaviour
{
    // Serialized
    [Header("Goal Zone List")]
    [Tooltip("This list must be populated with all goal zones being used in the level!")]
    [SerializeField] List<GameObject> goalZones = new();

    // Private
    private Dictionary<int, GoalState> goals = new();
    private GameManager gameManager;
    // TODO: Add objective text handling
    // TODO: Add VFX triggers

    // Public
    [Header("Level Completion")]
    [Tooltip("This is the amount of happiness lost if the player\n" +
        "exits a level before completing enough objectives.\n" +
        "Value is multiplied by 500.")]
    [Range(1, 10)]
    public int earlyExitHappinessLoss;
    [Tooltip("This is the number of goals the player must satisfy to not lose happiness when ending the level early.")]
    public int minNumGoalsCompleted;

    public enum GoalState
    {
        Incomplete,
        Satisfied,
        Perfect
    }

    void Start()
    {
        earlyExitHappinessLoss *= 500;
        // init range slider values if they have not been set. 
        if (earlyExitHappinessLoss <= 0) earlyExitHappinessLoss = 50;

        gameManager = transform.root.GetComponent<GameManager>();
        if (!gameManager)
        {
            Debug.Log("Game Manager not found!");
            return;
        }

        // Populate private dictionary with goal zones & their ID values
        foreach (GameObject goalZone in goalZones)
        {
            SetGoal(goalZone.GetComponent<GoalZone>().GetInstanceID(), GoalState.Incomplete);
            goalZone.GetComponent<GoalZone>().SetTrackerRef(this.GetComponent<ObjectiveTracker>());
        }
    }

    /// <summary>
    /// Pass in your object ID and goal state. 
    /// </summary>
    /// <param name="goalID"></param>
    /// <param name="goalState"></param>
    public void SetGoal(int goalID, GoalState goalState)
    {
        if (!goals.TryAdd(goalID, goalState))
        {
            goals[goalID] = goalState;
            Debug.Log("Goal ID: " + goalID + "Goal Value: " + goalState);
        }

        AutoFinishLevel();
    }

    /// <summary>
    /// Automatically finish the level if the player has completed all the objectives.
    /// Called each time a goal is updated. 
    /// </summary>
    private void AutoFinishLevel()
    {
        if (CountCompletedGoals() >= goals.Count)
        {
            Debug.Log("Level complete.");
            gameManager.FinishLevel();
        }
    }

    /// <summary>
    /// Counts the number of goals that have been completed.
    /// Does not differentiate between correct and incorrect goal completions. 
    /// </summary>
    /// <returns></returns>
    public int CountCompletedGoals()
    {
        int completeCount = 0;
        foreach (GoalState goalState in goals.Values)
        {
            if ((int)goalState >= 1)
            {
                completeCount++;
            }
        }
        return completeCount;
    }
}
