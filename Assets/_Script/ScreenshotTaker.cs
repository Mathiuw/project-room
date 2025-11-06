using System;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    [SerializeField] private int superSize = 2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            string screenshotName = "Screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".png";

            ScreenCapture.CaptureScreenshot(screenshotName, superSize);
            Debug.Log("Screenshot: " + screenshotName + " was saved");
        }
    }
}
