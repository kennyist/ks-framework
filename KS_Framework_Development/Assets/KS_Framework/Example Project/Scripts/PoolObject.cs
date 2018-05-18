using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Pooling;

public class PoolObject : KS_poolObject {

    public string poolTag = "Box";
    public int limit = 10;

    public float maxAliveTime = 3f;
    float aliveTime;
    float timer = 0f;

    // Use this for initialization
    private void OnEnable()
    {
        aliveTime = Random.Range(0.0f, maxAliveTime);
        timer = 0;
    }

    // Update is called once per frame
    void Update () {
		if(timer > aliveTime)
        {
            AddToPool();
        }
        else
        {
            timer += Time.deltaTime;
        }
	}

    public override PoolObjectSettings PoolSettings()
    {
        PoolObjectSettings settings = new PoolObjectSettings(poolTag, limit);

        return settings; 
    }
}
