using Assets.root.Runtime.Input.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.root.Runtime.Input.Handlers
{
    public class MovementInputHandler : IMovementInput
    {
        readonly InputAction moveAction;
        readonly InputAction runAction;
        readonly InputAction crouchAction;
        readonly InputAction jumpAction;

        public bool HasInput => Move() != Vector2.zero;

        public MovementInputHandler()
        {
            moveAction = InputSystem.actions.FindAction(InputsTags.MoveId);
            runAction = InputSystem.actions.FindAction(InputsTags.RunId);
            crouchAction = InputSystem.actions.FindAction(InputsTags.CrouchId);
            jumpAction = InputSystem.actions.FindAction(InputsTags.JumpId);
        }

        public Vector2 Move() => moveAction.ReadValue<Vector2>();
        public bool RunWasPressed() => runAction.WasPressedThisDynamicUpdate();
        public bool RunIsPressed() => runAction.IsPressed();
        public bool RunWasReleased() => runAction.WasReleasedThisDynamicUpdate();
        public bool CrouchWasPressed() => crouchAction.WasPressedThisDynamicUpdate();
        public bool JumpWasPressed() => jumpAction.WasPressedThisDynamicUpdate();
    }
}