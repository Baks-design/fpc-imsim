using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class SmoothSettings
    {
        public float rotationAcceleration = 1f;
        public float rotationDeceleration = 1f;
        public float rotationDeadzone = 0.1f;
        public float rotationThreshold = 0.1f;
        [Range(1f, 100f)] public float smoothRotateSpeed = 10f;
        [Range(1f, 100f)] public float smoothInputSpeed = 10f;
        [Range(1f, 100f)] public float smoothVelocitySpeed = 3f;
        [Range(1f, 100f)] public float smoothFinalDirectionSpeed = 10f;
        [Range(1f, 100f)] public float smoothHeadBobSpeed = 5f;
        public bool experimental = false;
        [Range(1f, 100f)] public float smoothInputMagnitudeSpeed = 5f;
    }
}