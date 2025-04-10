using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Crouch")]
    public class MovementCrouchSettings : ScriptableObject
    {
        public float crouchSpeed = 1f;
        [Range(0.2f, 0.9f)] public float crouchPercent = 0.6f;
        public float crouchTransitionDuration = 0.5f;
        public AnimationCurve crouchTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
}