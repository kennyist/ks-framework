using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_MatchPlayerRotation : MonoBehaviour
{

    public Transform target;

    private void Start()
    {
        if(!target)
            target = Camera.main.transform;
    }


    private void LateUpdate()
    {
        if(target != null)
            transform.rotation = target.rotation;
        else
            target = Camera.main.transform;
    }

}