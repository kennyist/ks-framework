using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] spawnObjects;
    public float spawnOffsetX = 8f;
    public float spawnOffsetY = 10f;

    private List<GameObject> spawnedObjects;

    private void Start()
    {
        spawnedObjects = new List<GameObject>();
    }

    public void spawn()
    {
        if (spawnObjects.Length <= 0) return;

        int i = Random.Range(0, spawnObjects.Length);

        Vector3 position = transform.position;

        position.x += Random.Range(-spawnOffsetX, spawnOffsetX);
        position.z += Random.Range(-spawnOffsetX, spawnOffsetX);
        position.y += Random.Range(-spawnOffsetY, spawnOffsetY);

        GameObject o = Instantiate(spawnObjects[i], position, Random.rotation);
        spawnedObjects.Add(o);
    }

    public void Clear()
    {
        for(int i = 0; i < spawnedObjects.Count; i++)
        {
            Destroy(spawnedObjects[i]);
        }

        spawnedObjects.Clear();
    }
}
