using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class GroundSettings
    {
        public bool useControllerFallback = true;
        public LayerMask groundLayer = ~0;
        [Range(0f, 1f)] public float rayLength = 0.1f;
        [Range(0.01f, 1f)] public float raySphereRadius = 0.1f;
        public float checkInterval = 0.1f;
        public float slopeLimit = 60f;
    }
}