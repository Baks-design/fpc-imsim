using System;
using Assets.root.Runtime.Movement.Interfaces;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class WallCollisionHandler : IWallChecker
    {
        readonly CharacterController controller;

        public bool IsHitWall { get; private set; }

        public WallCollisionHandler(CharacterController controller)
        => this.controller = controller != null ? controller : throw new ArgumentNullException(nameof(controller));

        public void UpdateWallCheck()
        => IsHitWall = CustomCharacterPhysics.CheckMovement(controller, controller.transform.forward, out var _);

        public void DrawDebugGizmos()
        {
            var rayStart = controller.transform.position + Vector3.up * (controller.height * 0.5f);
            var radius = controller.radius - 0.1f;
            Gizmos.color = IsHitWall ? Color.red : Color.green;
            Gizmos.DrawWireSphere(rayStart, radius);
        }
    }
}