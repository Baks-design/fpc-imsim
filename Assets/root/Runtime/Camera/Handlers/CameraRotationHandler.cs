using Assets.root.Runtime.Look.Interfaces;
using Assets.root.Runtime.Look.Settings;
using Assets.root.Runtime.Input.Handlers;
using Assets.root.Runtime.Input.Interfaces;
using UnityEngine;
using System;

namespace Assets.root.Runtime.Look.Handlers
{
    public class CameraRotationHandler : ICameraRotation
    {
        readonly Transform target;
        readonly ICameraInput inputProvider;
        readonly IInputServices inputServices;
        readonly CameraRotationSettings rotationSettings;
        float targetPitch;
        float targetYaw;

        public CameraRotationHandler(
            Transform target, ICameraInput inputProvider, IInputServices inputServices,
            CameraRotationSettings rotationSettings)
        {
            this.target = target != null ? target : throw new ArgumentNullException(nameof(target));
            this.inputProvider = inputProvider ?? throw new ArgumentNullException(nameof(inputProvider));
            this.inputServices = inputServices ?? throw new ArgumentNullException(nameof(inputServices));
            this.rotationSettings = rotationSettings ?? throw new ArgumentNullException(nameof(rotationSettings));
        }

        public void HandleRotation()
        {
            if (inputProvider.Look().sqrMagnitude < 0.01f) return;

            var deltaTimeMultiplier = inputServices.IsCurrentDeviceMouse() ? 1f : Time.deltaTime;

            targetPitch += -inputProvider.Look().y * rotationSettings.verticalSpeedRotation * deltaTimeMultiplier;
            targetYaw += inputProvider.Look().x * rotationSettings.horizontalSpeedRotation * deltaTimeMultiplier;

            targetPitch = RotationHelper.ClampAngle(targetPitch, rotationSettings.bottomClamp, rotationSettings.topClamp);

            target.localRotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        }
    }
}