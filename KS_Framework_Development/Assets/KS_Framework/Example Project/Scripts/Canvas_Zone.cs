using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Zone : MonoBehaviour {

    public GameObject CanvasElement;
    public string triggerTag = "Player";
    public bool stayEnabled = false;

    private void Start()
    {
        if (CanvasElement && !stayEnabled)
        {
            CanvasElement.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == triggerTag)
        {
            if (CanvasElement)
            {
                CanvasElement.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == triggerTag)
        {
            if (CanvasElement)
            {
                CanvasElement.SetActive(false);
            }
        }
    }
}
