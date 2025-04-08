using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool moveOn;
    public GameObject blackScreen;
    float dummy;

    public List<GameObject> sceneObjects;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0;

        foreach (GameObject sceneObject in FindObjectsOfType<GameObject>())
        {
            sceneObjects.Add(sceneObject);
        }

        for (int i = 0; i < sceneObjects.Count; i++)
        {
            if (sceneObjects[i].GetComponent<Rigidbody>() != null)
            {
                sceneObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                sceneObjects[i].GetComponent<Rigidbody>().useGravity = false;
                sceneObjects[i].GetComponent<Rigidbody>().isKinematic = true;
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
                0, ref dummy, 0.03f), 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (!moveOn) moveOn = true;
            else if (moveOn)
            {
                moveOn = false;

                for (int i = 0; i < sceneObjects.Count; i++)
                {
                    if (sceneObjects[i].GetComponent<Rigidbody>() != null &&
                        sceneObjects[i].GetComponent<Rigidbody>().velocity == Vector3.zero)
                    {
                        sceneObjects[i].GetComponent<Rigidbody>().velocity = Vector3.one;
                        sceneObjects[i].GetComponent<Rigidbody>().useGravity = true;
                        sceneObjects[i].GetComponent<Rigidbody>().isKinematic = false;
                    }
                }
                blackScreen.SetActive(false);
                Destroy(this);
            }
        }
    }
}
