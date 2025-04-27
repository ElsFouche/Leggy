using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggyAudioHandler : MonoBehaviour
{
    // Private
        // Leggy Arm
    private StudioEventEmitter _LeggyArmDepth;
    private StudioEventEmitter _LeggyArmHeight;
    private StudioEventEmitter _LeggyArmLockout;
        // Leggy Hand
    private StudioEventEmitter _LeggyClaw;
    private StudioEventEmitter _LeggyClawLockout;
    private StudioEventEmitter _LeggyWrist;
        // Leggy Gantry
    private StudioEventEmitter _LeggyGantry;
    private StudioEventEmitter _LeggyGantryLockout;
        // Leggy Body
    private StudioEventEmitter _LeggyRotation;
    private StudioEventEmitter _LeggyRotationLockout;

    void Start()
    {
        // Init
            // Leggy Arm
        _LeggyArmDepth = gameObject.AddComponent<StudioEventEmitter>();
        _LeggyArmHeight = gameObject.AddComponent<StudioEventEmitter>();
        _LeggyArmLockout = gameObject.AddComponent<StudioEventEmitter>();
            // Leggy Hand
        _LeggyClaw = gameObject.AddComponent<StudioEventEmitter>();
        _LeggyClawLockout = gameObject.AddComponent<StudioEventEmitter>(); 
        _LeggyWrist = gameObject.AddComponent<StudioEventEmitter>();
            // Leggy Gantry
        _LeggyGantry = gameObject.AddComponent<StudioEventEmitter>();
        _LeggyGantryLockout = gameObject.AddComponent<StudioEventEmitter>();
            // Leggy Body
        _LeggyRotation = gameObject.AddComponent<StudioEventEmitter>();
        _LeggyRotationLockout = gameObject.AddComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
