using UnityEngine;
using KBCore.Refs;
using Assets.root.Runtime.Movement.Handlers;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Assets.root.Runtime.Input.Handlers;
using Assets.root.Runtime.Look.Settings;
using Assets.root.Runtime.Look;

namespace Assets.root.Runtime.Movement
{
    public class PlayerEffectsController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Anywhere] PlayerMovementController movement;
        [SerializeField, Anywhere] PlayerLookController look;
        [SerializeField, Anywhere] PlayerCollisionController collision;

        IHeadBobHandler HeadBobHandler;
        ICameraEffectsHandler CameraEffectsHandler;

        void Awake()
        {
            CameraEffectsHandler = new CameraEffectsHandler(look, new MovementInputHandler(), new CameraSwaySettings());
            HeadBobHandler = new HeadBobHandler(new HeadBobSettings(), new SmoothSettings(),
            new MovementInputHandler(), new MovementSettings());
        }

        void Update()
        {
            HeadBobHandler.UpdateHeadBob(collision.GroundChecker, collision.WallChecker,
            movement.CrouchHandler, look.MainCamera.transform);
            CameraEffectsHandler.UpdateCameraEffects(collision.WallChecker, movement.MovementHandler);
        }
    }
}