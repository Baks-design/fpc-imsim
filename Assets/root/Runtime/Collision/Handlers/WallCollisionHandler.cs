using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class WallCollisionHandler : IWallChecker
    {
        readonly CharacterController controller;
        readonly WallSettings settings;
        readonly Transform transform;
        readonly Vector3[] checkDirections = new Vector3[4];
        RaycastHit wallHit;
        Vector3 lastWallNormal;
        bool isNearWall;
        float lastCheckTime;

        public bool IsHitWall => isNearWall;
        public Vector3 WallNormal => lastWallNormal;
        public float WallDistance => isNearWall ? wallHit.distance : float.PositiveInfinity;
        public Vector3 WallPoint => isNearWall ? wallHit.point : Vector3.zero;
        public Collider WallCollider => isNearWall ? wallHit.collider : null;

        public WallCollisionHandler(CharacterController controller, WallSettings settings, Transform transform)
        {
            this.controller = controller != null ? controller : throw new ArgumentNullException(nameof(controller));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));

            InitializeCheckDirections();
        }

        void InitializeCheckDirections()
        {
            checkDirections[0] = Vector3.forward;
            checkDirections[1] = Vector3.back;
            checkDirections[2] = Vector3.right;
            checkDirections[3] = Vector3.left;
        }

        public void UpdateWallCheck()
        {
            lastCheckTime += Time.time;
            if (lastCheckTime < settings.checkInterval) return;

            PerformWallCheck();
            lastCheckTime = 0f;
        }

        void PerformWallCheck()
        {
            var origin = transform.position + controller.center;
            isNearWall = false;
            var closestDistance = float.MaxValue;
            var closestHit = new RaycastHit();

            foreach (var direction in checkDirections)
            {
                var worldDirection = transform.TransformDirection(direction);

                if (Physics.SphereCast(
                    origin,
                    settings.rayObstacleSphereRadius,
                    worldDirection,
                    out var hit,
                    settings.rayObstacleLength,
                    settings.obstacleLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        closestHit = hit;
                        isNearWall = true;
                    }
                }
            }

            if (isNearWall)
            {
                wallHit = closestHit;
                lastWallNormal = wallHit.normal;
            }
        }

        public bool WouldHitWall(Vector3 movementDirection, float distanceThreshold = 0.1f)
        {
            if (!isNearWall) return false;

            var origin = transform.position + controller.center;
            var worldDirection = transform.TransformDirection(movementDirection.normalized);

            return Physics.SphereCast(
                origin,
                settings.rayObstacleSphereRadius,
                worldDirection,
                out _,
                WallDistance + distanceThreshold,
                settings.obstacleLayers,
                QueryTriggerInteraction.Ignore);
        }

        public Vector3 GetWallAdjustedDirection(Vector3 inputDirection)
        {
            if (!isNearWall) return inputDirection;

            // Project movement direction to be parallel with wall
            return Vector3.ProjectOnPlane(inputDirection, lastWallNormal).normalized;
        }

        public void DrawDebugGizmos()
        {
            var origin = transform.position + controller.center;
            Gizmos.color = isNearWall ? Color.red : Color.green;

            foreach (var direction in checkDirections)
            {
                var worldDirection = transform.TransformDirection(direction);
                Gizmos.DrawLine(origin, origin + worldDirection * settings.rayObstacleLength);
            }

            if (isNearWall)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(wallHit.point, 0.1f);
                Gizmos.DrawLine(wallHit.point, wallHit.point + wallHit.normal);
            }
        }
    }
}