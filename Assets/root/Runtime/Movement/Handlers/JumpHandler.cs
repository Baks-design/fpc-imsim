using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Name;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class JumpHandler : IJumpHandler
    {
        readonly JumpSettings settings;
        readonly ICrouchHandler crouchHandler;
        float coyoteTimeCounter;
        float jumpBufferCounter;
        bool wasJumping;

        public Vector3 JumpVelocity { get; private set; }
        public bool IsJumping { get; private set; }

        public JumpHandler(JumpSettings settings, ICrouchHandler crouchHandler)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.crouchHandler = crouchHandler ?? throw new ArgumentNullException(nameof(crouchHandler));
        }

        public void UpdateJump(IGroundChecker groundChecker, PlayerInputController input)
        {
            // Update coyote time
            if (groundChecker.IsGrounded)
                coyoteTimeCounter = settings.coyoteTime;
            else
                coyoteTimeCounter -= Time.deltaTime;

            // Update jump buffer
            if (input.MovementInput.JumpWasPressed())
                jumpBufferCounter = settings.jumpBufferTime;
            else
                jumpBufferCounter -= Time.deltaTime;

            IsJumping = !groundChecker.IsGrounded && wasJumping;

            wasJumping = !groundChecker.IsGrounded;
        }

        public void HandleJump(IGroundChecker groundChecker)
        {
            var canJump = (coyoteTimeCounter > 0f || groundChecker.IsGrounded) &&
                          !crouchHandler.IsCrouching &&
                          !crouchHandler.IsDuringCrouchAnimation;

            var shouldJump = jumpBufferCounter > 0f && canJump;

            if (shouldJump)
            {
                var jumpSpeed = settings.jumpSpeed;

                // Apply crouch jump bonus if jumping while crouched (but not during crouch animation)
                if (crouchHandler.IsCrouching)
                    jumpSpeed *= settings.crouchJumpMultiplier;

                JumpVelocity = Vector3.up * jumpSpeed;
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else
                JumpVelocity = Vector3.zero;
        }

        public void ResetJump()
        {
            JumpVelocity = Vector3.zero;
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
            IsJumping = false;
            wasJumping = false;
        }
    }
}