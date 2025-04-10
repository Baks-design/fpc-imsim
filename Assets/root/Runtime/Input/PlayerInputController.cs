using Assets.root.Runtime.Input.Handlers;
using Assets.root.Runtime.Input.Interfaces;
using UnityEngine;
using UnityServiceLocator;

namespace Name
{
    public class PlayerInputController : MonoBehaviour
    {
        IInputServices InputServices;

        void Awake()
        {
            InitializeClasses();
            RegisterServices();
        }

        void InitializeClasses() => InputServices = new InputServices();

        void RegisterServices()
        {
            ServiceLocator.Global.Register<ICameraInput>(new CameraInputHandler());
            ServiceLocator.Global.Register<IMovementInput>(new MovementInputHandler());
            ServiceLocator.Global.Register<IInputServices>(new InputServices());
        }

        void OnEnable() => InputServices.OnDeviceChangedSubscribe();

        void OnDisable() => InputServices.OnDeviceChangedUnsubscribe();

        void Start() => InputServices.SetCursorState(true);
    }
}