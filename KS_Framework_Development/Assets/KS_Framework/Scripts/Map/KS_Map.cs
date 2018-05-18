using KS_Core.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;
using KS_Core.Handlers;
using UnityEngine.UI;

namespace KS_Mapping
{
    /// <summary>
    /// Full screen map and Minimap functionality 
    /// </summary>
    public class KS_Map : KS_Behaviour
    {

        // Instancing

        private static KS_Map instance;
        /// <summary>
        /// Current active isntance of KS_Map
        /// </summary>
        public static KS_Map Instance
        {
            get
            {
                if (instance != null) return instance;
                return null;
            }
        }

        // Events 
        /// <summary>
        /// On map zoomed in or out
        /// </summary>
        public static event FloatHandler OnScale;
        /// <summary>
        /// On minimap enable or dissable
        /// </summary>
        public static event BoolHandler MiniMapStateChange;

        //

        /// <summary>
        /// Depth camera that will display the UI elements
        /// </summary>
        public GameObject UIOverlayCamera;
        private Camera UICam;
        /// <summary>
        /// Depth camera that will display the map content
        /// </summary>
        public GameObject mapCamera;

        /// <summary>
        /// Area of the world to show, recomended Box object with no collider or renderer
        /// </summary>
        public GameObject mapArea;
        private Vector3 mapTopRight;
        private Vector3 mapBottomLeft;

        private LayerMask uiMask;

        private bool mapActive = false;

        private GameObject mapHoverObject;

        //

        /// <summary>
        /// Map movement speed in world units 
        /// </summary>
        [Header("Map Movement")]
        public float mapMoveSpeed = 30f;
        /// <summary>
        /// Map movement smooth ammount
        /// </summary>
        public float mapMoveSmoothing = 5.0f;
        /// <summary>
        /// Map move speed based on the map zoom percent (1 = mapMoveSpeed)
        /// </summary>
        public AnimationCurve mapMoveSpeedCurve = new AnimationCurve(new Keyframe(0, 0.3f), new Keyframe(1, 1));
        /// <summary>
        /// Map zoom speed
        /// </summary>
        public float MaxZoomSpeed = 5f;
        /// <summary>
        /// Map zoom speed based on the map zoom percent (1 = MaxZoomSpeed)
        /// </summary>
        public AnimationCurve ZoomSpeedCurve = new AnimationCurve(new Keyframe(0, 0.3f), new Keyframe(1, 1));

        private float mapTargetHeight;
        private Vector3 mapTargetPosition;

        /// <summary>
        /// Minimum height of the camera (For zoom)
        /// </summary>
        [Header("Constraints")]
        public float minCameraHeight = 150f;
        /// <summary>
        /// Default camera height (For zoom), Defaults to this height on full screen map opening
        /// </summary>
        public float defaultCameraHeight = 250f;
        /// <summary>
        /// Maximum camera height (For zoom)
        /// </summary>
        public float maxCameraHeight = 600f;

        /// <summary>
        /// Enable minimap functionality 
        /// </summary>
        [Header("MiniMap")]
        public bool useMiniMap = true;
        private bool miniMapActive = false;
        /// <summary>
        /// Minimap camera height
        /// </summary>
        public float miniMapCameraHeight = 250f;
        /// <summary>
        /// Render texture format <see cref="RenderTextureFormat"/>
        /// </summary>
        public RenderTextureFormat renderFormat;
        private RenderTexture miniMapTexture;

        /// <summary>
        /// Center target of the map, centers here on full screen map opening
        /// </summary>
        public Transform mapCenterTarget;

        protected override void Awake()
        {
            instance = this;

            UIOverlayCamera.SetActive(false);
            mapCamera.SetActive(false);

            miniMapTexture = new RenderTexture(Screen.width, Screen.height, 16, renderFormat);
            miniMapTexture.Create();
        }

        // Use this for initialization
        void Start()
        {
            UICam = UIOverlayCamera.GetComponent<Camera>();
            uiMask = mapCamera.GetComponent<Camera>().cullingMask;
            mapTargetHeight = defaultCameraHeight;
            GetMapSize();
            CenterTarget = mapCenterTarget;

            HideMap();
            if (useMiniMap) EnableMinimap();
        }

        /// <summary>
        /// Current zoom speed based on current zoom level
        /// </summary>
        public float ZoomSpeed
        {
            get
            {
                return MaxZoomSpeed * ZoomSpeedCurve.Evaluate(ScalePercent);
            }
        }

        /// <summary>
        /// Current move speed based on current zoom level
        /// </summary>
        public float MoveSpeed
        {
            get
            {
                return mapMoveSpeed * mapMoveSpeedCurve.Evaluate(ScalePercent);
            }
        }

        private void FixedUpdate()
        {
            if (mapActive || miniMapActive)
            {
                // Map positioning 

                if (miniMapActive)
                {
                    //if (CenterTarget)
                    mapTargetPosition = CenterTarget.position;
                }
                else
                {
                    mapTargetPosition = MapMovement(mapTargetPosition);
                }
                // Camera height

                if (miniMapActive)
                {
                    mapTargetPosition.y = miniMapCameraHeight;
                }
                else
                {
                    mapTargetPosition.y = MapZoom(mapTargetPosition.y);
                }

                // Check camera is within map area

                mapTargetPosition = CheckCameraBounds(mapTargetPosition);

                // apply new position

                UIOverlayCamera.transform.position = Vector3.Lerp(UIOverlayCamera.transform.position, mapTargetPosition, Time.deltaTime * mapMoveSmoothing);

                if (OnScale != null)
                    OnScale(ScalePercent);

                if (!miniMapActive)
                {
                    /* Markers */

                    RayCastMouse();
                }
            }
        }

        private Vector3 MapMovement(Vector3 target)
        {
            target += Vector3.left * (KS_Input.GetAxis("mapMoveX") * (mapMoveSpeed * MoveSpeed));
            target += Vector3.forward * (KS_Input.GetAxis("mapMoveY") * (mapMoveSpeed * MoveSpeed));

            return target;
        }

        private float MapZoom(float target)
        {
            float inaxis = KS_Input.GetAxis("mapMoveIn");
            float outaxis = KS_Input.GetAxis("mapMoveOut");

            if (inaxis > 0f)
            {
                target += ZoomSpeed;
            }

            if (outaxis > 0f)
            {
                target -= ZoomSpeed;
            }


            if (target < minCameraHeight) target = minCameraHeight;
            if (target > maxCameraHeight) target = maxCameraHeight;

            return target;
        }

        private void RayCastMouse()
        {
            RaycastHit hit;
            Ray ray = UICam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 3000f, uiMask))
            {
                if (hit.collider.gameObject)
                {
                    if (mapHoverObject == null && hit.collider.gameObject.GetComponent<KS_MapMarker>())
                    {
                        mapHoverObject = hit.collider.gameObject;
                        mapHoverObject.GetComponent<KS_MapMarker>().OnHover();
                    }
                    else if (hit.collider.gameObject != mapHoverObject && hit.collider.gameObject.GetComponent<KS_MapMarker>())
                    {
                        mapHoverObject.GetComponent<KS_MapMarker>().OffHover();
                        mapHoverObject = hit.collider.gameObject;
                        mapHoverObject.GetComponent<KS_MapMarker>().OnHover();
                    }
                }
            }
            else
            {
                if (mapHoverObject != null)
                {
                    mapHoverObject.GetComponent<KS_MapMarker>().OffHover();
                    mapHoverObject = null;
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * 5000, Color.red, Time.deltaTime);

        }

        //

        Vector2 CameraOffsetBounds()
        {
            Vector2 bounds = new Vector2();

            Vector3 lowerLeft = UICam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 lowerRight = UICam.ViewportToWorldPoint(new Vector3(1, 0, 0));
            Vector3 topLeft = UICam.ViewportToWorldPoint(new Vector3(0, 1, 0));

            bounds.x = (lowerLeft - lowerRight).magnitude / 2;
            bounds.y = (lowerLeft - topLeft).magnitude / 2;

            return bounds;
        }

        private Vector3 CheckCameraBounds(Vector3 target)
        {
            float Width = FrustumWidth / 2;
            float height = FrustumHeight / 2;


            if (target.x > mapTopRight.x - Width)
            {
                target.x = mapTopRight.x - Width;
            }

            if (target.x < mapBottomLeft.x + Width)
            {
                target.x = mapBottomLeft.x + Width;
            }

            if (target.z > mapTopRight.y - height)
            {
                target.z = mapTopRight.y - height;
            }

            if (target.z < mapBottomLeft.y + height)
            {
                target.z = mapBottomLeft.y + height;
            }

            return target;
        }

        private void GetMapSize()
        {
            Renderer rend = mapArea.GetComponent<Renderer>();
            Bounds bounds = rend.bounds;

            mapTopRight = new Vector2(bounds.max.x, bounds.max.z);
            mapBottomLeft = new Vector2(bounds.min.x, bounds.min.z);
        }

        /// <summary>
        /// Enable full map display. Will enable map cameras and dissable minimap view
        /// </summary>
        public void ShowMap()
        {
            if (useMiniMap && miniMapActive)
            {
                DisableMinimap(false);
            }
            else
            {
                UIOverlayCamera.SetActive(true);
                mapCamera.SetActive(true);
            }

            UIOverlayCamera.SetActive(true);
            mapCamera.SetActive(true);
            mapTargetPosition.y = defaultCameraHeight;
            mapActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (CenterTarget)
                mapTargetPosition = new Vector3(mapCenterTarget.position.x, mapTargetHeight, mapCenterTarget.position.z);
        }

        /// <summary>
        /// Enable minimap display 
        /// </summary>
        public void EnableMinimap()
        {
            if (mapActive && !useMiniMap) return;

            UIOverlayCamera.SetActive(true);
            mapCamera.SetActive(true);

            if (UICam && miniMapTexture)
            {
                UICam.targetTexture = miniMapTexture;
            }
            mapCamera.GetComponent<Camera>().targetTexture = miniMapTexture;

            miniMapActive = true;

        }

        /// <summary>
        /// Disable fullmap display
        /// </summary>
        public void HideMap()
        {
            mapActive = false;
            Cursor.visible = false;
            UIOverlayCamera.SetActive(false);
            mapCamera.SetActive(false);

            if (!useMiniMap)
            {
                UIOverlayCamera.SetActive(false);
                mapCamera.SetActive(false);
            }
        }

        /// <summary>
        /// Disalbe minimap display
        /// </summary>
        /// <param name="dissableCameras"></param>
        public void DisableMinimap(bool disableCameras = true)
        {
            miniMapActive = false;

            UICam.targetTexture = null;
            mapCamera.GetComponent<Camera>().targetTexture = null;

            if (disableCameras)
            {
                UIOverlayCamera.SetActive(false);
                mapCamera.SetActive(false);
            }
        }

        /// <summary>
        /// Calculate the current FustumHeight of the camera
        /// </summary>
        private float FrustumWidth
        {
            get
            {
                return FrustumHeight * UICam.GetComponent<Camera>().aspect;
            }
        }

        /// <summary>
        /// Calculate the current frustum width of the camera
        /// </summary>
        private float FrustumHeight
        {
            get
            {
                return 2.0f * UICam.transform.position.y * Mathf.Tan(UICam.GetComponent<Camera>().fieldOfView * 0.5f * Mathf.Deg2Rad);
            }
        }

        /// <summary>
        /// Current map zoom level based on min and max camera heights
        /// </summary>
        public float ScalePercent
        {
            get { return (UICam.transform.position.y - minCameraHeight) / (maxCameraHeight - minCameraHeight); }
        }

        public float MarkerScalePercent
        {
            get { return UICam.transform.position.y / maxCameraHeight; }
        }

        /// <summary>
        /// Get or set the current maps center target
        /// </summary>
        public Transform CenterTarget
        {
            get; set;
        }

        /// <summary>
        /// Is the minimap active
        /// </summary>
        public bool MiniMapActive
        {
            get
            {
                return miniMapActive;
            }
        }

        /// <summary>
        /// Minimap render texture
        /// </summary>
        public RenderTexture MiniMapTexture
        {
            get
            {
                return miniMapTexture;
            }
        }

        /// <summary>
        /// Is the map currently showing
        /// </summary>
        public bool MapActive
        {
            get
            {
                return mapActive;
            }
        }

        //
    }
}