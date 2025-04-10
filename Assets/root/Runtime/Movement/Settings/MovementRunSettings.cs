using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Run")]
    public class MovementRunSettings : ScriptableObject
    {
        public float runSpeed = 6f;
        [Range(-1f, 1f)] public float canRunThreshold = 0.7f;
        public AnimationCurve runTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
}