using UnityEngine;

namespace Assets.root.Runtime.Utilities.Helpers
{
    public static class TransformHelper
    {
        public static Vector3 GetDirectionReorientedOnSlope(Transform transform, Vector3 direction, Vector3 slopeNormal)
        {
            var directionRight = Vector3.Cross(direction, transform.up);
            return Vector3.Cross(slopeNormal, directionRight).normalized;
        }

        // Gets the center point of the bottom hemisphere of the character controller capsule    
        public static Vector3 GetCapsuleBottomHemisphere(Transform transform, float radius)
        => transform.position + (transform.up * radius);

        // Gets the center point of the top hemisphere of the character controller capsule    
        public static Vector3 GetCapsuleTopHemisphere(Transform transform, float height, float radius)
        => transform.position + (transform.up * (height - radius));

        // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
        public static bool IsNormalUnderSlopeLimit(Transform transform, Vector3 normal, float slopeLimit)
        => Vector3.Angle(transform.up, normal) <= slopeLimit;
    }
}