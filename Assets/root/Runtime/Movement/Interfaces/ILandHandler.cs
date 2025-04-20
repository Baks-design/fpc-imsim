using Assets.root.Runtime.Collision;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ILandHandler
    {
        Vector3 LatestImpactSpeed { get; set; }

        void ApplyLanding(PlayerCollisionController collision);
    }
}