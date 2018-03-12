using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

public class KS_FullMap : MonoBehaviour {

    private List<KS_MapObject> mapObject;

    // Instancing

    private static KS_FullMap instance;
    public static KS_FullMap Instance
    {
        get
        {
            if (instance != null) return instance;
            return null;
        }
    }

    //  Events

    public delegate void ScaleHandler(float scale);
    public delegate void MiniMapToggle();
    public delegate void Vector3Handler(Vector3 position);
    public static event ScaleHandler OnScale;
    public static event MiniMapToggle OnMinimap;
    public static event MiniMapToggle OffMiniMap;
    public static event Vector3Handler WayPointMarkerAdded;
    public static event MiniMapToggle WayPointRemoved;

    // Waypoints and markers

    [Header("Waypoints & Markers")]
    public bool allowMarkers = true;
    public int maxMarkers = 4;
    public GameObject wayPointPrefab;
    private GameObject wayPoint;
    public LayerMask waypointMask;

    // Minimap

    [Header("Minimap")]
    public bool useMinimap = true;
    public GameObject minimapContainer;
    public float miniMapScale = 50f;
    public bool oreintToPlayer = false;
    public RawImage minimapDisplay;
    public RenderTextureFormat renderFormat;

    private RenderTexture miniMapTexture;

    [Header("Full Screen Map")]
    public float cameraSpeed = 30f;
    public float cameraSmoothTime = 1.0f;

    public float defultCameraScale = 50f;
    public float maxCameraScale = 50f;
    public float minCameraScale = 10f;
    public float cameraZoomSpeed = 5f;
    private float targetScale = 50f;
    public float cameraMouseWheelZoomeSpeed = 5f;

    public LayerMask UiCompenentRayTraceMask;

    private Vector2 bottomLeft = new Vector2();
    private Vector2 topRight = new Vector2();

    public List<GameObject> mapObjects = new List<GameObject>();

    public GameObject player;
    public GameObject camera;
    public GameObject uiObjectsCamera;

    public GameObject mapArea;


    private GameObject hoverMapObject = null;

    public bool mapActive = false;

    private void Awake()
    {
        instance = this;
        miniMapTexture = new RenderTexture(Screen.width, Screen.height, 16, renderFormat);
        miniMapTexture.Create();
        mapObject = new List<KS_MapObject>();
    }

    private void Start()
    {
        if(mapArea)
            GetMapSize();

        DeactivateMap();
    }

    private float oldAspectRatio;

    public void ActivateMap()
    {
        if (useMinimap)
        {
            uiObjectsCamera.GetComponent<Camera>().targetTexture = null;
            camera.GetComponent<Camera>().targetTexture = null;
            minimapDisplay.texture = null;
            minimapContainer.SetActive(false);

            camera.GetComponent<Camera>().aspect = oldAspectRatio;
            uiObjectsCamera.GetComponent<Camera>().aspect = oldAspectRatio;

            if (OffMiniMap != null)
                OffMiniMap();
        }

        targetPosition = player.transform.position;
        targetScale = defultCameraScale;
        mapActive = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DeactivateMap()
    {
        mapActive = false;
        Cursor.visible = false;

        if (useMinimap)
        {
            oldAspectRatio = camera.GetComponent<Camera>().aspect;

            camera.GetComponent<Camera>().aspect = 1f / 1f;
            uiObjectsCamera.GetComponent<Camera>().aspect = 1f / 1f;

            uiObjectsCamera.GetComponent<Camera>().targetTexture = miniMapTexture;
            camera.GetComponent<Camera>().targetTexture = miniMapTexture;
            minimapDisplay.texture = miniMapTexture;
            minimapContainer.SetActive(true);

            if (OnMinimap != null)
                OnMinimap();
        }
    }

    void GetMapSize()
    {
        Renderer rend = mapArea.GetComponent<Renderer>();
        Bounds bounds = rend.bounds;

        topRight= new Vector2(bounds.max.x, bounds.max.z);
        bottomLeft = new Vector2(bounds.min.x, bounds.min.z);
        
        Debug.Log("top R: " + topRight);
        Debug.Log("bottom L: " + bottomLeft);
    }

    Vector3 targetPosition = new Vector3();

    private void FixedUpdate()
    {
        if (useMinimap && !mapActive)
        {
            targetPosition = player.transform.position;
            targetScale = miniMapScale;
        }


        if (mapActive)
        {

            // Camera Movement

            if (Input.GetKey(KeyCode.W))
            {
                targetPosition += Vector3.forward * cameraSpeed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                targetPosition += Vector3.back * cameraSpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                targetPosition += Vector3.left * cameraSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                targetPosition += Vector3.right * cameraSpeed;
            }
        }

        targetPosition.y = camera.transform.position.y;

        targetPosition = CheckBounds(targetPosition);

        // Apply

        if (!mapActive && useMinimap)
        {
            camera.transform.position = targetPosition;
        }
        else
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, Time.deltaTime * cameraSmoothTime);
        }

        // end

        // Camera Scale
        if (mapActive)
        {
            if (Input.GetKey(KeyCode.Minus))
            {
                targetScale += cameraZoomSpeed * ScalePercent;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                targetScale += cameraMouseWheelZoomeSpeed;
            }

            if (Input.GetKey(KeyCode.Equals))
            {
                targetScale -= cameraZoomSpeed * ScalePercent;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                targetScale -= cameraMouseWheelZoomeSpeed;
            }
        }

        if (targetScale < minCameraScale) targetScale = minCameraScale;
        if (targetScale > maxCameraScale) targetScale = maxCameraScale;

        if (targetScale != camera.GetComponent<Camera>().orthographicSize)
        {
            if (OnScale != null)
                OnScale(ScalePercent);
        }

        camera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(camera.GetComponent<Camera>().orthographicSize, targetScale, Time.deltaTime * cameraSmoothTime);
        uiObjectsCamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(uiObjectsCamera.GetComponent<Camera>().orthographicSize, targetScale, Time.deltaTime * cameraSmoothTime);

        // End

        // RayTrace cursor
        if (mapActive)
        {
            RaycastHit hit;
            Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 3000f, UiCompenentRayTraceMask))
            {
                if (hit.collider.gameObject)
                {
                    if (hoverMapObject == null && hit.collider.gameObject.GetComponent<KS_MapObject>())
                    {
                        hoverMapObject = hit.collider.gameObject;
                        hoverMapObject.GetComponent<KS_MapObject>().OnHover();
                    }
                    else if (hit.collider.gameObject != hoverMapObject && hit.collider.gameObject.GetComponent<KS_MapObject>())
                    {
                        hoverMapObject.GetComponent<KS_MapObject>().OffHover();
                        hoverMapObject = hit.collider.gameObject;
                        hoverMapObject.GetComponent<KS_MapObject>().OnHover();
                    }
                }
            }
            else
            {
                if (hoverMapObject != null)
                {
                    hoverMapObject.GetComponent<KS_MapObject>().OffHover();
                    hoverMapObject = null;
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * 5000, Color.red, Time.deltaTime);
        }

        // Waypoint marker

        if (mapActive)
        {
            if (Input.GetMouseButtonDown(0)) {

                if (hoverMapObject != null)
                {
                    KS_MapObject mapObj;
                    if (mapObj = hoverMapObject.GetComponent<KS_MapObject>())
                    {
                        if (GetTargetedMapObject())
                        {
                            Debug.Log("Removing targeted");
                            if (mapObj.IsTargeted)
                            {
                                RemoveWaypoint();
                                mapObj.WaypointTarget = false;
                                return;
                            }

                            GetTargetedMapObject().WaypointTarget = false;
                        }

                        // If map object is a waypoint
                        if (mapObj.type == KS_MapObject.MapItemType.Waypoint)
                        {
                            Debug.Log("Waypoint - Removing");
                            RemoveWaypoint();
                            return;
                        }

                        // If not set as waypoint on not waypoint, create
                        Debug.Log("Creating waypoint on: " + mapObj.displayName);
                        SetWaypoint(hoverMapObject.transform.position);
                        wayPoint.GetComponent<Collider>().enabled = false;
                        mapObj.WaypointTarget = true;

                        return;
                    }
                }

                RaycastHit hit;
                Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject)
                    {
                        SetWaypoint(hit.point);
                    }
                }
            }
        }

    }

    void SetWaypoint(Vector3 target)
    {
        if (wayPoint)
        {
            wayPoint.transform.position = target;
            wayPoint.GetComponent<Collider>().enabled = true;
        }
        else
        {
            wayPoint = GameObject.Instantiate(wayPointPrefab,
                                                    target,
                                                    Quaternion.identity);
        }

        if (WayPointMarkerAdded != null)
            WayPointMarkerAdded(target);
    }

    void RemoveWaypoint()
    {
        Destroy(wayPoint);

        if (WayPointRemoved != null)
            WayPointRemoved();
    }

    Vector2 CameraOffsetBounds()
    {
        Camera mapCam = camera.GetComponent<Camera>();
        Vector2 bounds = new Vector2();

        Vector3 lowerLeft = mapCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 lowerRight = mapCam.ViewportToWorldPoint(new Vector3(1, 0, 0));
        Vector3 topLeft = mapCam.ViewportToWorldPoint(new Vector3(0, 1, 0));

        bounds.x = (lowerLeft - lowerRight).magnitude / 2;
        bounds.y = (lowerLeft - topLeft).magnitude / 2;

        return bounds;
    }

    Vector3 CheckBounds(Vector3 targetPosition)
    {
        Vector2 cameraBounds = CameraOffsetBounds();

        if (targetPosition.x > (topRight.x - cameraBounds.x))
        {
            targetPosition.x = topRight.x - cameraBounds.x;
        }

        if (targetPosition.x < (bottomLeft.x + cameraBounds.x))
        {
            targetPosition.x = bottomLeft.x + cameraBounds.x;
        }

        if(targetPosition.z > topRight.y - cameraBounds.y)
        {
            targetPosition.z = topRight.y - cameraBounds.y;
        }

        if (targetPosition.z < bottomLeft.y + cameraBounds.y)
        {
            targetPosition.z = bottomLeft.y + cameraBounds.y;
        }

        return targetPosition;
    }

    public float ScalePercent
    {
        get { return camera.GetComponent<Camera>().orthographicSize / maxCameraScale; }
    }

    public void RegisterMapObject(KS_MapObject obj)
    {
        mapObject.Add(obj);
    }

    public void UnregisterMapObject(KS_MapObject obj)
    {
        mapObject.Remove(obj);
    }

    public int GetTotalMapObjectsOfType(KS_MapObject.MapItemType type)
    {
        return mapObject.Count(i => i.type == type);
    }

    public KS_MapObject GetTargetedMapObject()
    {
        if (mapObject.Count <= 0) return null;

        for(int i = 0; i < mapObject.Count - 1; i++)
        {
            if (mapObject[i].IsTargeted) return mapObject[i];
        }

        return null;
    }

    public void NewWaypoint(Vector3 position)
    {
        SetWaypoint(position);
    }
}
