using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class RotationHandler : IRotationHandler
    {
        readonly Transform player;
        readonly MovementSmoothSettings settings;
        readonly Transform cameraT;

        public RotationHandler(Transform player, MovementSmoothSettings settings, Transform cameraT)
        {
            this.player = player != null ? player : throw new ArgumentNullException(nameof(player));
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.cameraT = cameraT != null ? cameraT : throw new ArgumentNullException(nameof(cameraT));
        }

        public void HandleRotation()
        => player.rotation = Quaternion.Slerp(player.rotation, cameraT.rotation, Time.deltaTime * settings.smoothRotateSpeed);

        public void ResetRotation() => cameraT.localEulerAngles = Vector3.zero;
    }
}