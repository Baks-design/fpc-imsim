using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class CeilSettings
    {
        public LayerMask ceilingLayers = ~0;
        public float checkDistance = 0.1f;
        public float checkRadius = 0.2f;
        public float checkInterval = 0.1f;
    }
}