using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS_Mapping
{
    /// <summary>
    /// Display Minimap on screen, disabling and enabling on fullmap state <see cref="KS_Map"/>
    /// </summary>
    public class DisplayMinimap : MonoBehaviour
    {

        KS_Map map;

        /// <summary>
        /// Container of all minimap objects
        /// </summary>
        public GameObject minimapContainer;
        /// <summary>
        /// Raw image to display the render texture from the fullmap
        /// </summary>
        public RawImage minimapRender;

        // Use this for initialization
        void Start()
        {
            map = KS_Map.Instance;
            minimapRender.texture = map.MiniMapTexture;
            //map.EnableMinimap();
        }

        // Update is called once per frame
        void Update()
        {
            if (!map.useMiniMap) minimapContainer.SetActive(false);

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