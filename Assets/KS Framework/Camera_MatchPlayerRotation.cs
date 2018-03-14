using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_MatchPlayerRotation : MonoBehaviour
{

    Transform target;

    private void Start()
    {
        target = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if(target != null)
            transform.rotation = target.rotation;
    }

}