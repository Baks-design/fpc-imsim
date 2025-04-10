using Assets.root.Runtime.Movement.Interfaces;
using UnityEngine;
using Assets.root.RunMovement.Handlers;
using Assets.root.Runtime.Cam.Settings;
using System;
using Assets.root.Runtime.Cam.Interfaces;
using Assets.root.Runtime.Cam;

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

        readonly PlayerCameraController lookController;
        readonly ICameraFOV cameraFovHandler;
        readonly ICameraSway cameraSwayHandler;
        readonly CameraSwaySettings swaySettings;
        CameraState currentCameraState;
        bool isDuringRunAnimation;
        float currentSwayIntensity;
        float swayVelocity;

        public CameraEffectsHandler(PlayerCameraController lookController, CameraSwaySettings swaySettings)
        {
            this.lookController = lookController != null ? lookController : throw new ArgumentNullException(nameof(lookController));
            this.swaySettings = swaySettings != null ? swaySettings : throw new ArgumentNullException(nameof(swaySettings));
        }

        public void UpdateCameraEffects(
            IWallChecker wallChecker, IMovementHandler movementHandler, PlayerController controller) //FIXME
        {
            UpdateCameraState(wallChecker, movementHandler, controller);
            HandleCameraSway(movementHandler);
            HandleRunFOV(controller);
        }

        void UpdateCameraState(IWallChecker wallChecker, IMovementHandler movementHandler, PlayerController controller)
        {
            if (wallChecker.IsHitWall)
            {
                currentCameraState = CameraState.WallHit;
                return;
            }
            if (!controller.MovementInput.HasInput)
            {
                currentCameraState = CameraState.NoInput;
                return;
            }
            if ((controller.MovementInput.RunIsPressed() || controller.MovementInput.RunWasPressed()) &&
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
            if (currentCameraState is CameraState.Running)
                return swaySettings.runSwayIntensity;

            if (currentCameraState is CameraState.WallHit)
                return 0f;

            return swaySettings.defaultSwayIntensity;
        }

        void HandleRunFOV(PlayerController controller)
        {
            var shouldStartRunFOV = (controller.MovementInput.RunWasPressed() || controller.MovementInput.RunIsPressed())
                && !isDuringRunAnimation && currentCameraState is CameraState.Running;

            var shouldResetRunFOV = controller.MovementInput.RunWasReleased() ||
                !controller.MovementInput.HasInput || currentCameraState is CameraState.WallHit;

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