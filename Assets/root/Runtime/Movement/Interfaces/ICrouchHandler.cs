using Name;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ICrouchHandler
    {
        bool IsCrouching { get; }
        bool IsDuringCrouchAnimation { get; }

        void HandleCrouch(PlayerInputController input, Transform camera);
    }
}