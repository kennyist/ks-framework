using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPoolTest : KS_Behaviour {

    public GameObject toShoot;
    public float shootSpeed = 40f;
    public bool playing = false;

    public override void OnPlay()
    {
        playing = true;
    }

    public override void OnPause()
    {
        playing = false;
    }

    // Update is called once per frame
    void Update () {
        if (playing)
        {
            if (KS_Input.GetInputDown("fire"))
            {
                KS_IPoolObject find = KS_PoolManager.Instance.Get("TestObj");

                if(find == null)
                {
                    GameObject obj = GameObject.Instantiate(toShoot);
                    obj.transform.position = transform.position;
                    obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * shootSpeed);
                }
                else
                {
                    find.GameObject.transform.position = transform.position;
                    find.GameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * shootSpeed);
                }
            }

            if (Input.GetKeyDown("space"))
            {
                Debug.Log("Space pressed");
            }
        }
	}
}
