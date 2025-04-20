using System;
using Assets.root.Runtime.Collision;
using Assets.root.Runtime.Input.Interfaces;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Assets.root.Runtime.Utilities.Helpers;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class MovementHandler : IMovementHandler
    {
        readonly MovementWalkSettings settings;
        readonly IMovementInput movementInput;
        readonly ICrouchHandler crouchHandler;
        readonly CharacterController character;

        public Vector3 Velocity { get; set; }
        public bool IsRunning { get; set; }

        public MovementHandler(
            MovementWalkSettings settings,
            IMovementInput movementInput,
            ICrouchHandler crouchHandler,
            CharacterController character)
        {
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.movementInput = movementInput ?? throw new ArgumentNullException(nameof(movementInput));
            this.crouchHandler = crouchHandler ?? throw new ArgumentNullException(nameof(crouchHandler));
            this.character = character != null ? character : throw new ArgumentNullException(nameof(character));
        }

        public void ApplyMove(PlayerCollisionController collision)
        {
            var input = new Vector3(movementInput.Move().x, 0f, movementInput.Move().y);
            var worldspaceMoveInput = character.transform.TransformVector(input);

            if (collision.GroundChecker.IsGrounded)
                HandleGroundedMovement(collision, worldspaceMoveInput);
            else
                HandleAirMovement(worldspaceMoveInput);

            ApplyFinalMovement();
        }

        void HandleGroundedMovement(PlayerCollisionController collision, Vector3 worldspaceMoveInput)
        {
            var speedModifier = IsRunning ? settings.SprintSpeedModifier : 1f;
            var targetVelocity = settings.MaxSpeedOnGround * speedModifier * worldspaceMoveInput;

            if (crouchHandler.IsCrouching)
                targetVelocity *= settings.MaxSpeedCrouchedRatio;

            targetVelocity = TransformHelper.GetDirectionReorientedOnSlope(
                character.transform, targetVelocity.normalized, collision.GroundChecker.GroundNormal) * targetVelocity.magnitude;

            Velocity = Vector3.Lerp(Velocity, targetVelocity, settings.MovementSharpnessOnGround * Time.deltaTime);
        }

        void HandleAirMovement(Vector3 worldspaceMoveInput)
        {
            var speedModifier = IsRunning ? settings.SprintSpeedModifier : 1f;
            Velocity += settings.AccelerationSpeedInAir * Time.deltaTime * worldspaceMoveInput;

            var verticalVelocity = Velocity.y;
            var horizontalVelocity = Vector3.ProjectOnPlane(Velocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, settings.MaxSpeedInAir * speedModifier);
            Velocity = horizontalVelocity + (Vector3.up * verticalVelocity);
        }

        void ApplyFinalMovement() => character.Move(Velocity * Time.deltaTime);
    }
}