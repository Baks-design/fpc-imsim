using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IWallChecker
    {
        bool IsHitWall { get; }
        float WallDistance { get; }
        Vector3 WallNormal { get; }
        Vector3 WallPoint { get; }
        Collider WallCollider { get; }

        void UpdateWallCheck();
        bool WouldHitWall(Vector3 movementDirection, float distanceThreshold = 0.1f);
        void DrawDebugGizmos();
    }
}