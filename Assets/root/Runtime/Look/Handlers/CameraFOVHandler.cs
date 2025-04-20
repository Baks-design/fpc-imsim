using System;
using System.Threading;
using Assets.root.Runtime.Look.Interfaces;
using Assets.root.Runtime.Look.Settings;
using Assets.root.Runtime.Utilities.Helpers;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.root.Runtime.Look.Handlers
{
    public class CameraFOVHandler : ICameraFOV, IDisposable
    {
        readonly CameraFOVSettings fOVSettings;
        readonly CinemachineCamera camera;
        readonly float initFOV;
        CancellationTokenSource activeRunToken;
        CancellationTokenSource activeZoomToken;
        bool isRunning;
        bool isZooming;

        public CameraFOVHandler(CinemachineCamera camera, CameraFOVSettings fOVSettings)
        {
            this.fOVSettings = fOVSettings != null ? fOVSettings : throw new ArgumentNullException(nameof(fOVSettings));
            this.camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            initFOV = camera.Lens.FieldOfView;
        }

        public async Awaitable ToggleZoom(bool zoomIn)
        {
            if (isRunning) return;
            if (zoomIn == isZooming) return;

            // Cancel any ongoing zoom operation
            activeZoomToken?.Cancel();
            await ChangeFOVAsync();
        }

        public async Awaitable ToggleRunFOV(bool returning)
        {
            if (returning == isRunning) return;

            // Cancel any ongoing run FOV operation
            activeRunToken?.Cancel();
            await ChangeRunFOVAsync(returning);
        }

        async Awaitable ChangeFOVAsync()
        {
            isZooming = !isZooming;
            activeZoomToken?.Dispose();
            activeZoomToken = new CancellationTokenSource();

            try
            {
                var percent = 0f;
                var speed = 1f / fOVSettings.zoomTransitionDuration;
                var startFOV = camera.Lens.FieldOfView;
                var targetFOV = isZooming ? fOVSettings.zoomFOV : initFOV;

                while (percent < 1f)
                {
                    if (activeZoomToken.IsCancellationRequested) break;

                    percent += Time.deltaTime * speed;
                    var smoothPercent = fOVSettings.zoomCurve.Evaluate(percent);
                    camera.Lens.FieldOfView = Mathfs.Eerp(startFOV, targetFOV, smoothPercent);

                    await Awaitable.NextFrameAsync();
                }

                // Ensure we reach the exact target value
                if (!activeZoomToken.IsCancellationRequested)
                    camera.Lens.FieldOfView = targetFOV;
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
            activeRunToken?.Dispose();
            activeRunToken = new CancellationTokenSource();

            try
            {
                var percent = 0f;
                var duration = returning ? fOVSettings.runReturnDuration : fOVSettings.runTransitionDuration;
                var speed = 1f / duration;
                var startFOV = camera.Lens.FieldOfView;
                var targetFOV = returning ? initFOV : fOVSettings.runFOV;

                while (percent < 1f)
                {
                    if (activeRunToken.IsCancellationRequested) break;

                    percent += Time.deltaTime * speed;
                    var smoothPercent = fOVSettings.runCurve.Evaluate(percent);
                    camera.Lens.FieldOfView = Mathfs.Eerp(startFOV, targetFOV, smoothPercent);

                    await Awaitable.NextFrameAsync();
                }

                // Ensure we reach the exact target value
                if (!activeRunToken.IsCancellationRequested)
                    camera.Lens.FieldOfView = targetFOV;
            }
            finally
            {
                activeRunToken?.Dispose();
                activeRunToken = null;
            }
        }

        public void Dispose()
        {
            activeZoomToken?.Cancel();
            activeZoomToken?.Dispose();
            activeRunToken?.Cancel();
            activeRunToken?.Dispose();
        }
    }
}