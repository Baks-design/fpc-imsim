using System;
using Assets.root.Runtime.Movement.Interfaces;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class CeilCollisionHandler : ICeilChecker
    {
        readonly CharacterController controller;

        public bool IsHitCeil { get; private set; }

        public CeilCollisionHandler(CharacterController controller)
        => this.controller = controller != null ? controller : throw new ArgumentNullException(nameof(controller));

        public void UpdateCeilCheck() => IsHitCeil = CustomCharacterPhysics.CheckHeadroom(controller, out var _);

        public void DrawDebugGizmos()
        {
            var rayStart = controller.transform.position + Vector3.up * (controller.height - controller.radius);
            var radius = controller.radius * 0.9f;
            Gizmos.color = IsHitCeil ? Color.red : Color.green;
            Gizmos.DrawWireSphere(rayStart, radius);
        }
    }
}