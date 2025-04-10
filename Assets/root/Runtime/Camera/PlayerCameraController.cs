using Assets.root.Runtime.Cam.Handlers;
using Assets.root.Runtime.Cam.Interfaces;
using Assets.root.Runtime.Cam.Settings;
using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;
using Assets.root.Runtime.Movement;

namespace Assets.root.Runtime.Cam
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Anywhere] Transform target;
        [SerializeField, Child] CinemachineCamera cinemachine;
        [SerializeField, Parent] PlayerController controller;
        [Header("Settings")]
        [SerializeField] CameraRotationSettings movementSettings;
        [SerializeField] CameraSwaySettings swaySettings;
        [SerializeField] CameraFOVSettings fOVSettings;
        ICameraRotation cameraRotation;
        ICameraFOV CameraFovHandler;
        ICameraSway CameraSwayHandler;

        void Awake()
        {
            CameraFovHandler = new CameraFOVHandler(cinemachine, fOVSettings);
            CameraSwayHandler = new CameraSwayHandler(target, swaySettings);
            cameraRotation = new CameraRotationHandler(target, transform, movementSettings);
        }

        void LateUpdate()
        {
            HandleRotation();
            HandleZoom();
        }

        void HandleRotation() => cameraRotation.HandleRotation(controller);

        void HandleZoom() => CameraFovHandler.ToggleZoom(controller);

        public void ChangeRunFOV(bool returning) => CameraFovHandler.ToggleRunFOV(returning);

        public void HandleSway(Vector3 input, float rawXInput) => CameraSwayHandler.Sway(input, rawXInput);
    }
}