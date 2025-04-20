using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Jump")]
    public class MovementJumpSettings : ScriptableObject
    {
        public float JumpForce = 30f;
    }
}