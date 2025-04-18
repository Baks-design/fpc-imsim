using System;
using Assets.root.Runtime.Movement;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Name;
using UnityEngine;

namespace Assets.root.RunMovement.Handlers
{
    public class MovementHandler : IMovementHandler
    {
        public enum MovementState
        {
            Idle,
            Walking,
            Running,
            Crouching,
            Backpedaling,
            Strafing
        }

        readonly MovementWalkSettings walkSettings;
        readonly MovementCrouchSettings crouchSettings;
        readonly MovementRunSettings runSettings;
        readonly IGravityHandler gravityHandler;
        readonly MovementSmoothSettings smoothSettings;
        readonly ICrouchHandler crouchHandler;
        readonly CharacterController character;
        readonly float walkRunSpeedDifference;
        MovementState currentState;
        Vector3 finalMoveDir;
        Vector3 smoothFinalMoveDir;
        Vector2 smoothInputVector;
        float smoothCurrentSpeed;
        float finalSmoothCurrentSpeed;

        public Vector3 FinalMove { get; private set; }
        public float CurrentSpeed { get; private set; }
        public MovementState CurrentState => currentState;

        public MovementHandler(
            MovementWalkSettings walkSettings,
            MovementCrouchSettings crouchSettings,
            MovementRunSettings runSettings,
            IGravityHandler gravityHandler,
            MovementSmoothSettings smoothSettings,
            ICrouchHandler crouchHandler,
            CharacterController character)
        {
            this.walkSettings = walkSettings != null ? walkSettings : throw new ArgumentNullException(nameof(walkSettings));
            this.runSettings = runSettings != null ? runSettings : throw new ArgumentNullException(nameof(runSettings));
            this.crouchSettings = crouchSettings != null ? crouchSettings : throw new ArgumentNullException(nameof(crouchSettings));
            this.gravityHandler = gravityHandler ?? throw new ArgumentNullException(nameof(gravityHandler));
            this.smoothSettings = smoothSettings != null ? smoothSettings : throw new ArgumentNullException(nameof(smoothSettings));
            this.crouchHandler = crouchHandler ?? throw new ArgumentNullException(nameof(crouchHandler));
            this.character = character != null ? character : throw new ArgumentNullException(nameof(character));

            walkRunSpeedDifference = runSettings.runSpeed - walkSettings.walkSpeed;
            currentState = MovementState.Idle;
        }

        public void HandleMovement(PlayerController input)
        {
            SmoothInput(input);
            CalculateMovementState(input);
            CalculateSpeed(input);
            CalculateSmoothSpeed();
            CalculateMovementDirection();
            CalculateFinalMovement();
            ApplyMovement();
        }

        void SmoothInput(PlayerController input) => smoothInputVector = Vector2.Lerp(
            smoothInputVector, input.MovementInput.Move(), Time.deltaTime * smoothSettings.smoothInputSpeed);

        void CalculateMovementState(PlayerController input)
        {
            if (!input.MovementInput.HasInput)
            {
                currentState = MovementState.Idle;
                return;
            }
            if (crouchHandler.IsCrouching)
            {
                currentState = MovementState.Crouching;
                return;
            }
            if (input.MovementInput.Move().y == -1f)
            {
                currentState = MovementState.Backpedaling;
                return;
            }
            if (input.MovementInput.Move().x != 0f && input.MovementInput.Move().y == 0f)
            {
                currentState = MovementState.Strafing;
                return;
            }
            if (input.MovementInput.RunIsPressed() && CanRun(character.transform))
            {
                currentState = MovementState.Running;
                return;
            }
            currentState = MovementState.Walking;
        }

        void CalculateSpeed(PlayerController input)
        {
            var baseSpeed = walkSettings.walkSpeed;

            switch (currentState)
            {
                case MovementState.Running:
                    baseSpeed = runSettings.runSpeed;
                    break;
                case MovementState.Crouching:
                    baseSpeed = crouchSettings.crouchSpeed;
                    break;
                case MovementState.Backpedaling:
                    baseSpeed *= walkSettings.moveBackwardsSpeedPercent;
                    break;
                case MovementState.Strafing:
                    baseSpeed *= walkSettings.moveSideSpeedPercent;
                    break;
            }

            CurrentSpeed = input.MovementInput.HasInput ? baseSpeed : 0f;
        }

        void CalculateSmoothSpeed()
        {
            smoothCurrentSpeed = Mathf.Lerp(smoothCurrentSpeed, CurrentSpeed, Time.deltaTime * smoothSettings.smoothVelocitySpeed);

            if (currentState == MovementState.Running)
            {
                var walkRunPercent = Mathf.InverseLerp(walkSettings.walkSpeed, runSettings.runSpeed, smoothCurrentSpeed);

                finalSmoothCurrentSpeed = runSettings.runTransitionCurve.Evaluate(walkRunPercent) *
                                        walkRunSpeedDifference + walkSettings.walkSpeed;
            }
            else
                finalSmoothCurrentSpeed = smoothCurrentSpeed;
        }

        bool CanRun(Transform transform)
        {
            if (smoothFinalMoveDir == Vector3.zero) return false;
            if (crouchHandler.IsCrouching) return false;

            var normalizedDir = smoothFinalMoveDir.normalized;
            var dot = Vector3.Dot(transform.forward, normalizedDir);
            return dot >= runSettings.canRunThreshold;
        }

        void CalculateMovementDirection()
        {
            var vDir = character.transform.forward * smoothInputVector.y;
            var hDir = character.transform.right * smoothInputVector.x;
            finalMoveDir = vDir + hDir;

            smoothFinalMoveDir = Vector3.Lerp(
                smoothFinalMoveDir,
                finalMoveDir,
                Time.deltaTime * smoothSettings.smoothFinalDirectionSpeed
            );
        }

        void CalculateFinalMovement()
        {
            var inputMagnitude = smoothSettings.experimental
                ? Mathf.Lerp(0f, 1f, Time.deltaTime * smoothSettings.smoothInputMagnitudeSpeed)
                : 1f;

            var horizontalMovement = finalSmoothCurrentSpeed * inputMagnitude * smoothFinalMoveDir;
            FinalMove = new Vector3(horizontalMovement.x, gravityHandler.Gravity.y, horizontalMovement.z);
        }

        void ApplyMovement() => character.Move(FinalMove * Time.deltaTime);

        public void ResetMovement()
        {
            smoothInputVector = Vector2.zero;
            finalMoveDir = Vector3.zero;
            smoothFinalMoveDir = Vector3.zero;
            smoothCurrentSpeed = 0f;
            finalSmoothCurrentSpeed = 0f;
            FinalMove = Vector3.zero;
            currentState = MovementState.Idle;
        }
    }
}