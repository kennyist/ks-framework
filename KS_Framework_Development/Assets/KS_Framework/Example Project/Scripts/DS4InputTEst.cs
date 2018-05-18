using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;

public class DS4InputTEst : MonoBehaviour
{

    [System.Serializable]
    public class DS4StickTrigger
    {
        public GameObject item;
        public Vector3 startPos;
        public float moveAmmount = 10f;
    }

    public Color clickColour;
    public Color defaultCol;

    public GameObject shareB;
    public GameObject touchpad;
    public GameObject options;
    public GameObject triangle;
    public GameObject circle;
    public GameObject x;
    public GameObject square;
    public GameObject home;
    public GameObject dpadUp;
    public GameObject dPadDown;
    public GameObject dPadLeft;
    public GameObject dPadRight;
    public DS4StickTrigger leftStick;
    public DS4StickTrigger rightStick;
    public DS4StickTrigger leftTrigger;
    public DS4StickTrigger rightTrigger;
    public GameObject rightButton;
    public GameObject leftButton;

    // Use this for initialization
    void Start()
    {
        leftStick.startPos = leftStick.item.transform.position;
        rightStick.startPos = rightStick.item.transform.position;
        leftTrigger.startPos = leftTrigger.item.transform.position;
        rightTrigger.startPos = rightTrigger.item.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.Share)))
        {
            shareB.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            shareB.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.TouchPad)))
        {
            touchpad.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            touchpad.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.Options)))
        {
            options.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            options.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.Triangle)))
        {
            triangle.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            triangle.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.Circle)))
        {
            circle.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            circle.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.X)))
        {
            x.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            x.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.Square)))
        {
            square.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            square.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.Home)))
        {
            home.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            home.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.L1)))
        {
            leftButton.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            leftButton.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.L3)))
        {
            leftStick.item.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            leftStick.item.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.R1)))
        {
            rightButton.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            rightButton.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (Input.GetKey(KS_Input.DS4ButtonToKey(DS4KeyCode.R3)))
        {
            rightStick.item.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            rightStick.item.GetComponent<Renderer>().material.color = defaultCol;
        }

        // sticks


        Vector3 lsPos = leftStick.startPos;
        lsPos.x += leftStick.moveAmmount * -KS_Input.GetDS4Axis(DS4Axis.LeftStickX);
        lsPos.y += leftStick.moveAmmount * KS_Input.GetDS4Axis(DS4Axis.LeftStickY);

        leftStick.item.transform.position = lsPos;

        Vector3 rsPos = rightStick.startPos;
        rsPos.x += rightStick.moveAmmount * KS_Input.GetDS4Axis(DS4Axis.RightStickX);
        rsPos.y += rightStick.moveAmmount * -KS_Input.GetDS4Axis(DS4Axis.RightStickY);

        rightStick.item.transform.position = rsPos;
        
        Vector3 ltPos = leftTrigger.startPos;

        ltPos.y += leftTrigger.moveAmmount * KS_Input.GetDS4Axis(DS4Axis.L2);

        leftTrigger.item.transform.position = ltPos;

        Vector3 rtPos = rightTrigger.startPos;
        rtPos.y += rightTrigger.moveAmmount * KS_Input.GetDS4Axis(DS4Axis.R2);

        rightTrigger.item.transform.position = rtPos;

        // dpad

        if (KS_Input.GetDS4Axis(DS4Axis.DPadX) < -0.9)
        {
            dPadLeft.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            dPadLeft.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (KS_Input.GetDS4Axis(DS4Axis.DPadX) > 0.9)
        {
            dPadRight.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            dPadRight.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (KS_Input.GetDS4Axis(DS4Axis.DPadY) < -0.9)
        {
            dPadDown.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            dPadDown.GetComponent<Renderer>().material.color = defaultCol;
        }

        if (KS_Input.GetDS4Axis(DS4Axis.DPadY) > 0.9)
        {
            dpadUp.GetComponent<Renderer>().material.color = clickColour;
        }
        else
        {
            dpadUp.GetComponent<Renderer>().material.color = defaultCol;
        }
    }
}
