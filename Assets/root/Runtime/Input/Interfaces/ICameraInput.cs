using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.root.Runtime.Input.Interfaces
{
    public interface ICameraInput
    {
        Vector2 Look();
        InputAction Zoom();
    }
}