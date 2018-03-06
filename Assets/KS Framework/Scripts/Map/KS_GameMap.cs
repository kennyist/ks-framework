using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_GameMap : MonoBehaviour {

    public bool enableMiniMap = true;
    public bool enableFullMap = true;

    public Camera mapCamera;
    public GameObject player;

    [Header("Minimap")]
    public float miniMapCameraHeight = 50.0f;
    public bool orientToPlayer = true;

    [Header("Full Map")]
    public float minCameraHeight = 75.0f;
    public float maxCameraHeight = 150.0f;







    private void Awake()
    {
        Vector3 pos = player.transform.position;
        Quaternion camRot = Quaternion.Euler(90, 0, 0);
        pos.y = miniMapCameraHeight;
        mapCamera.transform.position = pos;
        mapCamera.transform.rotation = camRot;
    }

    private void FixedUpdate()
    {
        Vector3 mapPos = player.transform.position;
        mapPos.y = miniMapCameraHeight;
        mapCamera.transform.position = mapPos;

        /*if (orientToPlayer)
        {
            Vector3 mapRot = new Vector3(0, 0, player.transform.rotation.z);
            mapCamera.transform.rotation = Quaternion.Euler(mapRot);
        }*/
    }

    public void RegisterIcon()
    {

    }
}
