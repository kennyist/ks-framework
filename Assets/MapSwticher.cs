using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MapSwticher : MonoBehaviour {

    public FirstPersonController player;
    public Camera playerCam;
    private KS_FullMap map;
    private bool mapActive = false;

	// Use this for initialization
	void Start () {
        map = KS_FullMap.Instance;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!mapActive)
            {
                playerCam.enabled = false;
                player.enabled = false;
                map.ActivateMap();
                mapActive = true;
            } 
            else
            {
                mapActive = false;
                map.DeactivateMap();
                player.enabled = true;
                playerCam.enabled = true;
            }
        }
	}
}
