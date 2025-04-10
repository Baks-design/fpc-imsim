using System;
using System.Threading;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{

    public class LandHandler : ILandHandler
    {
        readonly IGravityHandler gravityHandler;
        readonly MovementLandingSettings settings;
        readonly Transform yawTransform;
        CancellationTokenSource landTokenSource;

        public bool IsLanding { get; private set; }
        public float LastLandImpact { get; private set; }

        public LandHandler(IGravityHandler gravityHandler, MovementLandingSettings settings, Transform yawTransform)
        {
            this.gravityHandler = gravityHandler ?? throw new ArgumentNullException(nameof(gravityHandler));
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.yawTransform = yawTransform != null ? yawTransform : throw new ArgumentNullException(nameof(yawTransform));
        }

        public async void HandleLanding(IGroundChecker groundChecker) //FIXME
        {
            if (!ShouldTriggerLanding(groundChecker)) return;

            // Cancel any existing landing animation
            landTokenSource?.Cancel();
            landTokenSource?.Dispose();

            await LandingAnimationAsync();
        }

        bool ShouldTriggerLanding(IGroundChecker groundChecker)
        => !groundChecker.IsPreviouslyGrounded &&
            groundChecker.IsGrounded &&
            gravityHandler.InAirTimer > settings.minAirTime;

        async Awaitable LandingAnimationAsync()
        {
            landTokenSource = new CancellationTokenSource();
            var token = landTokenSource.Token;

            try
            {
                IsLanding = true;
                LastLandImpact = CalculateLandImpact();

                var percent = 0f;
                var landAmount = GetLandAmount();
                var speed = 1f / Mathf.Max(0.001f, settings.landDuration);

                var initialPosition = yawTransform.localPosition;
                var targetPosition = initialPosition;

                while (percent < 1f)
                {
                    if (token.IsCancellationRequested)
                        break;

                    percent += Time.deltaTime * speed;
                    var curveValue = settings.landCurve.Evaluate(Mathf.Clamp01(percent));

                    targetPosition.y = initialPosition.y + (curveValue * landAmount);
                    yawTransform.localPosition = targetPosition;

                    await Awaitable.NextFrameAsync();
                }

                // Ensure final position is reset exactly
                targetPosition.y = initialPosition.y;
                yawTransform.localPosition = targetPosition;
            }
            finally
            {
                IsLanding = false;
                landTokenSource?.Dispose();
                landTokenSource = null;
            }
        }

        float CalculateLandImpact() => Mathf.Clamp01(gravityHandler.InAirTimer / settings.maxAirTimeForImpact);

        float GetLandAmount()
        {
            var normalizedAirTime = Mathf.Clamp01(
                (gravityHandler.InAirTimer - settings.landTimer) /
                (settings.maxLandAirTime - settings.landTimer)
            );

            return Mathf.Lerp(
                settings.lowLandAmount,
                settings.highLandAmount,
                normalizedAirTime
            );
        }

        public void CancelLanding() => landTokenSource?.Cancel();
    }
}