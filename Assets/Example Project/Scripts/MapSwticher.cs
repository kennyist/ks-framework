using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;

public class MapSwticher : MonoBehaviour {

    public Camera playerCam;
    private KS_Mapping map;
    private bool mapActive = false;

	// Use this for initialization
	void Start () {
        map = KS_Mapping.Instance;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.M) || KS_Input.GetInputDown("map"))
        {
            if (!mapActive)
            {
                map.DissableMinimap();
                playerCam.enabled = false;
                map.ShowMap();
                mapActive = true;
            } 
            else
            {
                mapActive = false;
                map.HideMap();
                map.EnableMinimap();
                playerCam.enabled = true;
            }
        }
	}
}
