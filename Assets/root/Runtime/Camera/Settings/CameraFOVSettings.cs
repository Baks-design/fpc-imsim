using System;
using UnityEngine;

namespace Assets.root.Runtime.Look.Settings
{
    [Serializable]
    public class CameraFOVSettings
    {
        [Header("Zoom Settings")]
        [Range(20f, 60f)] public float zoomFOV = 40f;
        public float zoomTransitionDuration = 0.25f;
        public AnimationCurve zoomCurve = new();
        [Header("Run Settings")]
        [Range(60f, 100f)] public float runFOV = 70f;
        public float runTransitionDuration = 0.75f;
        public float runReturnTransitionDuration = 0.5f;
        public AnimationCurve runCurve = new();
    }
}