using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    bool moveOn;
    public GameObject blackScreen;
    float dummy;

    List<GameObject> sceneObjects;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0;

        //sceneObjects = FindObjectsOfType<GameObject>().asList();

        for (int i = 0; i < sceneObjects.Count; i++)
        {

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
                0, ref dummy, 0.03f), 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (!moveOn) moveOn = true;
            else if (moveOn)
            {
                moveOn = false;
                Time.timeScale = 1;
                blackScreen.SetActive(false);
                Destroy(this);
            }
        }
    }
}
