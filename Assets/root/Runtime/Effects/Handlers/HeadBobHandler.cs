using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class HeadBobHandler : IHeadBobHandler
    {
        public enum HeadBobState
        {
            Idle,
            Walking,
            Running,
            Crouching,
            Backpedaling,
            Strafing
        }

        readonly EffectHeadBobSettings headBobSettings;
        readonly MovementSmoothSettings smoothSettings;
        readonly MovementWalkSettings movementSettings;
        HeadBobState currentState;
        Vector3 finalOffset;
        Vector3 targetPosition;
        bool isResetting;
        float xScroll;
        float yScroll;
        float currentBobSpeed;

        public Vector3 HeadBobOffset => finalOffset;
        public float CurrentStateHeight { get; private set; }

        public HeadBobHandler(
            EffectHeadBobSettings headBobSettings,
            MovementSmoothSettings smoothSettings,
            MovementWalkSettings movementSettings)
        {
            this.headBobSettings = headBobSettings != null ? headBobSettings : throw new ArgumentNullException(nameof(headBobSettings));
            this.smoothSettings = smoothSettings != null ? smoothSettings : throw new ArgumentNullException(nameof(smoothSettings));
            this.movementSettings = movementSettings != null ? movementSettings : throw new ArgumentNullException(nameof(movementSettings));

            currentState = HeadBobState.Idle;
            ResetHeadBob();
        }

        public void UpdateHeadBob(
            IGroundChecker groundChecker, IWallChecker wallChecker, ICrouchHandler crouchHandler,
            Transform yawTransform, PlayerController controller) //FIXME
        {
            UpdateHeadBobState(groundChecker, wallChecker, crouchHandler, controller);
            HandleHeadBobMovement();
            ApplyHeadBobPosition(yawTransform);
        }

        void UpdateHeadBobState(
            IGroundChecker groundChecker, IWallChecker wallChecker, ICrouchHandler crouchHandler,
            PlayerController controller)
        {
            if (!controller.MovementInput.HasInput || !groundChecker.IsGrounded ||
                wallChecker.IsHitWall || crouchHandler.IsDuringCrouchAnimation)
            {
                currentState = HeadBobState.Idle;
                return;
            }

            if (crouchHandler.IsCrouching)
            {
                currentState = HeadBobState.Crouching;
                return;
            }

            if (controller.MovementInput.Move().y == -1f)
            {
                currentState = HeadBobState.Backpedaling;
                return;
            }

            if (controller.MovementInput.Move().x != 0f && controller.MovementInput.Move().y == 0f)
            {
                currentState = HeadBobState.Strafing;
                return;
            }

            if (controller.MovementInput.RunIsPressed())
            {
                currentState = HeadBobState.Running;
                return;
            }

            currentState = HeadBobState.Walking;
        }

        void HandleHeadBobMovement()
        {
            if (currentState == HeadBobState.Idle)
            {
                if (!isResetting)
                    ResetHeadBob();

                return;
            }

            isResetting = false;
            CalculateHeadBobOffset();
        }

        void CalculateHeadBobOffset()
        {
            // Get multipliers based on current state
            var amplitudeMultiplier = GetAmplitudeMultiplier();
            var frequencyMultiplier = GetFrequencyMultiplier();
            var additionalMultiplier = GetMovementDirectionMultiplier();

            // Calculate current bob speed with acceleration
            var targetBobSpeed = GetTargetBobSpeed();
            currentBobSpeed = Mathf.Lerp(
                currentBobSpeed,
                targetBobSpeed,
                Time.deltaTime * headBobSettings.bobSpeedTransitionSharpness
            );

            // Update scroll values
            xScroll += Time.deltaTime * headBobSettings.xFrequency * frequencyMultiplier * currentBobSpeed;
            yScroll += Time.deltaTime * headBobSettings.yFrequency * frequencyMultiplier * currentBobSpeed;

            // Evaluate curves
            var xValue = headBobSettings.xCurve.Evaluate(xScroll);
            var yValue = headBobSettings.yCurve.Evaluate(yScroll);

            // Apply all multipliers
            finalOffset = new Vector3(
                xValue * headBobSettings.xAmplitude * amplitudeMultiplier * additionalMultiplier,
                yValue * headBobSettings.yAmplitude * amplitudeMultiplier * additionalMultiplier,
                0f
            );
        }

        float GetAmplitudeMultiplier()
        => currentState switch
        {
            HeadBobState.Running => headBobSettings.runAmplitudeMultiplier,
            HeadBobState.Crouching => headBobSettings.crouchAmplitudeMultiplier,
            _ => 1f
        };

        float GetFrequencyMultiplier()
        => currentState switch
        {
            HeadBobState.Running => headBobSettings.runFrequencyMultiplier,
            HeadBobState.Crouching => headBobSettings.crouchFrequencyMultiplier,
            _ => 1f
        };

        float GetMovementDirectionMultiplier()
        => currentState switch
        {
            HeadBobState.Backpedaling => movementSettings.moveBackwardsSpeedPercent,
            HeadBobState.Strafing => movementSettings.moveSideSpeedPercent,
            _ => 1f
        };

        float GetTargetBobSpeed()
        => currentState switch
        {
            HeadBobState.Running => headBobSettings.runBobSpeed,
            _ => headBobSettings.walkBobSpeed
        };

        void ApplyHeadBobPosition(Transform yawTransform)
        {
            targetPosition = Vector3.up * CurrentStateHeight + finalOffset;
            yawTransform.localPosition = Vector3.Lerp(
                yawTransform.localPosition,
                targetPosition,
                Time.deltaTime * smoothSettings.smoothHeadBobSpeed
            );
        }

        public void ResetHeadBob()
        {
            isResetting = true;
            xScroll = 0f;
            yScroll = 0f;
            finalOffset = Vector3.zero;
            currentBobSpeed = 0f;
        }

        public void SetHeadBobIntensity(float intensityMultiplier)
        {
            headBobSettings.xAmplitude *= intensityMultiplier;
            headBobSettings.yAmplitude *= intensityMultiplier;
        }
    }
}