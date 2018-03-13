using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericMapMarker : KS_MapMarker {

    public string displayName = "Generic Marker";
    public float miniMapScale = 2f;
    public MapMarkerType type = MapMarkerType.Other;
    [Range(0.0f,1.0f)]
    public float showTillZoomPercent = 1f;

    public bool showOnFullMap = true;
    public bool showOnMiniMap = true;

    [Range(0.0f, 1.0f)]
    public float inactiveOpacity = 0.5f;

    public bool completed = false;
    public Color completedColour = new Color(25, 25, 25);
    private Color startColor;
    private float currentOpactity = 1f;

    public Text displayText;
    public Image displayImage;

	void Start () {
        LocationName = displayName;
        MiniMapScaleUp = miniMapScale;
        ShowTillZoomPercent = showTillZoomPercent;
        MarkerType = type;

        ShowOnMap = showOnFullMap;
        ShowOnMiniMap = showOnMiniMap;

        displayText.text = displayName;

        startColor = displayImage.color;

        if (completed)
        {
            SetCompleted();
        }
	}

    public void SetCompleted()
    {
        completedColour.a = currentOpactity;
        displayImage.color = completedColour;
        completed = true;
    }

    public void SetUncompleted()
    {
        completedColour.a = currentOpactity;
        displayImage.color = startColor;
        completed = false;
    }

    public override void OnHover()
    {
        base.OnHover();
        SetOpactiy(1f);

        displayText.gameObject.SetActive(true);
    }

    public override void OffHover()
    {
        base.OffHover();
        SetOpactiy(inactiveOpacity);

        displayText.gameObject.SetActive(false);
    }

    private void SetOpactiy(float value)
    {
        Color imageCol = displayImage.color;
        imageCol.a = value;
        currentOpactity = value;
        displayImage.color = imageCol;
    }

    public override void Hide()
    {
        base.Hide();
        displayText.gameObject.SetActive(false);
        displayImage.gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        displayImage.gameObject.SetActive(true);
    }

    public override void OffMiniMap()
    {
        base.OffMiniMap();
        SetOpactiy(inactiveOpacity);
    }

    public override void OnMiniMap()
    {
        base.OnMiniMap();
        displayText.gameObject.SetActive(false);
    }
}
