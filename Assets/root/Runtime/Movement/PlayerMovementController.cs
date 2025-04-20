using UnityEngine;
using KBCore.Refs;
using Assets.root.Runtime.Movement.Handlers;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Assets.root.Runtime.Input.Interfaces;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;
using Assets.root.Runtime.Input.Handlers;
using Assets.root.Runtime.Collision;
using Assets.root.Runtime.Look;
using Assets.root.Runtime.Damage;
using Assets.root.Runtime.Utilities.Helpers;

namespace Assets.root.Runtime.Movement
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Parent] CharacterController character;
        [SerializeField, Anywhere] PlayerCollisionController collision;
        [SerializeField, Anywhere] PlayerCameraController cam;
        [SerializeField, Anywhere] PlayerHealthController playerHealth;
        [SerializeField, Anywhere] Actor actor;
        [Header("Settings")]
        [SerializeField] MovementWalkSettings walkSettings;
        [SerializeField] MovementJumpSettings jumpSettings;
        [SerializeField] MovementCrouchSettings crouchSettings;
        [SerializeField] MovementLandingSettings landingSettings;
        [SerializeField] MovementGravitySettings gravitySettings;
        IGravityHandler GravityHandler;
        ILandHandler LandHandler;
        IRotationHandler RotationHandler;
        ICrouchHandler CrouchHandler;
        bool isCrouching = false;

        public IMovementInput MovementInput { get; private set; }
        public IJumpHandler JumpHandler { get; private set; }
        public IMovementHandler MovementHandler { get; private set; }

        void Awake()
        {
            RotationHandler = new RotationHandler(character.transform, cam.CameraYaw);
            MovementInput = new MovementInputHandler();
            CrouchHandler = new CrouchHandler(crouchSettings, character, cam.CameraYaw, actor.AimPoint);
            MovementHandler = new MovementHandler(walkSettings, MovementInput, CrouchHandler, character);
            JumpHandler = new JumpHandler(jumpSettings, MovementHandler);
            GravityHandler = new GravityHandler(gravitySettings, JumpHandler, MovementHandler);
            LandHandler = new LandHandler(landingSettings, MovementHandler, playerHealth);
        }

        void OnEnable()
        {
            MovementInput.Run().performed += OnRun;
            MovementInput.Run().canceled += OnRun;
            MovementInput.Jump().started += OnJump;
            MovementInput.Crouch().started += OnCrouch;
        }

        void OnDisable()
        {
            MovementInput.Run().performed -= OnRun;
            MovementInput.Run().canceled -= OnRun;
            MovementInput.Jump().started -= OnJump;
            MovementInput.Crouch().started -= OnCrouch;
        }

        void OnRun(CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    MovementHandler.IsRunning = true;
                    break;
                case InputActionPhase.Canceled:
                    MovementHandler.IsRunning = false;
                    break;
            }
        }

        void OnJump(CallbackContext context)
        {
            if (context.phase is InputActionPhase.Started && collision.GroundChecker.IsGrounded)
                JumpHandler.PerformJump();
        }

        void OnCrouch(CallbackContext context)
        {
            if (context.phase is InputActionPhase.Started && !collision.CeilChecker.IsHitCeil)
                CrouchHandler.ToggleCrouch();
        }

        void Start()
        {
            character.enableOverlapRecovery = true;

            CrouchHandler.SetCrouch(false);
            CrouchHandler.UpdateCharacterHeight(true);
        }

        void Update()
        {
            HandleVerticalVelocity();
            HandleHorizontalVelocity();
        }

        void HandleVerticalVelocity()
        {
            GravityHandler.ApplyGravity(collision);
            LandHandler.ApplyLanding(collision);
        }

        void HandleHorizontalVelocity()
        {
            MovementHandler.ApplyMove(collision);
            RotationHandler.HandleRotation();
            CrouchHandler.UpdateCharacterHeight(false);
        }
    }
}