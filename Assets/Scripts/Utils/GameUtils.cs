using UnityEngine;

namespace MaiNull.Utils
{
    public static class GameUtils
    {
        public static Quaternion GetSwayTargetRotation(float xAxis, float yAxis, float multiplier)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

            Quaternion rotationX = Quaternion.AngleAxis(-yAxis, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(xAxis, Vector3.up);

            Quaternion targetRotation = rotationX * rotationY;

            return targetRotation;
            
            // Example setup:
            //localRotation = Quaternion.Slerp(localRotation, GetSwayTargetRotation(float xAxis, float yAxis, float multiplier), smooth * Time.deltaTime);
        }
        
        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360f;
            }
            return angle;   
        }
    }
}