using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;
using UnityEngine.UI;

public class KS_Map_Marker : KS_Behaviour {

    public string locationName = "Default Location";
    [Range(0.0f, 1.0f)]
    public float showTillZoomPercent = 1f;
    private Vector3 startScale;

    private bool filtered = false;

    public Image mapIcon;
    public Text mapText;

    private void Awake()
    {
        startScale = transform.localScale;
        mapText.text = locationName;
        mapText.gameObject.SetActive(false);
        KS_Mapping.OnScale += OnScale;
        SetOpactiy(0.5f);
    }

    private void OnDestroy()
    {
        KS_Mapping.OnScale -= OnScale;
    }

    private void OnScale(float scale)
    {
        if (filtered) return;

        transform.localScale = startScale * scale;

        if(scale > showTillZoomPercent)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        GetComponent<Collider>().enabled = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        GetComponent<Collider>().enabled = false;

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnHover()
    {
        SetOpactiy(1f);
        mapText.gameObject.SetActive(true);
    }

    public void OffHover()
    {
        SetOpactiy(0.5f);
        mapText.gameObject.SetActive(false);
    }

    private void SetOpactiy(float value)
    {
        Color imageCol = mapIcon.color;
        imageCol.a = value;
        mapIcon.color = imageCol;
    }
}
