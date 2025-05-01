using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class IkTargetFallback : MonoBehaviour
{
    [SerializeField] GameObject Player;

    private float IK_Target_ZRotLimit;
    private float IK_Target_YRotLimit = 0; //since there isnt option to rotate wrist this will be left 0
    private float IK_Target_XRotMinLimit;
    private float IK_Target_XRotMaxLimit;

    //public TMP_Text IK_Debug_X;
    //public TMP_Text IK_Debug_Y;
    //public TMP_Text IK_Debug_Z;

    public GameObject VisualDebug;

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

        /*
        if (IK_Debug_X == null || IK_Debug_Y == null || IK_Debug_Z == null) { return; }
            IK_Debug_X.text = "X: " + IK_Target.transform.eulerAngles.x + " " + IK_Target.transform.rotation.x;
            IK_Debug_Y.text = "Y: " + IK_Target.transform.eulerAngles.y + " " + IK_Target.transform.rotation.y;
            IK_Debug_Z.text = "Z: " + IK_Target.transform.eulerAngles.z + " " + IK_Target.transform.rotation.z;
        if (VisualDebug == null) { return; }
            VisualDebug.transform.rotation = IK_Target.transform.rotation;*/
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
