using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Walk")]
    public class MovementWalkSettings : ScriptableObject
    {
        public float MaxSpeedOnGround = 10f;
        public float MovementSharpnessOnGround = 15;
        public float MaxSpeedCrouchedRatio = 0.5f;
        public float MaxSpeedInAir = 10f;
        public float AccelerationSpeedInAir = 25f;
        public float SprintSpeedModifier = 2f;
    }
}