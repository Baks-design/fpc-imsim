using Assets.root.Runtime.Input.Handlers;
using UnityEngine;
using System;
using Assets.root.Runtime.Input.Interfaces;
using Assets.root.Runtime.Look.Interfaces;
using Assets.root.Runtime.Look.Settings;
using Assets.root.Runtime.Utilities.Helpers;

namespace Assets.root.Runtime.Look.Handlers
{
    public class CameraRotationHandler : ICameraRotation
    {
        readonly CameraRotationSettings settings;
        readonly ICameraInput input;
        readonly Transform pitch;
        readonly Transform yaw;
        float targetPitch;
        float targetYaw;
        float desiredPitch;
        float desiredYaw;

        public CameraRotationHandler(CameraRotationSettings settings, ICameraInput input, Transform pitch, Transform yaw)
        {
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.input = input ?? throw new ArgumentNullException(nameof(input));
            this.pitch = pitch != null ? pitch : throw new ArgumentNullException(nameof(pitch));
            this.yaw = yaw != null ? yaw : throw new ArgumentNullException(nameof(yaw));
        }

        public void HandleRotation()
        {
            CalculateRotation();
            SmoothRotation();
            ApplyMovement();
        }

        void CalculateRotation()
        {
            desiredYaw += input.Look().x * settings.speedRotationX * Time.deltaTime;
            desiredPitch -= input.Look().y * settings.speedRotationY * Time.deltaTime;
            desiredPitch = RotationHelper.ClampAngle(desiredPitch, settings.bottomClampY, settings.topClampY);
        }

        void SmoothRotation()
        {
            targetYaw = Mathfs.ExpPosDecay(targetYaw, desiredYaw, Time.deltaTime, settings.decaySpeed);
            targetPitch = Mathfs.ExpPosDecay(targetPitch, desiredPitch, Time.deltaTime, settings.decaySpeed);
        }

        void ApplyMovement()
        {
            yaw.eulerAngles = new Vector3(0f, targetYaw, 0f);
            pitch.localEulerAngles = new Vector3(targetPitch, 0f, 0f);
        }
    }
}