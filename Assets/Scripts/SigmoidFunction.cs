using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// Els: modified this script heavily 04/18/2025 in order to reduce
/// complexity and improve ease of use. 

/// <summary>
/// This script serves to provide access to an animation curve. 
/// </summary>
public class SigmoidFunction
{
    [Tooltip("Amount of seconds before happiness starts depleting.")]
    public int buffer;

    [Tooltip("Amount of seconds to reach the highest point in the sigmoid curve.")]
    public int timeFrame;

    public AnimationCurve sigmoidCurve;

    // Default constructor
    public SigmoidFunction()
    {

    }

    // Overloaded constructor accepting variable buffer and time frame
    public SigmoidFunction(int inBuffer, int  inTimeFrame)
    {
        buffer = inBuffer;
        timeFrame = inTimeFrame;
        sigmoidCurve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(timeFrame, 1, 0, 0));
    }

    // Overloaded constructor accepting any key frames. 
    public SigmoidFunction(Keyframe startFrame, Keyframe endFrame)
    {
        sigmoidCurve = new AnimationCurve(startFrame, endFrame);
    }

    public SigmoidFunction(int inBuffer, int inTimeFrame, Keyframe startFrame, Keyframe endFrame)
    {
        buffer = inBuffer;
        timeFrame = inTimeFrame;
        sigmoidCurve = new AnimationCurve(startFrame, endFrame);
    }

    public void SetCurveLength(int inTimeFrame)
    {
        timeFrame = inTimeFrame;
        int endFrame = sigmoidCurve.keys.Length - 1;
        sigmoidCurve.keys[endFrame] = new Keyframe(timeFrame, 
            sigmoidCurve.keys[endFrame].value,
            sigmoidCurve.keys[endFrame].inTangent,
            sigmoidCurve.keys[endFrame].outTangent);
    }
}
