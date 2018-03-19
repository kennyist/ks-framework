using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_SavingLoading;

public class TESTsave : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            KS_SaveLoad.Save("test");
        }
	}
}
