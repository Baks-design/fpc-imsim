using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Walk")]
    public class MovementWalkSettings : ScriptableObject
    {
        public float walkSpeed = 3f;
        [Range(0f, 1f)] public float moveBackwardsSpeedPercent = 0.5f;
        [Range(0f, 1f)] public float moveSideSpeedPercent = 0.7f;
    }
}