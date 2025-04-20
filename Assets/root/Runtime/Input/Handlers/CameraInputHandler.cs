using Assets.root.Runtime.Input.Interfaces;
using Assets.root.Runtime.Utilities.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.root.Runtime.Input.Handlers
{
    public class CameraInputHandler : ICameraInput
    {
        readonly InputAction lookAction;
        readonly InputAction zoomAction;
    
        public CameraInputHandler()
        {
            lookAction = InputSystem.actions.FindAction(InputsTags.Look);
            zoomAction = InputSystem.actions.FindAction(InputsTags.Zoom);
        }

        public Vector2 Look() => lookAction.ReadValue<Vector2>();
        public InputAction Zoom() => zoomAction;
    }
}