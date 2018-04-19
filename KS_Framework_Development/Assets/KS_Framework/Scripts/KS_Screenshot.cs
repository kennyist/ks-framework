using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.IO;

namespace KS_Utility
{

    public class KS_Screenshot : MonoBehaviour
    {

        public GameObject UI;
        public bool hideUI = true;
        public string screenshotNamePrefix = "gamename_";

        private Texture2D image;
        private KS_FileHelper IO;

        void Start()
        {
            IO = KS_FileHelper.Instance;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                StartCoroutine(TakeScreenshot());
            }
        }


        WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();
        public IEnumerator TakeScreenshot()
        {
            if (UI != null && hideUI) UI.SetActive(false);

            yield return frameEnd;

            image = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            image.Apply();

            byte[] file = image.EncodeToPNG();

            SaveScreenshot(file);

            Debug.Log("Screenshot Taken");

            if (UI != null && hideUI) UI.SetActive(true);
        }

        void SaveScreenshot(byte[] fileContents)
        {
            System.DateTime time = System.DateTime.Now;

            string fileTime = time.Hour.ToString() + ""
                            + time.Minute.ToString() + ""
                            + time.Second.ToString() + ""
                            + time.Day.ToString() + ""
                            + time.Month.ToString() + ""
                            + time.Year.ToString();

            IO.SaveFile(Folders.Screenshots, screenshotNamePrefix + fileTime + ".png", fileContents);
        }
    }
}