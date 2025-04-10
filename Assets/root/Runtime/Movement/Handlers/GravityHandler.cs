using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class GravityHandler : IGravityHandler
    {
        readonly MovementGravitySettings gravitySettings;
        Vector3 finalMoveVector;

        public Vector3 Gravity { get; private set; }
        public float InAirTimer { get; private set; }

        public GravityHandler(MovementGravitySettings gravitySettings)
        => this.gravitySettings = gravitySettings != null ? gravitySettings : throw new ArgumentNullException(nameof(gravitySettings));

        public void UpdateInAirTimer(IGroundChecker groundChecker)
        => InAirTimer = groundChecker.IsGrounded ? 0f : InAirTimer + Time.deltaTime;

        public void ApplyGravity(IGroundChecker groundChecker) //FIXME
        {
            if (!groundChecker.IsGrounded)
            {
                // Apply gravity with multiplier
                finalMoveVector += gravitySettings.gravityMultiplier * Time.deltaTime * Physics.gravity;

                // Clamp maximum falling speed
                finalMoveVector.y = Mathf.Max(finalMoveVector.y, -gravitySettings.maxFallSpeed);
            }
            else if (groundChecker.IsGrounded)
                // Reset vertical movement but maintain small ground stick force
                finalMoveVector.y = Mathf.Max(-gravitySettings.stickToGroundForce, finalMoveVector.y);

            Gravity = finalMoveVector;
        }

        public void ResetGravity()
        {
            finalMoveVector = Vector3.zero;
            Gravity = Vector3.zero;
            InAirTimer = 0f;
        }
    }
}