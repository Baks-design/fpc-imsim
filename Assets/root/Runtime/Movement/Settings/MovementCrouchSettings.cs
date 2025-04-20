using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Crouch")]
    public class MovementCrouchSettings : ScriptableObject
    {
        public float CameraHeightRatio = 0.9f;
        public float CapsuleHeightStanding = 1.8f;
        public float CapsuleHeightCrouching = 0.9f;
        public float CrouchingSharpness = 10f;
    }
}