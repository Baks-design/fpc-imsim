using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class LandingSettings
    {
        [Range(0.05f, 0.5f)] public float lowLandAmount = 0.1f;
        [Range(0.2f, 0.9f)] public float highLandAmount = 0.4f;
        public float landTimer = 0.5f;
        public float landDuration = 0.5f;
        public float maxLandAirTime = 0.1f;
        public float maxAirTimeForImpact = 0.1f;
        public float minAirTime = 0.1f;
        public AnimationCurve landCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
}