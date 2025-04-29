using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCup : MonoBehaviour
{
    public Material paintBrushAlteredMat;
    public GameObject paintBrush;

    public GameObject sphereFlag;
    public GameObject squareFlag;

    private PaintCupDetection sphereDetection;
    private PaintCupDetection squareDetection;

    private bool brushDetectedAndAccepted = false;

    private void Awake()
    {
        if ((sphereFlag == null || sphereFlag.GetComponent<PaintCupDetection>() == null) ||
            (squareFlag == null || squareFlag.GetComponent<PaintCupDetection>() == null)) { return; }
        else
        {
            sphereDetection = sphereFlag.GetComponent<PaintCupDetection>();
            squareDetection = squareFlag.GetComponent<PaintCupDetection>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(sphereDetection.brushInCollider && squareDetection.brushInCollider)
        {
            paintBrush.GetComponent<Renderer>().material = paintBrushAlteredMat;
        }
    }
}
