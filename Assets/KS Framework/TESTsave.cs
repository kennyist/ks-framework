using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_SavingLoading;

public class TESTsave : MonoBehaviour {

	// Use this for initialization
	void Start () {
        KS_SaveLoad.OnSave += OnSave;
        KS_SaveLoad.OnLoad += OnLoad;
	}

    void clearlevel()
    {
        KS_SaveableObject[] goa = GameObject.FindObjectsOfType<KS_SaveableObject>();

        foreach(KS_SaveableObject so in goa)
        {
            Destroy(so.gameObject);
        }
    }

    void OnSave(ref Dictionary<string, object> saveGame)
    {

    }

    void OnLoad(KS_SaveGame save)
    {

        Debug.Log("Scene: " + save.SceneIndex);
        Debug.Log("Objects: " + save.gameObjects.Count);

        for(int i = 0; i < save.gameObjects.Count; i++)
        {
            GameObject obj = KS_SaveLoad.RestoreGameObject(save.gameObjects[i]);

            if (obj)
                obj.SendMessage("OnLoad", save, SendMessageOptions.DontRequireReceiver);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            KS_SaveLoad.Save("test");
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            clearlevel();
            KS_SaveLoad.Load("test");
        }

	}
}
