using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;

public class DS4InputTEst : MonoBehaviour {

    public float stickMoveAmmount = 20f;
    public GameObject leftStick;
    private Vector3 leftStickStartPos;
    public GameObject rightStick;
    private Vector3 rightStickStartPos;
    public GameObject l2;
    private Vector3 l2StartPos;
    public GameObject r2;
    private Vector3 r2StartPos;
    public GameObject dpad;
    private Vector3 dpadStartPos;

	// Use this for initialization
	void Start () {
        leftStickStartPos = leftStick.transform.position;
        rightStickStartPos = rightStick.transform.position;
        l2StartPos = l2.transform.position;
        r2StartPos = r2.transform.position;
        dpadStartPos = dpad.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (KS_Input.GetInputDown("fire"))
        {
            Debug.Log("DS4 got");
        }

        Vector3 leftNewLoc = leftStickStartPos;

        if (KS_Input.GetDS4Axis(DS4Axis.LeftStickX) != 0.0f)
        {
            leftNewLoc += -Vector3.left * (KS_Input.GetDS4Axis(DS4Axis.LeftStickX) * stickMoveAmmount);
        }

        if(KS_Input.GetDS4Axis(DS4Axis.LeftStickY) != 0.0f)
        {
            leftNewLoc += -Vector3.up * (KS_Input.GetDS4Axis(DS4Axis.LeftStickY) * stickMoveAmmount);
        }

        leftStick.transform.position = leftNewLoc;

        Vector3 rightNewLoc = rightStickStartPos;

        if (KS_Input.GetDS4Axis(DS4Axis.RightStickX) != 0.0f)
        {
            rightNewLoc += -Vector3.left * (KS_Input.GetDS4Axis(DS4Axis.RightStickX) * stickMoveAmmount);
        }

        if (KS_Input.GetDS4Axis(DS4Axis.RightStickY) != 0.0f)
        {
            rightNewLoc += -Vector3.up * (KS_Input.GetDS4Axis(DS4Axis.RightStickY) * stickMoveAmmount);
        }

        rightStick.transform.position = rightNewLoc;

        Vector3 l2NwLc = l2StartPos;

        if (KS_Input.GetDS4Axis(DS4Axis.L2) != 0)
        {
            l2NwLc += Vector3.up * (KS_Input.GetDS4Axis(DS4Axis.L2) * stickMoveAmmount);
        }

        l2.transform.position = l2NwLc;

        Vector3 r2NwLc = r2StartPos;

        if (KS_Input.GetDS4Axis(DS4Axis.R2) != 0)
        {
            r2NwLc += Vector3.up * (KS_Input.GetDS4Axis(DS4Axis.R2) * stickMoveAmmount);
        }

        r2.transform.position = r2NwLc;

        Vector3 nwdppos = dpadStartPos;

        if (KS_Input.GetDS4Axis(DS4Axis.DPadX) != 0)
        {
            nwdppos += -Vector3.left * (KS_Input.GetDS4Axis(DS4Axis.DPadX) * stickMoveAmmount);
        }

        if(KS_Input.GetDS4Axis(DS4Axis.DPadY) != 0)
        {
            nwdppos += Vector3.up * (KS_Input.GetDS4Axis(DS4Axis.DPadY) * stickMoveAmmount);
        }

        dpad.transform.position = nwdppos;
    }
}
