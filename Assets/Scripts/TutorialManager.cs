using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool moveOn;
    public GameObject blackScreen;
    public GameObject continueHint;
    public GameObject happinessExplanation;
    float dummy;

    public bool viewingTutorial;

    public List<GameObject> sceneObjects;

    // Start is called before the first frame update
    void Start()
    {
        viewingTutorial = true;
        happinessExplanation.SetActive(false);

        continueHint.transform.localPosition = new Vector3(-625, -100, 0);

        //Time.timeScale = 0;

        foreach (GameObject sceneObject in FindObjectsOfType<GameObject>())
        {
            sceneObjects.Add(sceneObject);
        }

        for (int i = 0; i < sceneObjects.Count; i++)
        {
            if (sceneObjects[i].gameObject.name.Contains("Leggy") || sceneObjects[i].gameObject.name == "Wrist_rotate_jnt")
            {
                sceneObjects.RemoveAt(i);
                i--;
            }
            if (sceneObjects[i].GetComponent<Rigidbody>() != null)
            {
                //sceneObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                //sceneObjects[i].GetComponent<Rigidbody>().useGravity = false;
                //sceneObjects[i].GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        dummy = 0;
        blackScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveOn)
        {
            blackScreen.transform.localPosition = new Vector3(Mathf.SmoothDamp(blackScreen.transform.localPosition.x,
                0, ref dummy, 0.03f), Mathf.SmoothDamp(blackScreen.transform.localPosition.y,
                0f, ref dummy, 0.03f), 0);

            continueHint.transform.localPosition = new Vector3(Mathf.SmoothDamp(continueHint.transform.localPosition.x,
                675, ref dummy, 0.03f), Mathf.SmoothDamp(continueHint.transform.localPosition.y, 300, ref dummy, 0.03f), 0);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (!moveOn)
            {
                moveOn = true;
                happinessExplanation.SetActive(true);
            }
            else if (moveOn)
            {
                moveOn = false;
                viewingTutorial = false;
                blackScreen.SetActive(false);
                continueHint.SetActive(false);
                happinessExplanation.SetActive(false);

                gameObject.GetComponent<HappinessManager>().callCoroutine();

                for (int i = 0; i < sceneObjects.Count; i++)
                {
                    if (sceneObjects[i].GetComponent<Rigidbody>() != null &&
                        sceneObjects[i].GetComponent<Rigidbody>().velocity == Vector3.zero)
                    {
                        //sceneObjects[i].GetComponent<Rigidbody>().velocity = Vector3.one;
                        //sceneObjects[i].GetComponent<Rigidbody>().useGravity = true;
                        //sceneObjects[i].GetComponent<Rigidbody>().isKinematic = false;
                    }
                }

                Destroy(this);
            }
        }
    }
}
