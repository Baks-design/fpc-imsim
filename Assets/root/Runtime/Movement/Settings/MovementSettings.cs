using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class MovementSettings
    {
        public float walkSpeed = 3f;
        [Range(0f, 1f)] public float moveBackwardsSpeedPercent = 0.5f;
        [Range(0f, 1f)] public float moveSideSpeedPercent = 0.7f;
    }
}