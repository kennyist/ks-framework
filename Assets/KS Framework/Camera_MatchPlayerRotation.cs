using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_MatchPlayerRotation : MonoBehaviour
{

    public Transform target;

    private void FixedUpdate()
    {
        transform.rotation = target.rotation;
    }

}