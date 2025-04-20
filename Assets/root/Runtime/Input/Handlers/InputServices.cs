using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.root.Runtime.Input.Handlers
{
    public class InputServices : IInputServices
    {
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