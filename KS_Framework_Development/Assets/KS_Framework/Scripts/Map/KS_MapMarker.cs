using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;
using UnityEngine.UI;

namespace KS_Mapping
{
    /// <summary>
    /// Simple map marker script that handles the scaling of the UI components on map scale and simple hover text display
    /// </summary>
    public class KS_MapMarker : KS_Behaviour
    {
        /// <summary>
        /// Name of this markers location
        /// </summary>
        public string locationName = "Default Location";
        /// <summary>
        /// Show on map till zoom level hit <see cref="KS_Map.ScalePercent"/>
        /// </summary>
        [Range(0.0f, 1.0f)]
        public float showTillZoomPercent = 1f;
        private Vector3 startScale;

        private bool filtered = false;

        /// <summary>
        /// Map icon display
        /// </summary>
        public Image mapIcon;
        /// <summary>
        /// Text display
        /// </summary>
        public Text mapText;

        protected override void Awake()
        {
            startScale = transform.localScale;
            mapText.text = locationName;
            mapText.gameObject.SetActive(false);
            KS_Map.OnScale += OnScale;
            SetOpactiy(0.5f);
        }

        protected override void OnDestroy()
        {
            KS_Map.OnScale -= OnScale;
        }

        private void OnScale(float scale)
        {
            if (filtered) return;

            transform.localScale = startScale * KS_Map.Instance.MarkerScalePercent;

            if (scale > showTillZoomPercent)
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

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// On mouse hover while map is active
        /// </summary>
        public void OnHover()
        {
            SetOpactiy(1f);
            mapText.gameObject.SetActive(true);
        }

        /// <summary>
        /// On mouse leave hover while map is active
        /// </summary>
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
}