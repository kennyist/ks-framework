using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KS_MapObject : MonoBehaviour {

    public enum MapItemType
    {
        Settlement,
        chest,
        Waypoint,
    }

    public string displayName = "Place/item";
    public MapItemType type;
    public Sprite mapIcon;

    public bool showOnMinimap = true;
    private bool miniMapActive = false;
    public bool WaypointTarget = false;

    public float minimapIconScaleUp = 2f;

    [Range(0.0f, 1.0f)]
    public float dontShowAfterMapZoomPercent = 1f;

    public Text displayText;
    public Image displayImage;

    private Vector3 startScale;

    public void OnHover()
    {
        displayText.gameObject.SetActive(true);
        SetImageOpacity(1f);
    }

    public void OffHover()
    {
        displayText.gameObject.SetActive(false);
        SetImageOpacity(0.3f);
    }

    void OnScale(float scale)
    {
        transform.localScale = startScale * scale;

        if(scale > dontShowAfterMapZoomPercent)
        {
            GetComponent<BoxCollider>().enabled = false;
            displayImage.gameObject.SetActive(false);
        }
        else
        {
            if (!displayImage.gameObject.activeSelf)
            {
                GetComponent<BoxCollider>().enabled = true;

                if (miniMapActive && !showOnMinimap)
                {
                    displayImage.gameObject.SetActive(false);
                }
                else
                {
                    displayImage.gameObject.SetActive(true);
                }
            }
        }
    }

    void SetImageOpacity(float value)
    {
        Color imageCol = displayImage.color;
        imageCol.a = value;
        displayImage.color = imageCol;
    }

    void OnMiniMap()
    {
        miniMapActive = true;
        startScale = startScale * minimapIconScaleUp;

        if (!showOnMinimap)
            displayImage.gameObject.SetActive(false);

        OffHover();
        SetImageOpacity(1);
    }

    void OffMiniMap()
    {
        miniMapActive = false;

        startScale = startScale / minimapIconScaleUp;
        OffHover();
    }

    // Use this for initialization
    void Awake () {
        
        startScale = transform.localScale;
        displayText.text = displayName;

        KS_FullMap.OnScale += OnScale;
        KS_FullMap.OnMinimap += OnMiniMap;
        KS_FullMap.OffMiniMap += OffMiniMap;


        if (mapIcon)
        {
            displayImage.sprite = mapIcon;
        }

    }

    void SetWaypointTarget()
    {

    }

    void UnSetWaypointTarget()
    {

    }

    public bool IsTargeted
    {
        get { return WaypointTarget; }
    }

    private void Start()
    {
        KS_FullMap.Instance.RegisterMapObject(this);
    }

    private void OnDestroy()
    {
        KS_FullMap.Instance.UnregisterMapObject(this);
        KS_FullMap.OnScale -= OnScale;
        KS_FullMap.OnMinimap -= OnMiniMap;
        KS_FullMap.OffMiniMap -= OffMiniMap;
    }
}
