using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class SnowmanManager : MonoBehaviour
{
    public GameObject BotGoal;
    public GameObject MidGoal;
    public GameObject TopGoal;

    public TransitionManager transitionManager;

    public GameObject gameover;

    private void Start()
    {
        gameover.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

        if (BotGoal.GetComponent<SnowmanGoal>().goalCompleted &&
            TopGoal.GetComponent<SnowmanGoal>().goalCompleted &&
            MidGoal.GetComponent<SnowmanGoal>().goalCompleted )
        {
            CompleteGoal();
        }
    }

    private void CompleteGoal()
    {
        Debug.Log("Goal Completed!");
        transitionManager.PressToStart();

        // Add any completion logic here (e.g., UI update, level progression, etc.)
        gameover.SetActive(true);
    }
}
