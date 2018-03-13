using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLegendUI : MonoBehaviour {

    private bool mapEnabled = false;
    public GameObject LegendContainer;

    private bool filter = false;

	// Use this for initialization
	void Awake () {
        KS_FullMap.OffMiniMap += OnMap;
        KS_FullMap.OnMinimap += OffMap;
	}

    void OnMap()
    {
        mapEnabled = true;
    }

    void OffMap()
    {
        mapEnabled = false;
        LegendContainer.SetActive(false);
    }

    public void SetFilter()
    {
        Debug.Log("Setting filter");
        KS_FullMap.Instance.SetFilter(KS_MapMarker.MapMarkerType.Other, filter);
        filter = !filter;
    }

    void Legend()
    {
        LegendContainer.SetActive(!LegendContainer.activeSelf);
    }

    private void Update()
    {
        if (!mapEnabled) return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            Legend();
            Debug.Log("Legend");
        }
    }

}
