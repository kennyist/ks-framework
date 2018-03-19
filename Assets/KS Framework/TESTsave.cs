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
        string[] test = new string[] { "test string" };
        saveGame.Add("Test", test);
    }

    void OnLoad(KS_SaveGame save)
    {
        string[] test = (string[])save.SaveData["Test"];

        Debug.Log("Scene: " + save.SceneIndex + " - test: " + test[0]);

        foreach(KS_SaveObject so in save.gameObjects)
        {
            GameObject go = KS_SaveLoad.RestoreGameObject(so);
            go.SendMessage("OnLoad", save, SendMessageOptions.DontRequireReceiver);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            KS_SaveLoad.Save("test");
            clearlevel();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            KS_SaveLoad.Load("test");
        }
	}
}
