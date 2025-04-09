using Name;
using UnityEngine;
using static Assets.root.RunMovement.Handlers.MovementHandler;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IMovementHandler
    {
        Vector3 FinalMove { get; }
        float CurrentSpeed { get; }
        public MovementState CurrentState { get; }

        void HandleMovement(Transform transform, PlayerInputController input);
        void ResetMovement();
    }
}