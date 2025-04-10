using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class GroundCollisionHandler : IGroundChecker
    {
        readonly CharacterController controller;
        readonly MovementJumpSettings jumpSettings;
        bool isGrounded;
        bool wasGrounded;
        float lastGroundTime;

        public bool IsGrounded { get; private set; }
        public bool IsPreviouslyGrounded { get; private set; }
        public float TimeSinceGrounded => Time.time - lastGroundTime;

        public GroundCollisionHandler(CharacterController controller, MovementJumpSettings jumpSettings)
        {
            this.controller = controller != null ? controller : throw new ArgumentNullException(nameof(controller));
            this.jumpSettings = jumpSettings != null ? jumpSettings : throw new ArgumentNullException(nameof(jumpSettings));
        }

        public void UpdateGroundCheck()
        {
            IsPreviouslyGrounded = IsGrounded;
            wasGrounded = isGrounded;

            isGrounded = CustomCharacterPhysics.IsCharacterGrounded(controller, out var _);

            // Update ground normal if grounded
            if (isGrounded)
               lastGroundTime = Time.time;
           
            // Apply coyote time if leaving ground
            IsGrounded = wasGrounded && !isGrounded ? TimeSinceGrounded <= jumpSettings.coyoteTime : isGrounded;
        }

        public void DrawDebugGizmos()
        {
            var rayStart = controller.transform.position + Vector3.up * (controller.skinWidth + 0.1f);
            var radius = controller.radius;
            Gizmos.color = IsGrounded ? Color.red : Color.green;
            Gizmos.DrawWireSphere(rayStart, radius);
        }
    }
}