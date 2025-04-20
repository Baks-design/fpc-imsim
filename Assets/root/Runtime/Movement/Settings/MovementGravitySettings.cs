using UnityEngine;

namespace Assets.root.Runtime.Movement.Settings
{
    [CreateAssetMenu(menuName = "Settings/Movement/Gravity")]
    public class MovementGravitySettings : ScriptableObject
    {
        public float GravityDownForce = 20f;
    }
}