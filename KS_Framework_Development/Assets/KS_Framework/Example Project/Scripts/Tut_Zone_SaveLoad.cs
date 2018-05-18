using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;
using KS_Core;
using KS_Core.IO;
using KS_Core.Localisation;

public class Tut_Zone_SaveLoad : MonoBehaviour {

    public List<GameObject> objects = new List<GameObject>();

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.F) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Triangle)))
        {
            ResetObjects();
        }

        if (Input.GetKeyDown(KeyCode.Q) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Square)))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.E) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.X)))
        {
            Load();
        }

        if (Input.GetMouseButtonDown(0) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.R2)))
        {
            FindObjectOfType<updateBulletPoint>().Fire();
        }
    }

    private void OnEnable()
    {
        Clone();
    }

    private void OnDisable()
    {
        ResetObjects();
    }

    void Clone()
    {
        objects.Clear();

        foreach (SaveRestore obj in FindObjectsOfType<SaveRestore>())
        {
            GameObject o = Instantiate(obj.gameObject, obj.transform.position, obj.transform.rotation);
            o.SetActive(false);
            objects.Add(o);
        }
    }

    void ResetObjects()
    {
        SaveRestore[] robjects = GameObject.FindObjectsOfType<SaveRestore>();

        for (int i = 0; i < robjects.Length; i++)
        {
            if (robjects[i].gameObject.activeSelf)
            {
                Destroy(robjects[i].gameObject);
            }
        }

        FindObjectOfType<updateBulletPoint>().Clear();

        foreach(GameObject obj in objects)
        {
            GameObject o = Instantiate(obj.gameObject, obj.transform.position, obj.transform.rotation);
            o.SetActive(true);
        }

    }

    public void Save()
    {
        KS_Manager.Instance.SaveGame("tutSave");

        KS_Subtitle.Instance.ShowText("Saved: tutsave.sav", 2f);
    }

    public void Load()
    {
        clear();

        KS_SaveGame save = KS_SaveLoad.Load("tutSave");

        for(int i = 0; i < save.gameObjects.Count; i++)
        {
            KS_SaveLoad.RestoreGameObject(save.gameObjects[i]);
        }

        KS_Subtitle.Instance.ShowText("Loaded: tutsave.sav", 2f);
    }

    void clear()
    {
        KS_SaveableObject[] sobjects = GameObject.FindObjectsOfType<KS_SaveableObject>();

        Debug.Log("Found objects: " + sobjects.Length);

        for(int i = 0; i < sobjects.Length; i++)
        {
            Destroy(sobjects[i].gameObject);
        }
    }
}
