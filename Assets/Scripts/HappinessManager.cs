using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessManager : MonoBehaviour
{
    //The higher the position, the worse the state
    [Header("Debug")]
    public int extraHappy;
    public int happy;
    public int sad;
    public bool isVisible;

    [Header("Level Designers")]
    [Tooltip("The amount of happiness the level starts off with.")]
    public int happinessCount;
    [Tooltip("The maximum amount of happiness lost after the sigmoid curve runs its course.")]
    public int maxDepressor;
    [Tooltip("The minimum amount of happiness required to gain for the 'extra happy' sound to play.")]
    public int extraExcitedThreshold;

    [Header("Misc References")]
    public List<AudioClip> emotes;
    public AudioSource speaker;
    public TextMeshProUGUI happinessDisplay;

    public Image backgroundBar;
    public Image backgroundDepressLayer;
    public TextMeshProUGUI thousandTracker;
    int thousands;

    bool decreasing;
    public bool buffering;

    public SigmoidFunction sigmoidFunction;

    bool startedFunction = false;

    public bool viewingTutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        // Els: Modified 04/18/2025
        // removed search for sigmoid function. SigmoidFunction.cs is now a non-monbehavior-derived purely digital
        // function. The value of variable sigmoidFunction is set from within GameManager.cs

        backgroundDepressLayer.GetComponent<Image>().fillAmount = backgroundBar.GetComponent<Image>().fillAmount;
        updateThousands();
    }

    private float depressionFactor;
    private float depressionTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //maxHappy();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //normalHappy();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //getDepressed();
        }

        happinessDisplay.text = "HAPPINESS: " + happinessCount;

        //int depression = maxDepressor;
        
        if (isVisible && !startedFunction)
        {
            callCoroutine();



            /*
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                happinessCount -= depression;
                timer = 0;

                thousands = (happinessCount - (happinessCount % 1000)) / 1000;

                updateThousands();
                StartCoroutine(testing());
            }
            */
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

    

    public void callCoroutine()
    {
        if (viewingTutorial)
        {
            return;
        }
        StartCoroutine(artificialDelay());
        startedFunction = true;
    }

    
    public IEnumerator artificialDelay()
    {
        yield return new WaitForSeconds(sigmoidFunction.buffer);
        StartCoroutine(startSigmoid());
        StartCoroutine(depression());
    }


    float sigmoidTime = 0;

    public float sigmoidMultiplier = 0;

    private IEnumerator startSigmoid()
    {
        sigmoidTime += Time.deltaTime;
        // Els: removed unnecessary curve evaluation 04/18/2025
        // sigmoidFunction.sigmoidCurve.Evaluate(sigmoidTime);
        //Debug.Log(sigmoidFunction.sigmoidCurve.Evaluate(sigmoidTime));

        sigmoidMultiplier = sigmoidFunction.sigmoidCurve.Evaluate(sigmoidTime);
        //Debug.Log($"Curve Value at time {sigmoidTime}: {sigmoidMultiplier}");

        yield return new WaitForSeconds(Time.deltaTime);

        if (sigmoidTime < sigmoidFunction.timeFrame) StartCoroutine(startSigmoid());
    }

    public float timeBetweenHappinessLoss;

    public IEnumerator depression()
    {
        int depressionAmount = Mathf.RoundToInt(maxDepressor * sigmoidMultiplier);
        happinessCount -= depressionAmount;
        // Debug.Log("Happiness Lost: " + depressionAmount);
        StartCoroutine(testing());
        updateThousands();
        yield return new WaitForSeconds(timeBetweenHappinessLoss);
        StartCoroutine(depression());
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

    public void gainHappiness(int happinessToGain)
    {
        happinessCount += happinessToGain;

        if (happinessToGain <= 0)
        {
            happinessCount -= happinessToGain;
            return;
        }
        else if (emotes.Count > 0)
        {
            if (happinessToGain >= extraExcitedThreshold)
            {
                // Els: Modified 04/18/025
                // This is now how we'll be handling sound. 
                // speaker.clip = emotes[0];
            }
            else
            {
                // speaker.clip = emotes[1];
            }
        }

        // Els: Modified 04/18/025
        // This is now how we'll be handling sound. 
        // speaker.Play();
        updateThousands();
    }

    public void loseHappiness(int happinessToLose)
    {
        happinessCount -= happinessToLose;

        if (happinessToLose >= 0)
        {
            happinessCount += happinessToLose;
            return;
        }
        else if (emotes.Count >= 3) speaker.clip = emotes[2];
        // speaker.Play();
        updateThousands();
    }


    /*
    public void maxHappy()
    {
        speaker.clip = emotes[0];
        happinessCount += extraHappy;
        speaker.Play();
        updateThousands();
    }

    public void normalHappy()
    {
        speaker.clip = emotes[1];
        happinessCount += happy;
        speaker.Play();
        updateThousands();
    }

    public void getDepressed()
    {
        speaker.clip = emotes[2];
        happinessCount += sad;
        speaker.Play();
        updateThousands();
    }
    */
}
