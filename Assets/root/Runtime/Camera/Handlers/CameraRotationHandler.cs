using Assets.root.Runtime.Cam.Interfaces;
using Assets.root.Runtime.Cam.Settings;
using Assets.root.Runtime.Input.Handlers;
using UnityEngine;
using System;
using Assets.root.Runtime.Movement;

namespace Assets.root.Runtime.Cam.Handlers
{
    public class CameraRotationHandler : ICameraRotation
    {
        readonly Transform target;
        readonly Transform camT;
        readonly CameraRotationSettings rotationSettings;
        float targetPitch;
        float targetYaw;
        float desiredPitch;
        float desiredYaw;

        public CameraRotationHandler(Transform target, Transform camT, CameraRotationSettings rotationSettings)
        {
            this.target = target != null ? target : throw new ArgumentNullException(nameof(target));
            this.camT = camT != null ? camT : throw new ArgumentNullException(nameof(camT));
            this.rotationSettings = rotationSettings != null ? rotationSettings :
                throw new ArgumentNullException(nameof(rotationSettings));
        }

        public void HandleRotation(PlayerController input)
        {
            CalculateRotation(input);
            SmoothRotation();
            ApplyMovement();
        }

        void CalculateRotation(PlayerController input)
        {
            var deltaTimeMultiplier = input.InputServices.IsCurrentDeviceMouse() ? 1f : Time.deltaTime;

            desiredYaw += input.CameraInput.Look().x * rotationSettings.speedRotationX * deltaTimeMultiplier;
            desiredPitch -= input.CameraInput.Look().y * rotationSettings.speedRotationY * deltaTimeMultiplier;
            desiredPitch = RotationHelper.ClampAngle(desiredPitch, rotationSettings.bottomClampY, rotationSettings.topClampY);
        }

        void SmoothRotation()
        {
            targetYaw = Mathf.Lerp(targetYaw, desiredYaw, rotationSettings.smoothRotationX * Time.deltaTime);
            targetPitch = Mathf.Lerp(targetPitch, desiredPitch, rotationSettings.smoothRotationY * Time.deltaTime);
        }

        void ApplyMovement()
        {
            camT.eulerAngles = new Vector3(0f, targetYaw, 0f);
            target.localEulerAngles = new Vector3(targetPitch, 0f, 0f);
        }
    }
}