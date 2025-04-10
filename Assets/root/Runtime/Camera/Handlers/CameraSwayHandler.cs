using System;
using Assets.root.Runtime.Cam.Interfaces;
using Assets.root.Runtime.Cam.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Cam.Handlers
{
    public class CameraSwayHandler : ICameraSway
    {
        readonly Transform camTransform;
        readonly CameraSwaySettings swaySettings;
        bool differentDirection;
        float scrollSpeed;
        float xAmountThisFrame;
        float xAmountPreviousFrame;

        public CameraSwayHandler(Transform camTransform, CameraSwaySettings swaySettings)
        {
            this.camTransform = camTransform != null ? camTransform : throw new ArgumentNullException(nameof(camTransform));
            this.swaySettings = swaySettings != null ? swaySettings : throw new ArgumentNullException(nameof(swaySettings));
        }

        public void Sway(Vector3 inputVector, float rawXInput) //FIXME
        {
            xAmountThisFrame = rawXInput;

            if (rawXInput != 0f)
            {
                if (xAmountThisFrame != xAmountPreviousFrame && xAmountPreviousFrame != 0f)
                    differentDirection = true;

                var speedMultiplier = differentDirection ? swaySettings.changeDirectionMultiplier : 1f;
                scrollSpeed += inputVector.x * swaySettings.swaySpeed * Time.deltaTime * speedMultiplier;
            }
            else
            {
                if (xAmountThisFrame == xAmountPreviousFrame)
                    differentDirection = false;

                scrollSpeed = Mathf.Lerp(scrollSpeed, 0f, Time.deltaTime * swaySettings.returnSpeed);
            }

            scrollSpeed = Mathf.Clamp(scrollSpeed, -1f, 1f);
            var swayFinalAmount = CalculateSwayAmount(scrollSpeed);
            ApplySway(swayFinalAmount);

            xAmountPreviousFrame = xAmountThisFrame;
        }

        float CalculateSwayAmount(float speed)
        {
            if (speed < 0f) return -swaySettings.swayCurve.Evaluate(-speed) * -swaySettings.swayAmount;
            return swaySettings.swayCurve.Evaluate(speed) * -swaySettings.swayAmount;
        }

        void ApplySway(float swayAmount)
        {
            var angles = camTransform.localEulerAngles;
            angles.z = swayAmount;
            camTransform.localEulerAngles = angles;
        }

        public void ResetSway() => camTransform.localEulerAngles = Vector3.zero;
    }
}