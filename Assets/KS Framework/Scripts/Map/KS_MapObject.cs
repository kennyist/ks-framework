using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_MapObject : MonoBehaviour {

    public enum MapItemType
    {
        Settlement,
        chest,

    }

    public string displayName = "Place/item";
    public MapItemType type;
    public Texture2D mapIcon;
    [Range(0.1f, 99.9f)]
    public float dontShowAfterMapZoomPercent = 99.9f;
    public float mapIconSize = 2f;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
