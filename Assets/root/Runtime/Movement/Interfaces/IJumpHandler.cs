using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IJumpHandler
    {
        Vector3 JumpVelocity { get; }

        void UpdateJump(IGroundChecker groundChecker, PlayerController input);
        void HandleJump(IGroundChecker groundChecker);
        void ResetJump();
    }
}