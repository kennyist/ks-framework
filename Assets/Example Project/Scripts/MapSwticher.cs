using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;

public class MapSwticher : MonoBehaviour {

    public Camera playerCam;
    private KS_FullMap map;
    private bool mapActive = false;

	// Use this for initialization
	void Start () {
        map = KS_FullMap.Instance;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.M) || KS_Input.GetInputDown("map"))
        {
            if (!mapActive)
            {
                playerCam.enabled = false;
                map.ActivateMap();
                mapActive = true;
            } 
            else
            {
                mapActive = false;
                map.DeactivateMap();
                playerCam.enabled = true;
            }
        }
	}
}
