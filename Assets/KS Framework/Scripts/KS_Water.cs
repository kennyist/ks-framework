using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Water : MonoBehaviour {

    private Collider collider;

	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            KS_CharacterController controller = other.GetComponent<KS_CharacterController>();
            controller.IsSwimming = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        KS_CharacterController controller = other.GetComponent<KS_CharacterController>();

        if(controller && controller.IsSwimming)
        {
            Transform position = other.transform;

            if (other.transform.position.y == (collider.bounds.max.y - 0.5f))
            {
                other.GetComponent<Rigidbody>().AddForce(-Physics.gravity * 1.2f, ForceMode.Force);
            }
            else if (other.transform.position.y < (collider.bounds.max.y - 0.5f))
            {
                other.GetComponent<Rigidbody>().AddForce(-Physics.gravity * 1.2f, ForceMode.Force);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        KS_CharacterController controller = other.GetComponent<KS_CharacterController>();

        if (controller && controller.IsSwimming)
        {
            controller.IsSwimming = false;
        }
    }
}
