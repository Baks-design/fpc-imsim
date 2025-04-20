using UnityEngine;

namespace Assets.root.Runtime.Collision.Interfaces
{
    public interface IGroundChecker
    {
        bool IsLanded { get; }
        bool IsGrounded { get; }
        bool WasGroundedLastFrame { get; set; }
        Vector3 GroundNormal { get; }

        void UpdateGroundCheck();
    }
}