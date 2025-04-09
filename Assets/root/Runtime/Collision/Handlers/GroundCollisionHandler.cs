using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class GroundCollisionHandler : IGroundChecker
    {
        readonly CharacterController controller;
        readonly GroundSettings groundSettings;
        readonly JumpSettings jumpSettings;
        readonly float finalRayLength;
        RaycastHit groundHit;
        Vector3 groundNormal;
        bool isGrounded;
        bool wasGrounded;
        float lastCheckTime;
        float lastGroundTime;

        public bool IsGrounded { get; private set; }
        public bool IsPreviouslyGrounded { get; private set; }
        public bool IsOnSlope => IsGrounded && Vector3.Angle(groundNormal, Vector3.up) > groundSettings.slopeLimit;
        public float GroundAngle => IsGrounded ? Vector3.Angle(groundNormal, Vector3.up) : 0f;
        public Vector3 GroundNormal => groundNormal;
        public Collider GroundCollider => groundHit.collider;
        public float TimeSinceGrounded => Time.time - lastGroundTime;

        public GroundCollisionHandler(CharacterController controller, GroundSettings groundSettings, JumpSettings jumpSettings)
        {
            this.controller = controller != null ? controller : throw new ArgumentNullException(nameof(controller));
            this.groundSettings = groundSettings ?? throw new ArgumentNullException(nameof(groundSettings));
            this.jumpSettings = jumpSettings ?? throw new ArgumentNullException(nameof(jumpSettings));

            finalRayLength = groundSettings.rayLength + controller.center.y;
            groundNormal = Vector3.up;
        }

        public void UpdateGroundCheck()
        {
            lastCheckTime += Time.time;
            if (lastCheckTime < groundSettings.checkInterval) return;

            PerformGroundCheck();
            lastCheckTime = 0f;
        }

        void PerformGroundCheck()
        {
            IsPreviouslyGrounded = IsGrounded;
            wasGrounded = isGrounded;

            var origin = controller.transform.position + controller.center;
            isGrounded = Physics.SphereCast(
                origin,
                groundSettings.raySphereRadius,
                Vector3.down,
                out groundHit,
                finalRayLength,
                groundSettings.groundLayer,
                QueryTriggerInteraction.Ignore);

            // Use CharacterController.isGrounded as fallback
            if (groundSettings.useControllerFallback)
                isGrounded = isGrounded || controller.isGrounded;

            // Update ground normal if grounded
            if (isGrounded)
            {
                groundNormal = groundHit.normal;
                lastGroundTime = Time.time;
            }
            else
                groundNormal = Vector3.up;

            // Apply coyote time if leaving ground
            IsGrounded = wasGrounded && !isGrounded ? TimeSinceGrounded <= jumpSettings.coyoteTime : isGrounded;
        }

        public bool WouldSlideOnSlope(Vector3 movementDirection)
        {
            if (!IsOnSlope) return false;
            var slopeDirection = Vector3.Cross(Vector3.Cross(groundNormal, Vector3.down), groundNormal);
            return Vector3.Angle(movementDirection, slopeDirection) < 90f;
        }

        public Vector3 GetSlopeAdjustedDirection(Vector3 inputDirection)
        {
            if (!IsGrounded) return inputDirection;
            return Vector3.ProjectOnPlane(inputDirection, groundNormal).normalized;
        }

        public void DrawDebugGizmos()
        {
            var origin = controller.transform.position + controller.center;
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(
                origin + Vector3.down * (IsGrounded ? groundHit.distance : finalRayLength),
                groundSettings.raySphereRadius);

            if (IsGrounded)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(groundHit.point, groundHit.point + groundNormal);
            }
        }
    }
}