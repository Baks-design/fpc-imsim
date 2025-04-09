using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ILandHandler
    {
        void HandleLanding(IGroundChecker groundChecker, Transform yawTransform);
        void CancelLanding();
    }
}