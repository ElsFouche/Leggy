using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessManager : MonoBehaviour
{
    //The higher the position, the worse the state
    public List<AudioClip> emotes;
    public AudioSource speaker;
    public TextMeshProUGUI happinessDisplay;
    public int happinessCount;
    public int maxDepressor;
    public float depressorMultiplier;
    float timer;

    public bool isVisible;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            speaker.clip = emotes[0];
            happinessCount += 100;
            speaker.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            speaker.clip = emotes[1];
            happinessCount += 55;
            speaker.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            speaker.clip = emotes[2];
            happinessCount -= 50;
            speaker.Play();
        }

        happinessDisplay.text = "HAPPINESS: " + happinessCount;

        if (isVisible)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                happinessCount -= Mathf.RoundToInt(maxDepressor * depressorMultiplier);
                timer = 0;
            }
        }
    }
}
