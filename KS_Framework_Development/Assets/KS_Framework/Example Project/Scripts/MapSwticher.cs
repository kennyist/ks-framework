using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;
using KS_Core;
using KS_Mapping;

public class MapSwticher : KS_Behaviour {

    public Camera playerCam;
    private KS_Map map;
    private bool mapActive = false;

	// Use this for initialization
	void Start () {
        map = KS_Map.Instance;
	}
	
	// Update is called once per frame
	void Update () {

        if ((Input.GetKeyDown(KeyCode.M) || KS_Input.GetInputDown("map")) && Manager.State == KS_Manager.GameState.Playing)
        {
            if (!mapActive)
            {
                map.DisableMinimap();
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
