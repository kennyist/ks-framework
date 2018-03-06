using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KS_FullMap : MonoBehaviour {

    [Header("Map Area (World Coordinates")]
    public Vector2 bottomLeft;
    public Vector2 topRight;

    public List<GameObject> mapObjects = new List<GameObject>();

    public GameObject player;
    public GameObject camera;

    public GameObject mapContainer;
    public GameObject prefab;

    public void RegisterObject(GameObject obj)
    {
        mapObjects.Add(obj);
    }

    void OnScroll(Vector2 position)
    {
        /*
        Vector3 newPos = new Vector3();

        Vector2 mapSize = GetMapSize();

        newPos.x = (mapSize.x - Screen.width) * position.x;
        newPos.z = (mapSize.y - Screen.height) * position.y;

        newPos.x += Screen.width / 2;
        newPos.y = 680;
        newPos.z += Screen.height / 2;
  


        camera.transform.position = newPos;
        */

        Vector3 newPos = new Vector3();

        Vector2 mapSize = GetMapSize();

        newPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, Screen.height / 2));

        newPos.x += mapSize.x * position.x;
        newPos.y = 600;
        newPos.z += mapSize.y * position.y;

        camera.transform.position = newPos;
        

    }

	// Use this for initialization
	void Start ()
    {
        mapContainer.GetComponent<RectTransform>().sizeDelta = GetMapSize();

        GetComponentInChildren<ScrollRect>().onValueChanged.AddListener(OnScroll);

        //

        camera.transform.position = new Vector3(0, 50, 0);

        //
        GameObject o = new GameObject();
        o.transform.position = new Vector3(200, 0, 845);

        RegisterObject(o);

        DrawObjs();
	}

    Vector2 GetMapSize()
    {
        Vector2 size = new Vector2();

        // Width 

        if(bottomLeft.x < 0 && topRight.x < 0)
        {
            size.x = Mathf.Abs(bottomLeft.x) - Mathf.Abs(topRight.x);
        }
        else if(bottomLeft.x < 0)
        {
            size.x = Mathf.Abs(bottomLeft.x) + topRight.x;
        }
        else
        {
            size.x = topRight.x - bottomLeft.x;
        }

        // height

        if(bottomLeft.y < 0 && topRight.y < 0)
        {
            size.y = Mathf.Abs(bottomLeft.y) - Mathf.Abs(topRight.y);
        }
        else if(bottomLeft.y < 0)
        {
            size.y = Mathf.Abs(bottomLeft.y) + topRight.y;
        }
        else
        {
            size.y = topRight.y - bottomLeft.y;
        }

        return size;
    }


    Vector2 WorldToMapCoords(Vector3 position)
    {
        Vector2 mapPos = new Vector2();

        // x

        mapPos.x = topRight.x - position.x;

        // y

        mapPos.y = topRight.y - position.z;



        return mapPos;
    }

    void DrawObjs()
    {
        Vector2 pos = WorldToMapCoords(mapObjects[0].transform.position);

        GameObject obj = GameObject.Instantiate(prefab, mapContainer.transform);
        obj.GetComponent<RectTransform>().position = pos;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
