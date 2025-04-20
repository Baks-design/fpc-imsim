using System;
using UnityEngine;

namespace Assets.root.Runtime.Look.Settings
{
    [CreateAssetMenu(menuName = "Settings/Look/FOV")]
    public class CameraFOVSettings : ScriptableObject
    {
        [Header("Zoom Settings")]
        [Range(20f, 60f)] public float zoomFOV = 40f;
        public float zoomTransitionDuration = 0.25f;
        public AnimationCurve zoomCurve = new();
        [Header("Run Settings")]
        [Range(60f, 100f)] public float runFOV = 70f;
        public float runTransitionDuration = 0.75f;
        public float runReturnDuration = 0.5f;
        public AnimationCurve runCurve = new();
    }
}