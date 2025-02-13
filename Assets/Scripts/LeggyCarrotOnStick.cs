using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggyCarrotOnStick : MonoBehaviour
{
    public GameObject LeggyClawCenter;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = LeggyClawCenter.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
