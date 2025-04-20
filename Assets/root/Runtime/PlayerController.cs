using UnityEngine;
using KBCore.Refs;
using Assets.root.Runtime.Movement.States;
using Assets.root.Runtime.Utilities.Patterns.StateMachine;
using Assets.root.Runtime.Collision;
using Assets.root.Runtime.Movement;

namespace Assets.root.Runtime
{
    public class PlayerController : StatefulEntity
    {
        [Header("Dependencies")]
        [SerializeField, Child] PlayerCollisionController collision;
        [SerializeField, Child] PlayerMovementController movement;
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
            At(groundedState, jumpingState, () => movement.JumpHandler.HasJumpedThisFrame);
            At(fallingState, groundedState, () => collision.GroundChecker.IsLanded);
            At(jumpingState, fallingState, () => collision.CeilChecker.IsHitCeil);

            stateMachine.SetState(fallingState);
        }
    }
}