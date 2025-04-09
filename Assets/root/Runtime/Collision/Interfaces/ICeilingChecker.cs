using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ICeilChecker
    {
        bool IsNearCeil { get; }
        float DistanceToCeil { get; }
        Vector3 CeilNormal { get; }

        void UpdateCeilCheck();
        bool WouldHitCeiling(float proposedMovement);
        void DrawDebugGizmos();
    }
}