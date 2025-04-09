using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IHeadBobHandler
    {
        Vector3 HeadBobOffset { get; }
        float CurrentStateHeight { get; }

        void UpdateHeadBob(IGroundChecker groundChecker, IWallChecker wallChecker,
        ICrouchHandler crouchHandler, Transform yawTransform);
        void ResetHeadBob();
        void SetHeadBobIntensity(float intensityMultiplier);
    }
}