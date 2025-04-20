using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.root.Runtime.Input.Interfaces
{
    public interface IMovementInput
    {
        bool HasInput { get; }

        Vector2 Move();
        InputAction Run();
        InputAction Crouch();
        InputAction Jump();
    }
}