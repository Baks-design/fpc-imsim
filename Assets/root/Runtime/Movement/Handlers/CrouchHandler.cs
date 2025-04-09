using System;
using System.Threading;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Name;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class CrouchHandler : ICrouchHandler
    {
        readonly CrouchSettings settings;
        readonly CharacterController controller;
        CancellationTokenSource crouchTokenSource;
        Vector3 initCenter;
        Vector3 crouchCenter;
        float initHeight;
        float crouchHeight;
        float initCamHeight;
        float crouchCamHeight;

        public bool IsCrouching { get; private set; }
        public bool IsDuringCrouchAnimation { get; private set; }

        public CrouchHandler(CrouchSettings settings, CharacterController controller)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.controller = controller != null ? controller : throw new ArgumentNullException(nameof(controller));
            var camT = Camera.main.transform;
            var cam = camT != null ? camT : throw new ArgumentNullException(nameof(camT));

            InitializeCrouchParameters(cam);
        }

        void InitializeCrouchParameters(Transform camera)
        {
            initHeight = controller.height;
            initCenter = controller.center;
            initCamHeight = camera.localPosition.y;

            crouchHeight = Mathf.Max(0.1f, initHeight * settings.crouchPercent);
            crouchCenter = (crouchHeight / 2f + controller.skinWidth) * Vector3.up;
            crouchCamHeight = initCamHeight - (initHeight - crouchHeight);
        }

        public async void HandleCrouch(PlayerInputController input, Transform camera)
        {
            if (!input.MovementInput.CrouchWasPressed() || IsDuringCrouchAnimation || Time.deltaTime <= 0f)
                return;

            // Cancel any existing crouch animation
            crouchTokenSource?.Cancel();
            crouchTokenSource?.Dispose();

            await CrouchAnimationAsync(camera);
        }

        async Awaitable CrouchAnimationAsync(Transform camera)
        {
            crouchTokenSource = new CancellationTokenSource();
            var token = crouchTokenSource.Token;

            try
            {
                IsDuringCrouchAnimation = true;

                var percent = 0f;
                var speed = 1f / Mathf.Max(0.001f, settings.crouchTransitionDuration);

                var currentHeight = controller.height;
                var currentCenter = controller.center;
                var camPos = camera.localPosition;
                var camCurrentHeight = camPos.y;

                var desiredHeight = IsCrouching ? initHeight : crouchHeight;
                var desiredCenter = IsCrouching ? initCenter : crouchCenter;
                var camDesiredHeight = IsCrouching ? initCamHeight : crouchCamHeight;

                IsCrouching = !IsCrouching;

                while (percent < 1f)
                {
                    if (token.IsCancellationRequested)
                        break;

                    percent += Time.deltaTime * speed;
                    var smoothPercent = settings.crouchTransitionCurve.Evaluate(Mathf.Clamp01(percent));

                    // Apply smooth transitions
                    controller.height = Mathf.Lerp(currentHeight, desiredHeight, smoothPercent);
                    controller.center = Vector3.Lerp(currentCenter, desiredCenter, smoothPercent);

                    camPos.y = Mathf.Lerp(camCurrentHeight, camDesiredHeight, smoothPercent);
                    camera.localPosition = camPos;

                    await Awaitable.NextFrameAsync();
                }

                // Ensure final values are set exactly
                controller.height = desiredHeight;
                controller.center = desiredCenter;
                camPos.y = camDesiredHeight;
                camera.localPosition = camPos;
            }
            finally
            {
                IsDuringCrouchAnimation = false;
                crouchTokenSource?.Dispose();
                crouchTokenSource = null;
            }
        }
    }
}