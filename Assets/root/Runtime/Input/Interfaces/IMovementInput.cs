using UnityEngine;

namespace Assets.root.Runtime.Input.Interfaces
{
    public interface IMovementInput
    {
        bool HasInput { get; }

        Vector2 Move();
        bool RunWasPressed();
        bool RunIsPressed();
        bool RunWasReleased();
        bool CrouchWasPressed();
        bool JumpWasPressed();
    }
}