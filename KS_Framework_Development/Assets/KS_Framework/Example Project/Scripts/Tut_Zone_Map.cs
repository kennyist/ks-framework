using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Mapping;
using KS_Core.Input;

public class Tut_Zone_Map : MonoBehaviour {

    KS_Map map;
    public GameObject mapContainer;

	// Use this for initialization
	void OnEnable () {
        map = KS_Map.Instance;
        map.useMiniMap = true;
        map.EnableMinimap();
        mapContainer.SetActive(true);

        FindObjectOfType<MapSwticher>().enabled = true;
	}

    private void OnDisable()
    {
        map.useMiniMap = false;
        map.DisableMinimap();
        mapContainer.SetActive(false);

        FindObjectOfType<MapSwticher>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.N) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Triangle)))
        {
            map.useMiniMap = !map.useMiniMap;

            if (map.MiniMapActive && !map.useMiniMap && !map.MapActive)
            {
                map.DisableMinimap();
                mapContainer.SetActive(false);
            }
            if (!map.MapActive && map.useMiniMap)
            {
                map.EnableMinimap();
                mapContainer.SetActive(true);
            }
        }
	}
}
