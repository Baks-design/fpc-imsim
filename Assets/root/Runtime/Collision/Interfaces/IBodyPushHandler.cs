using UnityEngine;

namespace Assets.root.Runtime.Collision.Interfaces
{
    public interface IBodyPushHandler
    {
        void PushRigidBodiesHandler(ControllerColliderHit hit);
    }
}