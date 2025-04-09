using Name;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IJumpHandler
    {
        Vector3 JumpVelocity { get; }

        void UpdateJump(IGroundChecker groundChecker, PlayerInputController input);
        void HandleJump(IGroundChecker groundChecker);
        void ResetJump();
    }
}