using System;
using Assets.root.Runtime.Collision.Interfaces;
using Assets.root.Runtime.Collision.Settings;
using Assets.root.Runtime.Utilities.Helpers;
using UnityEngine;

namespace Assets.root.Runtime.Collision.Handlers
{
    public class GroundCollisionHandler : IGroundChecker
    {
        readonly GroundCheckSettings settings;
        readonly CharacterController character;
        const float k_GroundCheckDistanceInAir = 0.07f;

        public bool IsLanded => IsGrounded && !WasGroundedLastFrame;
        public bool IsGrounded { get; private set; }
        public bool WasGroundedLastFrame { get; set; }
        public Vector3 GroundNormal { get; private set; }

        public GroundCollisionHandler(GroundCheckSettings settings, CharacterController character)
        {
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.character = character != null ? character : throw new ArgumentNullException(nameof(character));
        }

        public void UpdateGroundCheck()
        {
            WasGroundedLastFrame = IsGrounded;

            var checkDistance = IsGrounded
                ? (character.skinWidth + settings.GroundCheckDistance)
                : k_GroundCheckDistanceInAir;

            IsGrounded = false;
            GroundNormal = Vector3.up;

            if (Physics.CapsuleCast(
                TransformHelper.GetCapsuleBottomHemisphere(character.transform, character.radius),
                TransformHelper.GetCapsuleTopHemisphere(character.transform, character.height, character.radius),
                character.radius,
                Vector3.down,
                out var hit,
                checkDistance,
                settings.GroundCheckLayers,
                QueryTriggerInteraction.Ignore))
            {
                GroundNormal = hit.normal;

                if (Vector3.Dot(hit.normal, character.transform.up) > 0f &&
                    TransformHelper.IsNormalUnderSlopeLimit(character.transform, GroundNormal, character.slopeLimit))
                {
                    IsGrounded = true;

                    if (hit.distance > character.skinWidth)
                        character.Move(Vector3.down * hit.distance);
                }
            }
        }
    }
}