using System;
using Assets.root.Runtime.Collision;
using Assets.root.Runtime.Damage;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class LandHandler : ILandHandler
    {
        readonly MovementLandingSettings settings;
        readonly IMovementHandler movementHandler;
        readonly PlayerHealthController playerHealth;

        public Vector3 LatestImpactSpeed { get; set; }

        public event Action OnLand = delegate { };

        public LandHandler(
            MovementLandingSettings settings,
            IMovementHandler movementHandler,
            PlayerHealthController playerHealth)
        {
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.movementHandler = movementHandler ?? throw new ArgumentNullException(nameof(movementHandler));
            this.playerHealth = playerHealth != null ? playerHealth : throw new ArgumentNullException(nameof(playerHealth));
        }

        public void ApplyLanding(PlayerCollisionController collision)
        {
            // Check for landing
            if (collision.GroundChecker.IsGrounded && !collision.GroundChecker.WasGroundedLastFrame)
                ApplyLanding(movementHandler.Velocity.y);
            else if (!collision.GroundChecker.IsGrounded)
                // Record potential impact speed while falling
                RecordImpactSpeed(movementHandler.Velocity);

            collision.GroundChecker.WasGroundedLastFrame = collision.GroundChecker.IsGrounded;
        }

        void ApplyLanding(float fallSpeed)
        {
            // Use the stored impact speed
            var calculatedFallSpeed = -Mathf.Min(fallSpeed, LatestImpactSpeed.y);
            LatestImpactSpeed = Vector3.zero; // Reset after landing

            OnLand.Invoke();

            var fallSpeedRatio = Mathf.Clamp01(
                (calculatedFallSpeed - settings.MinSpeedForFallDamage) /
                (settings.MaxSpeedForFallDamage - settings.MinSpeedForFallDamage)
            );

            if (settings.ReceivesFallDamage && fallSpeedRatio > 0f)
            {
                var damage = Mathf.Lerp(settings.FallDamageAtMinSpeed, settings.FallDamageAtMaxSpeed, fallSpeedRatio);
                playerHealth.OnFall.Invoke(damage);
                //m_AudioSource.PlayOneShot(FallDamageSfx);
            }
            else if (calculatedFallSpeed > 2f) // Minimum speed for landing sound
            {
                //m_AudioSource.PlayOneShot(LandSfx);
            }
        }

        void RecordImpactSpeed(Vector3 velocity)
        {
            // Only record downward velocity
            if (velocity.y < LatestImpactSpeed.y)
                LatestImpactSpeed = velocity;
        }
    }
}