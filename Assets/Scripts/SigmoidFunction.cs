using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigmoidFunction : MonoBehaviour
{
    [Tooltip("Amount of seconds before happiness starts depleting.")]
    public int buffer;

    [Tooltip("Amount of seconds to reach the highest point in the sigmoid curve.")]
    public int timeFrame;

    public AnimationCurve sigmoidCurve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
