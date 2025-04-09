using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public interface IBodyPushHandler
    {
        void PushRigidBodiesHandler(ControllerColliderHit hit);
    }
}