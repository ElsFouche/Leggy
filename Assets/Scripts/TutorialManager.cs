
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    private float dummyVelocity;
    private bool moveOn;
    private bool isWaiting = true;
    private HappinessManager happinessManager;
    private ClawControls controls;

    public float tutorialStartDelay = 2.0f;
    public GameObject blackScreen;
    public GameObject continueHint;
    public GameObject happinessExplanation;
    public GameObject controlsExplanation;

    private void Awake()
    {
        controls = new ClawControls();
        controls.Tutorial.Advance.performed += ctx => MoveOn();
    }

    private void OnEnable() => controls.Tutorial.Enable();
    private void OnDisable() => controls.Tutorial.Disable();

    // Start is called before the first frame update
    void Start()
    {
        happinessManager = GetComponent<HappinessManager>();
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
        if (isWaiting) { return; }

        if (moveOn)
        {
            blackScreen.transform.localPosition = new Vector3(Mathf.SmoothDamp(blackScreen.transform.localPosition.x,
                0, ref dummyVelocity, 0.03f), Mathf.SmoothDamp(blackScreen.transform.localPosition.y,
                0f, ref dummyVelocity, 0.03f), 0);

            continueHint.transform.localPosition = new Vector3(Mathf.SmoothDamp(continueHint.transform.localPosition.x,
                675, ref dummyVelocity, 0.03f), Mathf.SmoothDamp(continueHint.transform.localPosition.y, 300, ref dummyVelocity, 0.03f), 0);
        }
        
    }

    private void MoveOn()
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

            controls.Tutorial.Disable();
            controls.Player.Enable();
            Debug.Log("Control State, Player: " + controls.Player.enabled);
            Debug.Log("Control State, UI: " + controls.UI.enabled);
            Debug.Log("Control State, Tutorial: " + controls.Tutorial.enabled);

            Destroy(this);
        }
    }

    private IEnumerator DelayTutorialStart()
    {
        yield return new WaitForSeconds(tutorialStartDelay);
        controls.Player.Disable();
        controls.UI.Disable();
        controls.Tutorial.Enable();
        Debug.Log("Control State, Player: " + controls.Player.enabled);
        Debug.Log("Control State, UI: " + controls.UI.enabled);
        Debug.Log("Control State, Tutorial: " + controls.Tutorial.enabled);

        blackScreen.SetActive(true);
        continueHint.SetActive(true);
        controlsExplanation.SetActive(true);
        isWaiting = false;
    }
}
