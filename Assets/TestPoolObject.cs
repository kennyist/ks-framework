using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoolObject : KS_poolObject {

    public float aliveTime = 3f;
    private float timer;

    private void OnEnable()
    {
        timer = aliveTime;   
    }

    private void OnDisable()
    {
    }

    public override void AddToPool()
    {
        base.AddToPool();
    }

    public override PoolObjectSettings PoolSettings()
    {
        PoolObjectSettings settings = new PoolObjectSettings("TestObj", 5);
        return settings;
    }

    // Update is called once per frame
    void Update () {
		if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            AddToPool();
        }
	}


}
