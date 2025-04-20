using Assets.root.Runtime.Collision;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IGravityHandler
    {
        void ApplyGravity(PlayerCollisionController collision);
    }
}