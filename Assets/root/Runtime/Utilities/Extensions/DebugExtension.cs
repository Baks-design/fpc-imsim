using UnityEngine;

namespace Assets.root.Runtime.Movement
{
    public static class DebugExtension
    {
        /// <summary>
        /// Draws a capsule in the Scene view
        /// </summary>
        public static void DrawCapsule(Vector3 bottom, Vector3 top, Color color, float radius, float duration = 0f)
        {
            var up = (top - bottom).normalized * radius;
            var forward = Vector3.Slerp(up, -up, 0.5f);
            var right = Vector3.Cross(up, forward).normalized * radius;

            // Draw side lines
            Debug.DrawLine(bottom + right, top + right, color, duration);
            Debug.DrawLine(bottom - right, top - right, color, duration);
            Debug.DrawLine(bottom + forward, top + forward, color, duration);
            Debug.DrawLine(bottom - forward, top - forward, color, duration);

            // Draw end caps
            DrawCircle(bottom, up, radius, color, duration);
            DrawCircle(top, -up, radius, color, duration);
        }

        /// <summary>
        /// Draws a circle in the Scene view
        /// </summary>
        public static void DrawCircle(Vector3 center, Vector3 normal, float radius, Color color, float duration = 0f)
        {
            var forward = Vector3.Slerp(normal, -normal, 0.5f);
            var right = Vector3.Cross(normal, forward).normalized * radius;

            var matrix = new Matrix4x4();
            matrix.SetTRS(center, Quaternion.LookRotation(normal), Vector3.one);

            var lastPoint = matrix.MultiplyPoint3x4(right);
            var nextPoint = Vector3.zero;

            for (var i = 0; i < 91; i++)
            {
                nextPoint.x = Mathf.Cos(i * 4f * Mathf.Deg2Rad) * radius;
                nextPoint.z = Mathf.Sin(i * 4f * Mathf.Deg2Rad) * radius;
                nextPoint.y = 0f;

                nextPoint = matrix.MultiplyPoint3x4(nextPoint);

                Debug.DrawLine(lastPoint, nextPoint, color, duration);
                lastPoint = nextPoint;
            }
        }

        /// <summary>
        /// Draws a sphere with more control than Debug.DrawSphere
        /// </summary>
        public static void DrawSphere(Vector3 center, float radius, Color color, float duration = 0f)
        {
            // Draw circles in all three axes
            DrawCircle(center, Vector3.up, radius, color, duration);
            DrawCircle(center, Vector3.right, radius, color, duration);
            DrawCircle(center, Vector3.forward, radius, color, duration);
        }

        /// <summary>
        /// Draws an arrow in the Scene view
        /// </summary>
        public static void DrawArrow(
            Vector3 from, Vector3 to, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f, float duration = 0f)
        {
            Debug.DrawLine(from, to, color, duration);

            var direction = to - from;
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f + arrowHeadAngle, 0f) * Vector3.forward;
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f - arrowHeadAngle, 0f) * Vector3.forward;
            Debug.DrawLine(to, to + right * arrowHeadLength, color, duration);
            Debug.DrawLine(to, to + left * arrowHeadLength, color, duration);
        }
    }
}