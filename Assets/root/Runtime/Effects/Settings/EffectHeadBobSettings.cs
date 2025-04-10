using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Effects/HeadBob")]
    public class EffectHeadBobSettings : ScriptableObject
    {
        public float bobSpeedTransitionSharpness = 1f;
        public float walkBobSpeed = 4f;
        public float runBobSpeed = 8f;
        public AnimationCurve xCurve;
        public AnimationCurve yCurve;
        public float xAmplitude = 0.05f;
        public float yAmplitude = 0.1f;
        public float xFrequency = 2f;
        public float yFrequency = 4f;
        public float runAmplitudeMultiplier = 1.5f;
        public float runFrequencyMultiplier = 1.5f;
        public float crouchAmplitudeMultiplier = 0.2f;
        public float crouchFrequencyMultiplier = 1f;
    }
}