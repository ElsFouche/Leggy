using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviour
{
    private float dummyVelocity;
    public bool moveOn;
    private bool isWaiting = true;
    private HappinessManager happinessManager;
    private List<KeyCode> keysToAdvance;

    public float tutorialStartDelay = 2.0f;
    public GameObject blackScreen;
    public GameObject continueHint;
    public GameObject happinessExplanation;
    public GameObject controlsExplanation;
    

    // Start is called before the first frame update
    void Start()
    {
        // Replace with new input system when possible
        keysToAdvance = new List<KeyCode> 
        {
        KeyCode.Mouse0, 
        KeyCode.Mouse1,
        KeyCode.Mouse2,
        KeyCode.Mouse3,
        KeyCode.Mouse4,
        KeyCode.Mouse5,
        KeyCode.Escape,
        KeyCode.JoystickButton0,
        KeyCode.JoystickButton1,
        KeyCode.JoystickButton2,
        KeyCode.JoystickButton3,
        KeyCode.JoystickButton4,
        KeyCode.JoystickButton5,
        KeyCode.JoystickButton6,
        KeyCode.JoystickButton7
        };

        happinessManager = FindObjectOfType<HappinessManager>();
        happinessExplanation.SetActive(false);
        controlsExplanation.SetActive(false);
        continueHint.SetActive(false);
        blackScreen.SetActive(false);

        continueHint.transform.localPosition = new Vector3(-625, -100, 0);

        dummyVelocity = 0;

        happinessManager.viewingTutorial = true;
        StartCoroutine(DelayTutorialStart());
    }

    // Update is called once per frame
    void Update()
    {

        if (moveOn)
        {
            blackScreen.transform.localPosition = new Vector3(Mathf.SmoothDamp(blackScreen.transform.localPosition.x,
                0, ref dummyVelocity, 0.03f), Mathf.SmoothDamp(blackScreen.transform.localPosition.y,
                0f, ref dummyVelocity, 0.03f), 0);

            continueHint.transform.localPosition = new Vector3(Mathf.SmoothDamp(continueHint.transform.localPosition.x,
                675, ref dummyVelocity, 0.03f), Mathf.SmoothDamp(continueHint.transform.localPosition.y, 300, ref dummyVelocity, 0.03f), 0);
        }
        
        if (isWaiting) { return; }

        // Check a custom list of keys that will advance the tutorial. 
        foreach(KeyCode keyPressed in keysToAdvance)
        {
            if (Input.GetKeyDown(keyPressed) && happinessManager.viewingTutorial)
            {
                if (!moveOn)
                {
                    moveOn = true;
                    controlsExplanation.SetActive(false);
                    happinessExplanation.SetActive(true);
                }
                else if (moveOn)
                {
                    moveOn = false;
                    happinessManager.viewingTutorial = false;
                    blackScreen.SetActive(false);
                    continueHint.SetActive(false);
                    happinessExplanation.SetActive(false);

                    happinessManager.callCoroutine();

                    Destroy(this);
                }
            }
        }
    }

    private IEnumerator DelayTutorialStart()
    {
        yield return new WaitForSeconds(tutorialStartDelay);
        blackScreen.SetActive(true);
        continueHint.SetActive(true);
        controlsExplanation.SetActive(true);
        isWaiting = false;
    }
}
