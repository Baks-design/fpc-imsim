using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IGravityHandler
    {
        float InAirTimer { get; }
        Vector3 Gravity { get; }

        void UpdateInAirTimer(IGroundChecker groundChecker);
        void ApplyGravity(IGroundChecker groundChecker);
        void ResetGravity();
    }
}