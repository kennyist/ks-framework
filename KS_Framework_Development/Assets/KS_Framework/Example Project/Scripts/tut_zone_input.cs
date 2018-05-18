using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;
using KS_Character;

public class tut_zone_input : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Triangle))){
            if (FindObjectOfType<KS_CharacterController>().enabled)
            {
                FindObjectOfType<KS_CharacterController>().enabled = false;
            }
            else
            {
                FindObjectOfType<KS_CharacterController>().enabled = true;
            }
        }
    }
}
