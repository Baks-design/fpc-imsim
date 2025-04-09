using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class RotationHandler : IRotationHandler
    {
        readonly Transform player;
        readonly SmoothSettings settings;

        public RotationHandler(Transform player, SmoothSettings settings)
        {
            this.player = player != null ? player : throw new ArgumentNullException(nameof(player));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void HandleRotation(Transform camera)
        {
            var currentRot = Quaternion.Euler(0f, player.rotation.y, 0f);
            var desiredRot = Quaternion.Euler(0f, camera.rotation.y, 0f);
            player.rotation = Quaternion.Slerp(currentRot, desiredRot, Time.deltaTime * settings.smoothRotateSpeed);
        }

        public void ResetRotation(Transform camera) => camera.localEulerAngles = Vector3.zero;
    }
}