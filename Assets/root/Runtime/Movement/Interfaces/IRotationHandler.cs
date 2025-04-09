using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IRotationHandler
    {
        void HandleRotation(Transform camera);
        void ResetRotation(Transform camera);
    }
}