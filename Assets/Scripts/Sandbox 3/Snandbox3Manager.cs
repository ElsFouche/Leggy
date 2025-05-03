using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

/// <summary>
/// Deprecated
/// </summary>
public class Sandbox3Manager : MonoBehaviour
{
    public GameObject frontGoal;
    public GameObject backGoal;
    public GameObject roofGoal;
    public GameObject leftWallGoal;
    public GameObject rightWallGoal;
    public GameObject floorGoal;


    public TransitionManager transitionManager;

    public GameObject gameover;

    private void Start()
    {
        gameover.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

        if (frontGoal.GetComponent<Sandbox3Goal>().goalCompleted &&
            backGoal.GetComponent<Sandbox3Goal>().goalCompleted &&
            roofGoal.GetComponent<Sandbox3Goal>().goalCompleted &&
            leftWallGoal.GetComponent<Sandbox3Goal>().goalCompleted &&
            rightWallGoal.GetComponent<Sandbox3Goal>().goalCompleted &&
            floorGoal.GetComponent<Sandbox3Goal>().goalCompleted
            )
        {
            CompleteGoal();
        }
    }

    private void CompleteGoal()
    {
        Debug.Log("Goal Completed!");
        // transitionManager.PressToStart();

        // Add any completion logic here (e.g., UI update, level progression, etc.)
        gameover.SetActive(true);
    }
}
