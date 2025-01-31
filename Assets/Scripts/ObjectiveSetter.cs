using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveSetter : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    // Start is called before the first frame update
    void Start()
    {
        setObjective("Sort Blocks");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setObjective(string objective)
    {
        objectiveText.text = "Goal: " + objective;
    }
}
