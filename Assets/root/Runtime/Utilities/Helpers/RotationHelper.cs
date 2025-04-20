namespace Assets.root.Runtime.Utilities.Helpers
{
    public static class RotationHelper
    {
        public static float ClampAngle(float angle, float min, float max)
        {
            angle = NormalizeAngle(angle);
            return Mathfs.Clamp(angle, min, max);
        }

        public static float NormalizeAngle(float angle)
        {
            // More efficient angle normalization
            angle %= 360f;
            if (angle > 180f) angle -= 360f;
            if (angle < -180f) angle += 360f;
            return angle;
        }
    }
}