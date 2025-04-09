using UnityEngine;
using KBCore.Refs;
using Assets.root.Runtime.Movement.States;
using Assets.root.Runtime.Utilities.Patterns.StateMachine;
using Name;

namespace Assets.root.Runtime.Movement
{
    public class PlayerController : StatefulEntity
    {
        [Header("Dependencies")]
        [SerializeField, Child] PlayerInputController input;
        [SerializeField, Child] PlayerCollisionController collision;
        GroundedState groundedState;
        FallingState fallingState;
        JumpingState jumpingState;

        protected override void Awake()
        {
            base.Awake();
            InitializeStates();
            StateMachine();
        }

        void InitializeStates()
        {
            groundedState = new GroundedState(this);
            fallingState = new FallingState(this);
            jumpingState = new JumpingState(this);
        }

        void StateMachine()
        {
            At(groundedState, fallingState, () => !collision.GroundChecker.IsGrounded);
            At(groundedState, jumpingState, () => input.MovementInput.JumpWasPressed());
            At(fallingState, groundedState, () => collision.GroundChecker.IsGrounded);
            At(jumpingState, fallingState, () => collision.CeilChecker.IsNearCeil);

            stateMachine.SetState(fallingState);
        }
    }
}