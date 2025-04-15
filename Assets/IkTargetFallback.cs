using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkTargetFallback : MonoBehaviour
{
    [SerializeField] GameObject Player;

    private float IK_Target_ZRotLimit;
    private float IK_Target_YRotLimit = 0; //since there isnt option to rotate wrist this will be left 0
    private float IK_Target_XRotMinLimit;
    private float IK_Target_XRotMaxLimit;

    public GameObject IK_Target;
    public bool IK_Target_Still_In_Range;
    public GameObject the_claw_jnt;

    private void Start()
    {
        if (Player != null)
        {
            IK_Target_ZRotLimit = Player.GetComponent<RigControls>().ikMaxRotationZ;
            IK_Target_XRotMinLimit = Player.GetComponent<RigControls>().ikMinRotationX;
            IK_Target_XRotMaxLimit = Player.GetComponent<RigControls>().ikMaxRotationX; 
        }

        if (IK_Target == null)
        {
            IK_Target = GameObject.Find("ArmIK_target");
        }
    }

    private void Update()
    {
        //transform.position = the_claw_jnt.transform.position;
        if(IK_Target.transform.localRotation.x > IK_Target_XRotMaxLimit) 
        {
            IK_Target.transform.localRotation = Quaternion.Euler(IK_Target_XRotMaxLimit, IK_Target.transform.localRotation.y, IK_Target.transform.localRotation.z); //apply the "clamped" x
        }
        else if (Mathf.Abs(IK_Target.transform.localRotation.x) < IK_Target_XRotMinLimit) 
        {
            IK_Target.transform.localRotation = Quaternion.Euler(IK_Target_XRotMinLimit, IK_Target.transform.localRotation.y, IK_Target.transform.localRotation.z); //apply the "clamped" x
        }

        /*
        //transform.position = the_claw_jnt.transform.position;
        if (Mathf.Abs(IK_Target.transform.localRotation.y) > 0) //this is abs to account for both - / + rots
        {
            //float tempY = IK_Target.transform.localRotation.y / IK_Target.transform.localRotation.y; // devideds the x by itself to reaquire - / +
            IK_Target.transform.localRotation = Quaternion.Euler(IK_Target.transform.localRotation.x, 0, IK_Target.transform.localRotation.z); //apply the "clamped" y
        }*/

        if (Mathf.Abs(IK_Target.transform.localRotation.z) > IK_Target_ZRotLimit) //this is abs to account for both - / + rots
        {
            float tempZ = IK_Target.transform.localRotation.z / IK_Target.transform.localRotation.z; // devideds the x by itself to reaquire - / +
            IK_Target.transform.localRotation = Quaternion.Euler(IK_Target.transform.localRotation.x, IK_Target.transform.localRotation.y, IK_Target_ZRotLimit * tempZ); //apply the "clamped" x
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Detected: " + other);
        if(other.gameObject == IK_Target)
        {
            //Debug.Log("Object Is IK: " + other);
            IK_Target_Still_In_Range = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Object Left: " + other);
        if (other.gameObject == IK_Target)
        {
            //Debug.Log("Object Was IK: " + other);
            IK_Target_Still_In_Range = false;
        }
    }
}
