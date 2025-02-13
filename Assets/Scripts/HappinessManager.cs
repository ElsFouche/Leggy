using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessManager : MonoBehaviour
{
    //The higher the position, the worse the state
    public List<AudioClip> emotes;
    public int extraHappy;
    public int happy;
    public int sad;
    public AudioSource speaker;
    public TextMeshProUGUI happinessDisplay;
    public int happinessCount;
    public int maxDepressor;
    public float depressorMultiplier;
    float timer;

    public bool isVisible;

    public Image backgroundBar;
    public Image backgroundDepressLayer;
    public TextMeshProUGUI thousandTracker;
    int thousands;

    bool decreasing;
    public bool buffering;

    // Start is called before the first frame update
    void Start()
    {
        backgroundDepressLayer.GetComponent<Image>().fillAmount = backgroundBar.GetComponent<Image>().fillAmount;
        updateThousands();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            speaker.clip = emotes[0];
            happinessCount += extraHappy;
            speaker.Play();
            updateThousands();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            speaker.clip = emotes[1];
            happinessCount += happy;
            speaker.Play();
            updateThousands();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            speaker.clip = emotes[2];
            happinessCount += sad;
            speaker.Play();
            updateThousands();
        }

        happinessDisplay.text = "HAPPINESS: " + happinessCount;

        int depression = Mathf.RoundToInt(maxDepressor * depressorMultiplier);

        if (isVisible)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                happinessCount -= depression;
                timer = 0;

                thousands = (happinessCount - (happinessCount % 1000)) / 1000;

                updateThousands();
                StartCoroutine(testing());
            }
        }

        float test = 0f;

        //backgroundBar.GetComponent<Image>().fillAmount = (happinessCount % 1000f) / 1000f;
        //backgroundBar.GetComponent<Image>().fillAmount = Mathf.Lerp((happinessCount % 1000) / 1000f, 1, Time.deltaTime);
        backgroundBar.GetComponent<Image>().fillAmount = Mathf.SmoothDamp
            (backgroundBar.GetComponent<Image>().fillAmount, (happinessCount % 1000) / 1000f, ref test, 0.03f);

        if (backgroundBar.GetComponent<Image>().fillAmount > backgroundDepressLayer.GetComponent<Image>().fillAmount)
        {
            backgroundDepressLayer.GetComponent<Image>().fillAmount = backgroundBar.GetComponent<Image>().fillAmount;
            decreasing = false;
            StartCoroutine(buffer());
        }
    }

    private void FixedUpdate()
    {
        if (decreasing)
        {
            backgroundDepressLayer.GetComponent<Image>().fillAmount -= 0.0015f;
        }
    }

    public void updateThousands()
    {
        thousands = (happinessCount - (happinessCount % 1000)) / 1000;
        thousandTracker.text = "";
        for (int i = 0; i < thousands; i++)
        {
            thousandTracker.text += "•";
        }
    }

    public IEnumerator testing()
    {
        yield return new WaitForSeconds(1f);
        if (!decreasing && !buffering) decreasing = true;
    }

    public IEnumerator buffer()
    {
        buffering = true;
        yield return new WaitForSeconds(0.78695f);
        buffering = false;
    }
}
