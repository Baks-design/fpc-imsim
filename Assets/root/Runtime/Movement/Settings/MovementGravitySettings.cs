using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Gravity")]
    public class MovementGravitySettings : ScriptableObject
    {
        public float gravityMultiplier = 2.5f;
        public float maxFallSpeed = 5f;
        public float stickToGroundForce = 1f;
    }
}