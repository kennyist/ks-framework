using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRestore : MonoBehaviour {

    public Vector3 startPos;
    public Quaternion startRot;

	// Use this for initialization
	public void Save() {
        startPos = transform.position;
        startRot = transform.rotation;
	}

    public void Restore()
    {
        transform.rotation = startRot;
        transform.position = startPos;
    }
}
