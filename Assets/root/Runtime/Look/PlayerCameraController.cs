using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.root.Runtime.Input.Interfaces;
using Assets.root.Runtime.Input.Handlers;
using static UnityEngine.InputSystem.InputAction;
using Assets.root.Runtime.Look.Settings;
using Assets.root.Runtime.Look.Interfaces;
using Assets.root.Runtime.Look.Handlers;

namespace Assets.root.Runtime.Look
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Child] CinemachineCamera cinemachine;
        [SerializeField, Anywhere] Transform pitch;
        [SerializeField, Anywhere] Transform yaw;
        [Header("Settings")]
        [SerializeField] CameraRotationSettings movementSettings;
        [SerializeField] CameraFOVSettings fOVSettings;
        ICameraInput input;
        ICameraRotation CameraRotation;

        public Transform CameraYaw => yaw;
        public ICameraFOV CameraFovHandler { get; private set; }

        void Awake()
        {
            input = new CameraInputHandler();
            CameraFovHandler = new CameraFOVHandler(cinemachine, fOVSettings);
            CameraRotation = new CameraRotationHandler(movementSettings, input, pitch, yaw);
        }

        void OnEnable()
        {
            input.Zoom().started += OnZoom;
            input.Zoom().canceled += OnZoom;
        }

        void OnDisable()
        {
            input.Zoom().started -= OnZoom;
            input.Zoom().canceled -= OnZoom;
        }

        async void OnZoom(CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    await CameraFovHandler.ToggleZoom(true);
                    break;
                case InputActionPhase.Canceled:
                    await CameraFovHandler.ToggleZoom(false);
                    break;
            }
        }

        void LateUpdate() => CameraRotation.HandleRotation();
    }
}