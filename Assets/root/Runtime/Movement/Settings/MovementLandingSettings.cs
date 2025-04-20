using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Landing")]
    public class MovementLandingSettings : ScriptableObject
    {
        public float MinSpeedForFallDamage = 10f;
        public float MaxSpeedForFallDamage = 30f;
        public float FallDamageAtMinSpeed = 10f;
        public float FallDamageAtMaxSpeed = 50f;
        public bool ReceivesFallDamage = true;
    }
}