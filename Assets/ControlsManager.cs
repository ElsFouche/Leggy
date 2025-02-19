using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{
    public Image speedTrackerLeft;
    public Image speedTrackerRight;

    public bool isLow;

    float amountOnLow;

    // Start is called before the first frame update
    void Start()
    {
        isLow = true;
        amountOnLow = 1;
    }

    float zero = 0f;

    // Update is called once per frame
    void Update()
    {
        speedTrackerRight.GetComponent<Image>().fillAmount = 1 - speedTrackerLeft.GetComponent<Image>().fillAmount;

        speedTrackerLeft.GetComponent<Image>().fillAmount = Mathf.SmoothDamp
                    (speedTrackerLeft.GetComponent<Image>().fillAmount, amountOnLow, ref zero, 0.03f);

        if (Input.GetKeyDown(KeyCode.P))
        {
            changeFloatValue();

            isLow = !isLow;
        }
    }

    public void changeFloatValue()
    {
        if (isLow) while (!Mathf.Approximately(amountOnLow, 0f))
            {
                amountOnLow = Mathf.Lerp(amountOnLow, 0f, Time.deltaTime);
            }

        else while (!Mathf.Approximately(amountOnLow, 1f))
            {
                amountOnLow = Mathf.Lerp(amountOnLow, 1f, Time.deltaTime);
            }

    }
}
