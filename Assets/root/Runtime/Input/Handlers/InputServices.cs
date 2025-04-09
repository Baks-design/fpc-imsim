using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.root.Runtime.Input.Handlers
{
    public class InputServices : IInputServices
    {
        InputDevice currentDevice;
        bool isMouse;

        public void OnDeviceChangedSubscribe() => InputSystem.onDeviceChange += OnDeviceChange;

        public void OnDeviceChangedUnsubscribe() => InputSystem.onDeviceChange -= OnDeviceChange;

        void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    currentDevice = device;
                    isMouse = device is Mouse;
                    Debug.Log($"Device added: {device.displayName}");
                    break;

                case InputDeviceChange.Disconnected:
                    Debug.Log($"Device disconnected: {device.displayName}");
                    if (currentDevice == device)
                    {
                        currentDevice = null;
                        isMouse = false;
                    }
                    break;

                case InputDeviceChange.Reconnected:
                    if (currentDevice == null || currentDevice == device)
                    {
                        currentDevice = device;
                        isMouse = device is Mouse;
                    }
                    Debug.Log($"Device reconnected: {device.displayName}");
                    break;

                case InputDeviceChange.Removed:
                    Debug.Log($"Device removed: {device.displayName}");
                    if (currentDevice == device)
                    {
                        currentDevice = null;
                        isMouse = false;
                    }
                    break;
            }
        }

        public bool IsCurrentDeviceMouse() => isMouse;

        public void SetCursorState(bool lockCursor)
        {
#if UNITY_STANDALONE
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
#elif UNITY_EDITOR
            Cursor.lockState = lockCursor ? CursorLockMode.Confined : CursorLockMode.None;
#endif
        }
    }
}