using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IGroundChecker
    {
        bool IsGrounded { get; }
        bool IsPreviouslyGrounded { get; }
        bool IsOnSlope { get; }
        float GroundAngle { get; }
        Vector3 GroundNormal { get; }
        Collider GroundCollider { get; }
        float TimeSinceGrounded { get; }

        void UpdateGroundCheck();
        bool WouldSlideOnSlope(Vector3 movementDirection);
        Vector3 GetSlopeAdjustedDirection(Vector3 inputDirection);
        void DrawDebugGizmos();
    }
}