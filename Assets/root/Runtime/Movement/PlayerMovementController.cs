using UnityEngine;
using KBCore.Refs;
using Assets.root.RunMovement.Handlers;
using Assets.root.Runtime.Movement.Handlers;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Assets.root.Runtime.Cam;

namespace Assets.root.Runtime.Movement
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Parent] CharacterController character;
        [SerializeField, Parent] PlayerController controller;
        [SerializeField, Anywhere] PlayerCollisionController collision;
        [SerializeField, Anywhere] PlayerCameraController cam;
        [Header("Settings")]
        [SerializeField] MovementWalkSettings walkSettings;
        [SerializeField] MovementJumpSettings jumpSettings;
        [SerializeField] MovementRunSettings runSettings;
        [SerializeField] MovementCrouchSettings crouchSettings;
        [SerializeField] MovementLandingSettings landingSettings;
        [SerializeField] MovementGravitySettings gravitySettings;
        [SerializeField] MovementSmoothSettings smoothSettings;
        IGravityHandler GravityHandler;
        ILandHandler LandHandler;
        IRotationHandler RotationHandler;
        IJumpHandler JumpHandler;

        public IMovementHandler MovementHandler { get; private set; }
        public ICrouchHandler CrouchHandler { get; private set; }

        void Awake()
        {
            RotationHandler = new RotationHandler(character.transform, smoothSettings, cam.transform);
            CrouchHandler = new CrouchHandler(crouchSettings, character, cam.transform);
            GravityHandler = new GravityHandler(gravitySettings);
            LandHandler = new LandHandler(GravityHandler, landingSettings, cam.transform);
            JumpHandler = new JumpHandler(jumpSettings, CrouchHandler);
            MovementHandler = new MovementHandler(walkSettings, crouchSettings, runSettings,
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
            LandHandler.HandleLanding(collision.GroundChecker);
        }

        void HandleHorizontalVelocity()
        {
            CrouchHandler.HandleCrouch(controller);
            JumpHandler.HandleJump(collision.GroundChecker);
            MovementHandler.HandleMovement(controller);
            RotationHandler.HandleRotation();
        }
    }
}