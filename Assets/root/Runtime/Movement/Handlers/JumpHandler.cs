using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class JumpHandler : IJumpHandler
    {
        readonly MovementJumpSettings settings;
        readonly IMovementHandler movementHandler;

        public bool HasJumpedThisFrame { get; private set; }
        public float LastTimeJumped { get; private set; }

        public JumpHandler(
            MovementJumpSettings settings,
            IMovementHandler movementHandler)
        {
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.movementHandler = movementHandler ?? throw new ArgumentNullException(nameof(movementHandler));

            HasJumpedThisFrame = false;
            LastTimeJumped = 0f;
        }

        public void PerformJump()
        {
            // Reset vertical velocity
            var velocity = movementHandler.Velocity;
            movementHandler.Velocity = new Vector3(velocity.x, 0f, velocity.z);
            // Apply jump force
            movementHandler.Velocity += Vector3.up * settings.JumpForce;

            // Update state
            LastTimeJumped = Time.time;
            HasJumpedThisFrame = !HasJumpedThisFrame;
        }
    }
}