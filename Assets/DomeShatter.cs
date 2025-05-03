using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DomeShatter : MonoBehaviour
{

    public GameObject GumballParent;
    public List<Rigidbody> GumballStorage = new List<Rigidbody>();

    public GameObject NormalDome;
    public GameObject ShatteredDome;

    public GameObject Hammer;

    [TextArea]
    public string endLevelWrongText;
    public TransitionManager.Speaker font;

    private Rigidbody hammerRigidbody;
    private bool gumballMachineIsBroken = false;
    private GameManager gameManager;
    private TransitionManager transitionManager;

    private void Awake()
    {
        for (int gumball = 0; gumball < GumballParent.transform.childCount; gumball++)
        {
            GumballStorage.Add(GumballParent.transform.GetChild(gumball).GetComponent<Rigidbody>());
        }
        hammerRigidbody = Hammer.GetComponent<Rigidbody>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject temp = GameObject.FindWithTag("GameManager");
        if (temp.TryGetComponent<GameManager>(out gameManager)) 
        {
            Debug.Log("Dome Shatter successfully found reference to game manager.");
        }
        if (temp.TryGetComponent<TransitionManager>(out transitionManager))
        {
            Debug.Log("Dome Shatter successfully found reference to transition manager.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (gumballMachineIsBroken) { return; }
        BasketData tempData; 
        if(other.transform.parent.TryGetComponent<BasketData>(out tempData))
        {
            if (tempData.objectRigidbody == hammerRigidbody)
            {
                BreakGumballMachine();
            }
        } else if (other.TryGetComponent<BasketData>(out tempData))
        {
            if (tempData.objectRigidbody == hammerRigidbody && !gumballMachineIsBroken)
            {
                BreakGumballMachine();
            }
        }
/*
        if(other.transform.parent.GetComponent<BasketData>().objectRigidbody == Hammer.GetComponent<Rigidbody>() ||
            other.GetComponent<BasketData>().objectRigidbody == Hammer.GetComponent<Rigidbody>()) 
        {
            for (int gumball = 0; gumball < GumballStorage.Count; gumball++)
            {
                GumballStorage[gumball].GetComponent<Rigidbody>().isKinematic = false;
            }

            ShatteredDome.gameObject.SetActive(true);

            for (int pieces = 0; pieces < ShatteredDome.transform.childCount; pieces++)
            {
                ShatteredDome.transform.GetChild(pieces).GetComponent<Rigidbody>().isKinematic = false;
            }
            NormalDome.SetActive(false);
        }
*/
    }

    private void BreakGumballMachine()
    {
        gumballMachineIsBroken = true;
        for (int gumball = 0; gumball < GumballStorage.Count; gumball++)
        {
            GumballStorage[gumball].GetComponent<Rigidbody>().isKinematic = false;
        }

        ShatteredDome.SetActive(true);

        for (int pieces = 0; pieces < ShatteredDome.transform.childCount; pieces++)
        {
            ShatteredDome.transform.GetChild(pieces).GetComponent<Rigidbody>().isKinematic = false;
        }

        NormalDome.SetActive(false);

        EndLevel(endLevelWrongText);
    }

    private void EndLevel(string levelEndText)
    {
        transitionManager.loreText.SetText(levelEndText);
        transitionManager.font = font;
        gameManager.FinishLevel();
    }
}