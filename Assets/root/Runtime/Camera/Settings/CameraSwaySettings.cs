using System;
using UnityEngine;

namespace Assets.root.Runtime.Look.Settings
{
    [Serializable]
    public class CameraSwaySettings
    {
        public float swayAmount = 1f;
        public float swaySpeed = 1f;
        public float returnSpeed = 3f;
        public float swayTransitionTime = 0.5f;
        public float runSwayIntensity = 1f;
        public float defaultSwayIntensity = 0.5f;
        public float changeDirectionMultiplier = 4f;
        public AnimationCurve swayCurve = new();
    }
}