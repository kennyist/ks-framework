using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS_Mapping
{
    public class DisplayMinimap : MonoBehaviour
    {

        KS_Map map;

        public GameObject minimapContainer;
        public RawImage minimapRender;

        // Use this for initialization
        void Start()
        {
            map = KS_Map.Instance;
            minimapRender.texture = map.MiniMapTexture;
            map.EnableMinimap();
        }

        // Update is called once per frame
        void Update()
        {
            if (map.MiniMapActive)
            {
                if (!minimapContainer.activeSelf)
                {
                    minimapContainer.SetActive(true);
                    minimapRender.texture = map.MiniMapTexture;
                }
            }
            else
            {
                if (minimapContainer.activeSelf)
                    minimapContainer.SetActive(false);
            }
        }
    }
}