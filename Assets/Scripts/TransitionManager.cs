using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public float fadeInDelay;
    public float fadeOutDelay;
    public float sceneSwitchDelay;
    public TMP_Text loreText;
    float fadeTime;
    //private string loreString;

    bool isTransitioning;
    bool loreFadedIn;

    public Image blackScreen;

    public GameObject happinessManager;

    public int sceneToChangeTo;

    // Start is called before the first frame update
    void Start()
    {
        fadeTime = 1;
        loreFadedIn = false;
        isTransitioning = false;

        blackScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        loreText.color = new Color(1, 1, 1, 0);

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            StartCoroutine(blackFadeOut());
            happinessManager = GameObject.FindGameObjectWithTag("HappyManager");
        }
        else
        {
            blackScreen.gameObject.SetActive(false);
            loreText.gameObject.SetActive(false);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator fadeToBlack()
    {
        blackScreen.gameObject.SetActive(true);
        loreText.gameObject.SetActive(true);
        while (blackScreen.GetComponent<Image>().color.a < 1.0f)
        {
            blackScreen.GetComponent<Image>().color = new Color
                (0, 0, 0, blackScreen.GetComponent<Image>().color.a + Time.deltaTime);

            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(fadeText(fadeInDelay, fadeOutDelay, sceneSwitchDelay));
    }

    public IEnumerator blackFadeOut()
    {
        while (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            blackScreen.GetComponent<Image>().color = new Color (0, 0, 0, fadeTime);

            yield return new WaitForSeconds(0.01f);
        }
        happinessManager.GetComponent<HappinessManager>().isVisible = true;
    }

    public IEnumerator fadeText(float fadeInDelay, float fadeOutDelay, float sceneSwitchDelay)
    {
        yield return new WaitForSeconds(fadeInDelay);

        while (loreText.color.a < 1.0f)
        {
            if (!loreFadedIn)
            {
                loreText.color = new Color(1, 1, 1, loreText.color.a + Time.deltaTime);

                yield return new WaitForSeconds(0.01f);
            }
        }
        loreFadedIn = true;
        yield return new WaitForSeconds(fadeOutDelay);

        while (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            loreText.color = new Color(fadeTime, fadeTime, fadeTime, 1);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(sceneSwitchDelay);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(sceneToChangeTo);
    }

    public void PressToStart()
    {
        isTransitioning = true;
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            happinessManager.GetComponent<HappinessManager>().isVisible = false;
        }
        StartCoroutine(fadeToBlack());
    }
}
