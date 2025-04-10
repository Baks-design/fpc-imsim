using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Jump")]
    public class MovementJumpSettings : ScriptableObject
    {
        public float jumpSpeed = 6f;
        public float coyoteTime = 1f;
        public float crouchJumpMultiplier = 1f;
        public float jumpBufferTime = 1f;
    }
}