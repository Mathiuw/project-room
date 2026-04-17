using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull.Utils
{
    public class ScreenshotTaker : MonoBehaviour
    {
        [SerializeField] private int superSize = 2;

        private void Update()
        {
            if (!Keyboard.current.f2Key.wasPressedThisFrame) return;
            
            string screenshotName = "Screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".png";

            ScreenCapture.CaptureScreenshot(screenshotName, superSize);
            Debug.Log("Screenshot: " + screenshotName + " was saved");
        }
    }
}
