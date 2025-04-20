using System;
using Assets.root.Runtime.Collision;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class GravityHandler : IGravityHandler
    {
        readonly MovementGravitySettings settings;
        readonly IJumpHandler jumpHandler;
        readonly IMovementHandler movementHandler;
        const float k_JumpGroundingPreventionTime = 0.2f;

        public GravityHandler(
            MovementGravitySettings settings,
            IJumpHandler jumpHandler,
            IMovementHandler movementHandler
        )
        {
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.jumpHandler = jumpHandler ?? throw new ArgumentNullException(nameof(jumpHandler));
            this.movementHandler = movementHandler ?? throw new ArgumentNullException(nameof(movementHandler));
        }

        public void ApplyGravity(PlayerCollisionController collision)
        {
            // Apply gravity
            if (!collision.GroundChecker.IsGrounded || Time.time < jumpHandler.LastTimeJumped + k_JumpGroundingPreventionTime)
                movementHandler.Velocity += settings.GravityDownForce * Time.deltaTime * Vector3.down;
        }
    }
}