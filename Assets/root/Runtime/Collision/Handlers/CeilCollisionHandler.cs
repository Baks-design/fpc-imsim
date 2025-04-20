using System;
using Assets.root.Runtime.Collision.Interfaces;
using Assets.root.Runtime.Utilities.Helpers;
using UnityEngine;

namespace Assets.root.Runtime.Collision.Handlers
{
    public class CeilCollisionHandler : ICeilChecker
    {
        readonly CharacterController character;
        readonly Collider[] results = new Collider[5];
        const float StandingHeight = 1.8f;

        public bool IsHitCeil { get; private set; }

        public CeilCollisionHandler(CharacterController character)
        => this.character = character != null ? character : throw new ArgumentNullException(nameof(character));

        public bool UpdateCeilCheck()
        {
            var numOverlaps = Physics.OverlapCapsuleNonAlloc(
                TransformHelper.GetCapsuleBottomHemisphere(character.transform, character.radius),
                TransformHelper.GetCapsuleTopHemisphere(character.transform, StandingHeight, character.radius),
                character.radius,
                results,
                -1,
                QueryTriggerInteraction.Ignore
            );

            for (var i = 0; i < numOverlaps; i++)
                if (results[i] != character)
                    return false;

            return true;
        }
    }
}