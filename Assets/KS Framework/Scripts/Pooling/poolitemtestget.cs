using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolitemtestget : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameObject o = KS_PoolManager.Instance.Get("test_item").GameObject;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject o = (GameObject)new GameObject();
            poolItemTest t = o.AddComponent<poolItemTest>();
            t.settings = new PoolObjectSettings("test_item", 3);
        }
    }
}
