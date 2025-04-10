using UnityEngine;
using KBCore.Refs;
using Assets.root.Runtime.Movement.Handlers;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using Assets.root.Runtime.Cam.Settings;
using Assets.root.Runtime.Cam;

namespace Assets.root.Runtime.Movement
{
    public class PlayerEffectsController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Parent] PlayerController controller;
        [SerializeField, Anywhere] PlayerMovementController movement;
        [SerializeField, Anywhere] PlayerCameraController look;
        [SerializeField, Anywhere] PlayerCollisionController collision;
        [Header("Settings")]
        [SerializeField] CameraSwaySettings cameraSwaySettings;
        [SerializeField] EffectHeadBobSettings headBobSettings;
        [SerializeField] MovementSmoothSettings smoothSettings;
        [SerializeField] MovementWalkSettings movementSettings;
        IHeadBobHandler HeadBobHandler;
        ICameraEffectsHandler CameraEffectsHandler;

        void Awake()
        {
            CameraEffectsHandler = new CameraEffectsHandler(look, cameraSwaySettings);
            HeadBobHandler = new HeadBobHandler(headBobSettings, smoothSettings, movementSettings);
        }

        void Update()
        {
            HeadBobHandler.UpdateHeadBob(
                collision.GroundChecker, collision.WallChecker, movement.CrouchHandler, look.transform, controller);
            CameraEffectsHandler.UpdateCameraEffects(collision.WallChecker, movement.MovementHandler, controller);
        }
    }
}