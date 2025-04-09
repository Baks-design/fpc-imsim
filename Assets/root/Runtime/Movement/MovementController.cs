using UnityEngine;
using KBCore.Refs;
using Assets.root.RunMovement.Handlers;
using Assets.root.Runtime.Movement.Handlers;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Assets.root.Runtime.Look;
using Name;

namespace Assets.root.Runtime.Movement
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Parent] CharacterController character;
        [SerializeField, Anywhere] PlayerCollisionController collision;
        [SerializeField, Anywhere] PlayerLookController look;
        [SerializeField, Anywhere] PlayerInputController input;
        [Header("Settings")]
        [SerializeField] MovementSettings movementSettings;
        [SerializeField] JumpSettings jumpSettings;
        [SerializeField] RunSettings runSettings;
        [SerializeField] CrouchSettings crouchSettings;
        [SerializeField] LandingSettings landingSettings;
        [SerializeField] GravitySettings gravitySettings;
        [SerializeField] SmoothSettings smoothSettings;

        public IMovementHandler MovementHandler { get; private set; }
        public ICrouchHandler CrouchHandler { get; private set; }
        IGravityHandler GravityHandler;
        ILandHandler LandHandler;
        IRotationHandler RotationHandler;
        IJumpHandler JumpHandler;

        void Awake()
        {
            RotationHandler = new RotationHandler(character.transform, smoothSettings);
            CrouchHandler = new CrouchHandler(crouchSettings, character);
            GravityHandler = new GravityHandler(gravitySettings);
            LandHandler = new LandHandler(GravityHandler, landingSettings);
            JumpHandler = new JumpHandler(jumpSettings, CrouchHandler);
            MovementHandler = new MovementHandler(movementSettings, crouchSettings, runSettings,
            GravityHandler, smoothSettings, CrouchHandler, character);
        }

        void Update()
        {
            HandleVerticalVelocity();
            HandleHorizontalVelocity();
        }

        void HandleVerticalVelocity()
        {
            GravityHandler.UpdateInAirTimer(collision.GroundChecker);
            GravityHandler.ApplyGravity(collision.GroundChecker);
            LandHandler.HandleLanding(collision.GroundChecker, look.MainCamera.transform);
        }

        void HandleHorizontalVelocity()
        {
            CrouchHandler.HandleCrouch(input, look.MainCamera.transform);
            JumpHandler.HandleJump(collision.GroundChecker);
            MovementHandler.HandleMovement(character.transform, input);
            RotationHandler.HandleRotation(look.MainCamera.transform);
        }
    }
}