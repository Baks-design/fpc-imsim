using UnityEngine;
using KBCore.Refs;
using Assets.root.Runtime.Movement.States;
using Assets.root.Runtime.Utilities.Patterns.StateMachine;
using UnityServiceLocator;
using Assets.root.Runtime.Input.Interfaces;
using System;
using Assets.root.Runtime.Input.Handlers;

namespace Assets.root.Runtime.Movement
{
    public class PlayerController : StatefulEntity
    {
        [Header("Dependencies")]
        [SerializeField, Child] PlayerCollisionController collision;
        GroundedState groundedState;
        FallingState fallingState;
        JumpingState jumpingState;
        IMovementInput movementInput;
        ICameraInput cameraInput;
        IInputServices inputServices;

        public IMovementInput MovementInput => movementInput;
        public ICameraInput CameraInput => cameraInput;
        public IInputServices InputServices => inputServices;

        protected override void Awake()
        {
            base.Awake();
            InitializeStates();
            StateMachine();
        }

        void Start()
        {
            ServiceLocator.Global.Get(out movementInput);
            ServiceLocator.Global.Get(out cameraInput);
            ServiceLocator.Global.Get(out inputServices);
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
            At(groundedState, jumpingState, () => movementInput.JumpWasPressed());
            At(fallingState, groundedState, () => collision.GroundChecker.IsGrounded);
            At(jumpingState, fallingState, () => collision.CeilChecker.IsHitCeil);

            stateMachine.SetState(fallingState);
        }
    }
}