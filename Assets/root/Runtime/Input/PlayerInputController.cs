using Assets.root.Runtime.Input.Handlers;
using UnityEngine;

namespace Assets.root.Runtime.Input
{
    public class PlayerInputController : MonoBehaviour
    {
        IInputServices InputServices;

        void Awake()
        {
            InitializeClasses();
            SetCursor();
        }

        void InitializeClasses() => InputServices = new InputServices();

        void SetCursor() => InputServices.SetCursorState(true);
    }
}