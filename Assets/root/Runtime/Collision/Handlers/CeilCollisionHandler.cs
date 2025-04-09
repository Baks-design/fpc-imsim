using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class CeilCollisionHandler : ICeilChecker
    {
        readonly Transform origin;
        readonly CeilSettings settings;
        readonly CharacterController controller;
        readonly RaycastHit[] ceilHits = new RaycastHit[4];
        RaycastHit closestHit;
        Vector3 lastCeilNormal;
        bool isNearCeil;
        float lastCheckTime;
        float lastCeilDistance;

        public bool IsNearCeil => isNearCeil;
        public float DistanceToCeil => isNearCeil ? lastCeilDistance : float.PositiveInfinity;
        public Vector3 CeilNormal => isNearCeil ? lastCeilNormal : Vector3.up;
        public RaycastHit? ClosestCeilingHit => isNearCeil ? closestHit : null;

        public CeilCollisionHandler(Transform origin, CeilSettings settings, CharacterController controller)
        {
            this.origin = origin != null ? origin : throw new ArgumentNullException(nameof(origin));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.controller = controller != null ? controller : throw new ArgumentNullException(nameof(controller));
        }

        public void UpdateCeilCheck( )
        {
            lastCheckTime += Time.time;
            if (lastCheckTime < settings.checkInterval) return;

            PerformCeilingCheck();
            lastCheckTime = 0f;
        }

        void PerformCeilingCheck()
        {
            var originPos = origin.position + controller.center;
            var hitCount = Physics.SphereCastNonAlloc(
                originPos,
                settings.checkRadius,
                Vector3.up,
                ceilHits,
                settings.checkDistance,
                settings.ceilingLayers,
                QueryTriggerInteraction.Ignore);

            if (hitCount == 0)
            {
                isNearCeil = false;
                return;
            }

            // Find closest ceiling hit
            closestHit = ceilHits[0];
            for (var i = 1; i < hitCount; i++)
                if (ceilHits[i].distance < closestHit.distance)
                    closestHit = ceilHits[i];
            
            isNearCeil = true;
            lastCeilDistance = closestHit.distance;
            lastCeilNormal = closestHit.normal;
        }

        public bool WouldHitCeiling(float proposedMovement)
        {
            if (!isNearCeil) return false;

            var originPos = origin.position + controller.center;
            return Physics.CheckSphere(
                originPos + Vector3.up * (lastCeilDistance + proposedMovement),
                settings.checkRadius,
                settings.ceilingLayers,
                QueryTriggerInteraction.Ignore);
        }

        public void DrawDebugGizmos()
        {
            var originPos = origin.position + controller.center;
            Gizmos.color = isNearCeil ? Color.red : Color.green;
            Gizmos.DrawWireSphere(originPos + Vector3.up * lastCeilDistance, settings.checkRadius);

            if (isNearCeil)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(closestHit.point, closestHit.point + closestHit.normal * 0.5f);
            }
        }
    }
}