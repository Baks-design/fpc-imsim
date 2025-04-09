using System;

namespace Assets.root.Runtime.Movement.Settings
{
    [Serializable]
    public class JumpSettings
    {
        public float jumpSpeed = 6f;
        public float coyoteTime = 1f;
        public float crouchJumpMultiplier = 1f;
        public float jumpBufferTime = 1f;
    }
}