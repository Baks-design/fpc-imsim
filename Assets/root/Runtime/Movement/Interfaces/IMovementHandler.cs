using Assets.root.Runtime.Collision;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IMovementHandler
    {
        bool IsRunning { get; set; }
        Vector3 Velocity { get; set; }

        void ApplyMove(PlayerCollisionController collision);
    }
}