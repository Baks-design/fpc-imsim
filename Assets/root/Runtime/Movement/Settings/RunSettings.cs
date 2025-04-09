using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class RunSettings
    {
        public float runSpeed = 6f;
        [Range(-1f, 1f)] public float canRunThreshold = 0.7f;
        public AnimationCurve runTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
}