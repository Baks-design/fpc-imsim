using Assets.root.Runtime.Input.Interfaces;
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
        public bool ZoomWasPressed() => zoomAction.WasPressedThisDynamicUpdate();
        public bool ZoomIsPressed() => zoomAction.IsPressed();
        public bool ZoomWasReleased() => zoomAction.WasReleasedThisDynamicUpdate();
    }
}