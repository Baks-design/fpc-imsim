using UnityEngine;

namespace Assets.root.Runtime.Input.Handlers
{
    public static class RotationHelper
    {
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
                angle = (angle % 360f) + 360f;
            else if (angle > 360f)
                angle %= 360f;
                
            return Mathf.Clamp(angle, min, max);
        }
    }
}