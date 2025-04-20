using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Utilities.Helpers;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class RotationHandler : IRotationHandler
    {
        readonly Transform player;
        readonly Transform camera;

        public RotationHandler(Transform player, Transform camera)
        {
            this.player = player != null ? player : throw new ArgumentNullException(nameof(player));
            this.camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
        }

        public void HandleRotation()
        {
            var targetRot = Quaternion.Euler(
                player.rotation.eulerAngles.x,
                camera.rotation.eulerAngles.y,
                player.rotation.eulerAngles.z
            );
            player.rotation = Mathfs.ExpRotDecay(player.rotation, targetRot, Time.deltaTime, 16f);
        }
    }
}