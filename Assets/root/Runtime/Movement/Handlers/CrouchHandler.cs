using System;
using System.Threading;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class CrouchHandler : ICrouchHandler
    {
        readonly MovementCrouchSettings settings;
        readonly CharacterController character;
        readonly Transform camera;
        CancellationTokenSource crouchTokenSource;
        Vector3 initCenter;
        Vector3 crouchCenter;
        float initHeight;
        float crouchHeight;
        float initCamHeight;
        float crouchCamHeight;

        public bool IsCrouching { get; private set; }
        public bool IsDuringCrouchAnimation { get; private set; }

        public CrouchHandler(MovementCrouchSettings settings, CharacterController character, Transform camera)
        {
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.character = character != null ? character : throw new ArgumentNullException(nameof(character));
            this.camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));

            InitializeCrouchParameters(camera);
        }

        void InitializeCrouchParameters(Transform camera)
        {
            initHeight = character.height;
            initCenter = character.center;
            initCamHeight = camera.localPosition.y;

            crouchHeight = Mathf.Max(0.1f, initHeight * settings.crouchPercent);
            crouchCenter = (crouchHeight / 2f + character.skinWidth) * Vector3.up;
            crouchCamHeight = initCamHeight - (initHeight - crouchHeight);
        }

        public async void HandleCrouch(PlayerController input)
        {
            if (!input.MovementInput.CrouchWasPressed() || IsDuringCrouchAnimation || Time.deltaTime <= 0f)
                return;

            // Cancel any existing crouch animation
            crouchTokenSource?.Cancel();
            crouchTokenSource?.Dispose();

            await CrouchAnimationAsync();
        }

        async Awaitable CrouchAnimationAsync()
        {
            crouchTokenSource = new CancellationTokenSource();
            var token = crouchTokenSource.Token;

            try
            {
                IsDuringCrouchAnimation = true;

                var percent = 0f;
                var speed = 1f / Mathf.Max(0.001f, settings.crouchTransitionDuration);

                var currentHeight = character.height;
                var currentCenter = character.center;
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
                    character.height = Mathf.Lerp(currentHeight, desiredHeight, smoothPercent);
                    character.center = Vector3.Lerp(currentCenter, desiredCenter, smoothPercent);

                    camPos.y = Mathf.Lerp(camCurrentHeight, camDesiredHeight, smoothPercent);
                    camera.localPosition = camPos;

                    await Awaitable.NextFrameAsync();
                }

                // Ensure final values are set exactly
                character.height = desiredHeight;
                character.center = desiredCenter;
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