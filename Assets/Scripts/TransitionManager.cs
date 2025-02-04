using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public float fadeInDelay;
    public float fadeOutDelay;
    public TMP_Text loreText;
    //private string loreString;

    bool isTransitioning;

    public Image blackScreen;

    // Start is called before the first frame update
    void Start()
    {
        isTransitioning = false;

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        loreText.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(fadeToBlack());
            }
        }
    }

    public IEnumerator fadeToBlack()
    {
        while (blackScreen.GetComponent<Image>().color.a < 1.0f)
        {
            blackScreen.GetComponent<Image>().color = new Color
                (0, 0, 0, blackScreen.GetComponent<Image>().color.a + (Time.deltaTime / Mathf.Abs(2)));

            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator fadeTextIn()
    {
        while (loreText.color.a < 1.0f)
        {
            loreText.color = new Color
                (0, 0, 0, loreText.color.a + (Time.deltaTime / Mathf.Abs(2)));

            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator fadeTextOut()
    {
        while (loreText.color.a > 0.0f)
        {
            loreText.color = new Color
                (0, 0, 0, loreText.color.a - (Time.deltaTime / Mathf.Abs(2)));

            yield return new WaitForSeconds(0.5f);
        }
    }
}
