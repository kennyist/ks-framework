using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;
using KS_Core.Pooling;
using UnityEngine.UI;

public class Tut_Zone_Pooling : MonoBehaviour {

    public GameObject[] spawnObjects;
    public Transform[] spawnPoints;
    KS_PoolManager pool;
    public Text wallText;

	// Use this for initialization
	void OnEnable () {
        pool = KS_PoolManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Square)))
        {
            Spawn();
        }

        if (Input.GetKeyDown(KeyCode.F) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Triangle)))
        {
            ClearPool();
        }

        string text = "";

        for(int i = 0; i < spawnObjects.Length; i++)
        {
            KS_IPoolObject o = spawnObjects[i].GetComponent<KS_IPoolObject>();

            text += o.PoolSettings().tag + ": Active in scene ("+ GetAmountOfType(o.PoolSettings().tag)
                +") ---- In pool ("+ pool.GetAmountOfType(o.PoolSettings().tag) +")\n";
        }

        wallText.text = text;
    }

    public int GetAmountOfType(string tag)
    {
        int count = 0;

        PoolObject[] all = GameObject.FindObjectsOfType<PoolObject>() as PoolObject[];

        for(int i = 0; i < all.Length; i++)
        {
            if (all[i].gameObject.activeInHierarchy)
            {
                if(all[i].PoolSettings().tag == tag)
                {
                    count++;
                }
            }
        }

        return count;
    }

    void Spawn()
    {
        int i = Random.Range(0, spawnObjects.Length);
        int p = Random.Range(0, spawnPoints.Length);

        string tag = spawnObjects[i].GetComponent<KS_IPoolObject>().PoolSettings().tag;

        KS_IPoolObject o = pool.Get(tag);
        GameObject obj;

        if(o != null)
        {
            obj = o.GameObject;
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(spawnObjects[i]);
        }

        obj.transform.position = spawnPoints[p].position;
        obj.transform.rotation = Random.rotation;
    }

    void ClearPool()
    {
        pool.Clear();
    }
}
