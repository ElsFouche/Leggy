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
    public bool levelComplete = false;

    private void Start()
    {
        gameover.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

        if (BotGoal.GetComponent<SnowmanGoal>().goalCompleted &&
            TopGoal.GetComponent<SnowmanGoal>().goalCompleted &&
            MidGoal.GetComponent<SnowmanGoal>().goalCompleted &&
            levelComplete == false)
        {
            CompleteGoal();
            levelComplete = true;
            StartCoroutine(delay());
        }
    }

    private void CompleteGoal()
    {
        Debug.Log("Goal Completed!");
        transitionManager.TransitionToScene(3);
        
        gameover.SetActive(true);
    }

   public IEnumerator delay()
    {
        yield return new WaitForSeconds(1);

        transitionManager.TransitionToScene(3);
    }
}
