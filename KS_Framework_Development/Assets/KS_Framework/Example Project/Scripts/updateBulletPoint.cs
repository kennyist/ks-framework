using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateBulletPoint : MonoBehaviour {

    public Transform bulletPoint;
    public GameObject bullet;
    public float bulletSpeed = 50f;
    public List<GameObject> bullets = new List<GameObject>();

    public void Clear()
    {
        for (int j = 0; j < bullets.Count; j++)
        {
            Destroy(bullets[j]);
        }

        bullets.Clear();
    }

    public void Fire()
    {
        GameObject o = Instantiate(bullet);
        o.transform.position = bulletPoint.transform.position;
        o.GetComponent<Rigidbody>().AddForce(bulletPoint.transform.forward * bulletSpeed, ForceMode.Impulse);

        bullets.Add(o);
    }
}
