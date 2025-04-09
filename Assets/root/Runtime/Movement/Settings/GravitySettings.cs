using System;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class GravitySettings
    {
        public float gravityMultiplier = 2.5f;
        public float maxFallSpeed = 5f;
        public float stickToGroundForce = 1f;
    }
}