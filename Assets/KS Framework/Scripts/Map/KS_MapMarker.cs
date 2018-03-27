using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_MapMarker : MonoBehaviour {

    public enum MapMarkerType
    {
        Player,
        Other,
        Waypoint,
    }

    private int _ID;
    private string locationName = "Default Location";
    private MapMarkerType type = MapMarkerType.Other;
    private float miniMapScaleUp = 2f;
    private float showTillZoomPercent = 1f;
    private Vector3 startScale;

    private bool filterable = true;
    private bool showOnMap = true;
    private bool showOnMiniMap = true;
    private bool isWaypointTarget = false;

    private bool isMiniMap = true;
    private bool filtered = false;

    public MapMarkerType MarkerType { get { return type; } set { type = value; } }
    public int MapID { get { return _ID; } }
    public string LocationName { get { return locationName; } set { locationName = value; } }
    public float MiniMapScaleUp { get { return miniMapScaleUp; } set { miniMapScaleUp = value; } }
    public float ShowTillZoomPercent {
        get { return showTillZoomPercent; }
        set
        {
            if (value > 1.0f) showTillZoomPercent = 1.0f;
            else if (value < 0.0f) showTillZoomPercent = 0.0f;
            else showTillZoomPercent = value;
        }
    }
    public bool IsFilterable { get { return filterable; } set { filterable = value; } }
    public bool ShowOnMap { get { return showOnMap; } set { showOnMap = value; } }
    public bool ShowOnMiniMap { get { return showOnMiniMap; } set { showOnMiniMap = value; } }
    public bool MinimapActive { get { return isMiniMap; } }
    public bool IsWaypointTarget { get { return isWaypointTarget; } }

    public virtual void OnHover() { }

    public virtual void OffHover() { }

    public virtual void Hide()
    {
        GetComponent<Collider>().enabled = false;
    }

    public virtual void Show()
    {
        GetComponent<Collider>().enabled = true;
    }

    private void OnScale(float scale)
    {
        if (isMiniMap)
        {
            transform.localScale = (startScale * miniMapScaleUp) * scale;
        }
        else
        {
            transform.localScale = startScale * scale;
        }

        if (!isMiniMap && showOnMap && !filtered)
        {
            Debug.Log("scale");
            if (scale > showTillZoomPercent)
            {
                Hide();
            }
            else
            {
                if(!filtered) Show();
            }
        }
    }

    public virtual void OnMiniMap()
    {
        if (ShowOnMiniMap) Show();
        else Hide();

        isMiniMap = true;
    }

    public virtual void OffMiniMap()
    {
        if (!filtered)
        {
            if (showOnMap) Show();
            else Hide();
        }
        else
        {
            Hide();
        }

        isMiniMap = false;
    }

    public virtual void OnFilter(MapMarkerType type, bool show)
    {
        if (!filterable) return;

        if(type == MarkerType)
        {
            if (show)
            {
                Debug.Log("Filter deactivated");
                if (!isMiniMap)
                    Show();
                filtered = false;
            }
            else
            {
                Debug.Log("Filter activated");
                if (!isMiniMap)
                    Hide();
                filtered = true;
            }
        }
    }

    private void Awake()
    {
        startScale = transform.localScale;

        KS_Mapping.OnScale += OnScale;
        KS_FullMap.OnMinimap += OnMiniMap;
        KS_FullMap.OffMiniMap += OffMiniMap;
        KS_FullMap.OnFilter += OnFilter;
    }

    private void Start()
    {
        _ID = KS_FullMap.Instance.RegisterMapObject(this);
    }

    private void OnDestroy()
    {
        if (KS_FullMap.Instance != null)
        {
            KS_FullMap.Instance.UnregisterMapObject(this);
        }
        KS_Mapping.OnScale -= OnScale;
        KS_FullMap.OnMinimap -= OnMiniMap;
        KS_FullMap.OffMiniMap -= OffMiniMap;
        KS_FullMap.OnFilter -= OnFilter;
    }
}
