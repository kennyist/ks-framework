using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Atmosphere
{

    public class KS_Water : MonoBehaviour
    {

        private Collider collider;

        // Use this for initialization
        void Start()
        {
            collider = GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Player")
            {
                KS_CharacterController controller = other.GetComponent<KS_CharacterController>();
                controller.IsSwimming = true;
            }
            else if (other.GetComponent<KS_Buoyancy>())
            {
                other.GetComponent<KS_Buoyancy>().EnterWater(collider.bounds.max.y);
            }


        }

        private void OnTriggerStay(Collider other)
        {
            KS_CharacterController controller = other.GetComponent<KS_CharacterController>();

            if (controller && controller.IsSwimming)
            {
                Transform position = other.transform;
                Rigidbody rb = other.GetComponent<Rigidbody>();

                if (other.transform.position.y == (collider.bounds.max.y - 0.5f))
                {
                    other.GetComponent<Rigidbody>().AddForce(-Physics.gravity * rb.mass, ForceMode.Force);
                }
                else if (other.transform.position.y < (collider.bounds.max.y - 0.5f))
                {
                    other.GetComponent<Rigidbody>().AddForce(-Physics.gravity * 1.2f * rb.mass, ForceMode.Force);
                }
                else if (other.transform.position.y < collider.bounds.max.y - 2f)
                {
                    other.GetComponent<Rigidbody>().AddForce(-Physics.gravity * 1.3f * rb.mass, ForceMode.Force);
                    controller.state = KS_CharacterController.PlayerState.underwater;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            KS_CharacterController controller = other.GetComponent<KS_CharacterController>();

            if (controller && controller.IsSwimming)
            {
                controller.IsSwimming = false;
                controller.state = KS_CharacterController.PlayerState.walking;
            }
            else if (other.GetComponent<KS_Buoyancy>())
            {
                other.GetComponent<KS_Buoyancy>().LeaveWater();
            }
        }
    }
}