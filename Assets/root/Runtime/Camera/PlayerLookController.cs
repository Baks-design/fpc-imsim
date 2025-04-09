using Assets.root.Runtime.Look.Handlers;
using Assets.root.Runtime.Look.Interfaces;
using Assets.root.Runtime.Look.Settings;
using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;
using System;
using Assets.root.Runtime.Input.Interfaces;
using Assets.root.Runtime.Input.Handlers;

namespace Assets.root.Runtime.Look
{
    public class PlayerLookController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Anywhere] Transform target;
        [SerializeField, Child] CinemachineCamera cinemachine;
        [Header("Settings")]
        [SerializeField] CameraRotationSettings movementSettings;
        [SerializeField] CameraSwaySettings swaySettings;
        [SerializeField] CameraFOVSettings fOVSettings;
        ICameraRotation cameraRotation;
        ICameraFOV CameraFovHandler;
        ICameraSway CameraSwayHandler;
        ICameraInput InputCameraHandler;
        IInputServices InputCameraServices;

        public Camera MainCamera { get; private set; }

        void Awake()
        {
            GetVars();
            InitializationComponents();
        }

        void GetVars() => MainCamera = Camera.main != null ? Camera.main : throw new ArgumentNullException(nameof(Camera.main));

        void InitializationComponents()
        {
            InputCameraHandler = new CameraInputHandler();
            InputCameraServices = new InputServices();
            CameraFovHandler = new CameraFOVHandler(cinemachine, fOVSettings);
            CameraSwayHandler = new CameraSwayHandler(target, swaySettings);
            cameraRotation = new CameraRotationHandler(target, InputCameraHandler, InputCameraServices, movementSettings);
        }

        void Start() => InputCameraServices.SetCursorState(true);

        void LateUpdate()
        {
            HandleRotation();
            HandleZoom();
        }

        void HandleRotation() => cameraRotation.HandleRotation();

        void HandleZoom()
        {
            if (!InputCameraHandler.ZoomIsPressed() && !InputCameraHandler.ZoomWasReleased()) return;
            CameraFovHandler.ToggleZoom();
        }

        public void ChangeRunFOV(bool returning) => CameraFovHandler.ToggleRunFOV(returning);

        public void HandleSway(Vector3 input, float rawXInput) => CameraSwayHandler.Sway(input, rawXInput);
    }
}