using UnityEngine;

namespace Assets.root.Runtime.Collision.Settings
{
    [CreateAssetMenu(menuName = "Settings/Collision/GroundCheck")]
    public class GroundCheckSettings : ScriptableObject
    {
        public LayerMask GroundCheckLayers = -1;
        public float GroundCheckDistance = 0.05f;
    }
}