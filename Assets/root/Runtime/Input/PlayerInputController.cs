using Assets.root.Runtime.Input.Handlers;
using Assets.root.Runtime.Input.Interfaces;
using UnityEngine;

namespace Name
{
    public class PlayerInputController : MonoBehaviour
    {
        public ICameraInput CameraInput { get; private set; }
        public IInputServices InputServices { get; private set; }
        public IMovementInput MovementInput { get; private set; }

        void Awake()
        {
            CameraInput = new CameraInputHandler();
            InputServices = new InputServices();
            MovementInput = new MovementInputHandler();
        }
    }
}