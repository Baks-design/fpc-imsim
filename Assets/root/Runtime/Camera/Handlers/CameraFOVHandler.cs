using System;
using System.Threading;
using Assets.root.Runtime.Look.Interfaces;
using Assets.root.Runtime.Look.Settings;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.root.Runtime.Look.Handlers
{
    [Serializable]
    public class CameraFOVHandler : ICameraFOV
    {
        readonly CameraFOVSettings fOVSettings;
        readonly float initFOV;
        CancellationTokenSource activeZoomToken;
        CancellationTokenSource activeRunToken;
        bool isRunning;
        bool isZooming;

        public float CurrentFOV { get; private set; }

        public CameraFOVHandler(CinemachineCamera camera, CameraFOVSettings fOVSettings)
        {
            this.fOVSettings = fOVSettings ?? throw new ArgumentNullException(nameof(fOVSettings));
            var cam = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            initFOV = cam.Lens.FieldOfView;
            CurrentFOV = initFOV;
        }

        public async void ToggleZoom()
        {
            if (isRunning) return;

            CancelActiveOperations();
            await ChangeFOVAsync();
        }

        public async void ToggleRunFOV(bool returning)
        {
            CancelActiveOperations();
            await ChangeRunFOVAsync(returning);
        }

        void CancelActiveOperations()
        {
            activeZoomToken?.Cancel();
            activeRunToken?.Cancel();
        }

        async Awaitable ChangeFOVAsync()
        {
            isZooming = !isZooming;
            activeZoomToken = new CancellationTokenSource();

            try
            {
                var percent = 0f;
                var speed = 1f / fOVSettings.zoomTransitionDuration;
                var currentFOV = CurrentFOV;
                var targetFOV = isZooming ? fOVSettings.zoomFOV : initFOV;

                while (percent < 1f)
                {
                    if (activeZoomToken.Token.IsCancellationRequested) break;

                    percent += Time.deltaTime * speed;
                    var smoothPercent = fOVSettings.zoomCurve.Evaluate(percent);
                    CurrentFOV = Mathf.Lerp(currentFOV, targetFOV, smoothPercent);
                    await Awaitable.NextFrameAsync();
                }

                // Ensure final value is set
                CurrentFOV = targetFOV;
            }
            finally
            {
                activeZoomToken?.Dispose();
                activeZoomToken = null;
            }
        }

        async Awaitable ChangeRunFOVAsync(bool returning)
        {
            isRunning = !returning;
            activeRunToken = new CancellationTokenSource();

            try
            {
                var percent = 0f;
                var duration = returning ? fOVSettings.runReturnTransitionDuration : fOVSettings.runTransitionDuration;
                var speed = 1f / duration;
                var currentFOV = CurrentFOV;
                var targetFOV = returning ? initFOV : fOVSettings.runFOV;

                while (percent < 1f)
                {
                    if (activeRunToken.Token.IsCancellationRequested) break;

                    percent += Time.deltaTime * speed;
                    var smoothPercent = fOVSettings.runCurve.Evaluate(percent);
                    CurrentFOV = Mathf.Lerp(currentFOV, targetFOV, smoothPercent);
                    await Awaitable.NextFrameAsync();
                }

                // Ensure final value is set
                CurrentFOV = targetFOV;
            }
            finally
            {
                activeRunToken?.Dispose();
                activeRunToken = null;
            }
        }

        public void ResetFOV() => CurrentFOV = initFOV;
    }
}