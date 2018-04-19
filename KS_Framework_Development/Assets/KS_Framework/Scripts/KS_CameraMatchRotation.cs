using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Utility
{
    public class KS_CameraMatchRotation : MonoBehaviour
    {

        public Transform target;

        private void Start()
        {
            if (!target)
                target = Camera.main.transform;
        }


        private void LateUpdate()
        {
            if (target != null)
                transform.rotation = target.rotation;
            else
                target = Camera.main.transform;
        }

    }
}