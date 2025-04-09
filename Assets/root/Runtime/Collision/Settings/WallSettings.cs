using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class WallSettings
    {
        public LayerMask obstacleLayers = ~0;
        [Range(0f, 1f)] public float rayObstacleLength = 0.1f;
        [Range(0.01f, 1f)] public float rayObstacleSphereRadius = 0.1f;
        public float checkInterval = 0.1f;
    }
}