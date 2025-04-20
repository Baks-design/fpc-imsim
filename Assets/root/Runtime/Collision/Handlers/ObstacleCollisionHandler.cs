using System;
using Assets.root.Runtime.Collision.Interfaces;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Utilities.Helpers;
using UnityEngine;

namespace Assets.root.Runtime.Collision.Handlers
{
    public class ObstacleCollisionHandler : IObstacleChecker
    {
        readonly CharacterController character;

        public bool IsHitObstacle { get; private set; }

        public ObstacleCollisionHandler(CharacterController character)
        => this.character = character != null ? character : throw new ArgumentNullException(nameof(character));

        public void UpdateObstacleCheck(IMovementHandler movementHandler)
        {
            IsHitObstacle = false;

            if (Physics.CapsuleCast(
                TransformHelper.GetCapsuleBottomHemisphere(character.transform, character.radius),
                TransformHelper.GetCapsuleTopHemisphere(character.transform, character.height, character.radius),
                character.radius,
                movementHandler.Velocity.normalized,
                out var hit,
                movementHandler.Velocity.magnitude * Time.deltaTime,
                -1,
                QueryTriggerInteraction.Ignore)
            )
            {
                IsHitObstacle = true;
                movementHandler.Velocity = Vector3.ProjectOnPlane(movementHandler.Velocity, hit.normal);
            }
        }
    }
}