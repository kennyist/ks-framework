using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_SaveableObject : MonoBehaviour {

    public string prefab;
    private string id;
    private string parentID;
    public string ID { get { return id; } set { id = value; } }
    public string ParentID { get { return parentID; } set { parentID = value; } }

	// Use this for initialization
	public void GetID () {
        id = System.Guid.NewGuid().ToString();
        GetParents();
	}
	
	// Update is called once per frame
	void GetParents () {
        if (transform.parent == null)
        {
            parentID = null;
        }
        else
        {
            KS_SaveableObject[] childrenIds = GetComponentsInChildren<KS_SaveableObject>();
            foreach (KS_SaveableObject idScript in childrenIds)
            {
                if (idScript.transform.gameObject != gameObject)
                {
                    idScript.parentID = id;
                    idScript.GetID();
                }
            }
        }
    }
}
