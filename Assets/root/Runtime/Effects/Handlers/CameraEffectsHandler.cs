using Assets.root.Runtime.Input.Interfaces;
using Assets.root.Runtime.Movement.Interfaces;
using UnityEngine;
using Assets.root.RunMovement.Handlers;
using Assets.root.Runtime.Look.Settings;
using System;
using Assets.root.Runtime.Look.Interfaces;
using Assets.root.Runtime.Look;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class CameraEffectsHandler : ICameraEffectsHandler
    {
        public enum CameraState
        {
            Default,
            Running,
            WallHit,
            NoInput
        }

        readonly PlayerLookController lookController;
        readonly IMovementInput input;
        readonly ICameraFOV cameraFovHandler;
        readonly ICameraSway cameraSwayHandler;
        readonly CameraSwaySettings swaySettings;
        CameraState currentCameraState;
        bool isDuringRunAnimation;
        float currentSwayIntensity;
        float swayVelocity;

        public CameraEffectsHandler(
            PlayerLookController lookController,
            IMovementInput input,
            CameraSwaySettings swaySettings)
        {
            this.lookController = lookController != null ? lookController : throw new ArgumentNullException(nameof(lookController));
            this.input = input ?? throw new ArgumentNullException(nameof(input));
            this.swaySettings = swaySettings ?? throw new ArgumentNullException(nameof(swaySettings));
        }

        public void UpdateCameraEffects(IWallChecker wallChecker, IMovementHandler movementHandler)
        {
            UpdateCameraState(wallChecker, movementHandler);
            HandleCameraSway(movementHandler);
            HandleRunFOV();
        }

        void UpdateCameraState(IWallChecker wallChecker, IMovementHandler movementHandler)
        {
            if (wallChecker.IsHitWall)
            {
                currentCameraState = CameraState.WallHit;
                return;
            }

            if (!input.HasInput)
            {
                currentCameraState = CameraState.NoInput;
                return;
            }

            if ((input.RunIsPressed() || input.RunWasPressed()) &&
                movementHandler.CurrentState == MovementHandler.MovementState.Running)
            {
                currentCameraState = CameraState.Running;
                return;
            }

            currentCameraState = CameraState.Default;
        }

        void HandleCameraSway(IMovementHandler movementHandler)
        {
            var targetSwayIntensity = CalculateTargetSwayIntensity();
            currentSwayIntensity = Mathf.SmoothDamp(
                currentSwayIntensity,
                targetSwayIntensity,
                ref swayVelocity,
                swaySettings.swayTransitionTime,
                Mathf.Infinity,
                Time.deltaTime
            );

            lookController.HandleSway(movementHandler.FinalMove, movementHandler.FinalMove.x * currentSwayIntensity);
        }

        float CalculateTargetSwayIntensity()
        {
            if (currentCameraState == CameraState.Running)
                return swaySettings.runSwayIntensity;

            if (currentCameraState == CameraState.WallHit)
                return 0f;

            return swaySettings.defaultSwayIntensity;
        }

        void HandleRunFOV()
        {
            var shouldStartRunFOV = (input.RunWasPressed() || input.RunIsPressed())
                && !isDuringRunAnimation && currentCameraState == CameraState.Running;

            var shouldResetRunFOV = input.RunWasReleased() || !input.HasInput || currentCameraState == CameraState.WallHit;

            if (shouldStartRunFOV)
            {
                isDuringRunAnimation = true;
                lookController.ChangeRunFOV(true);
            }
            else if (shouldResetRunFOV && isDuringRunAnimation)
            {
                isDuringRunAnimation = false;
                lookController.ChangeRunFOV(false);
            }
        }

        public void ResetCameraEffects()
        {
            isDuringRunAnimation = false;
            currentSwayIntensity = 0f;
            swayVelocity = 0f;
            cameraFovHandler.ResetFOV();
            cameraSwayHandler.ResetSway();
        }
    }
}