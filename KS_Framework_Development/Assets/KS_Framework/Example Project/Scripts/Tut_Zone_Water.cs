using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;

public class Tut_Zone_Water : MonoBehaviour {

    public Spawner spawner;
    public Spawner spawner2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Q) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Square)))
        {
            Spawn();
        }	

        if(Input.GetKeyDown(KeyCode.E) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Triangle)))
        {
            clear();
        }
	}

    void Spawn()
    {
        spawner.spawn();
        spawner2.spawn();
    }

    void clear()
    {
        spawner.Clear();
        spawner2.Clear();
    }
}
