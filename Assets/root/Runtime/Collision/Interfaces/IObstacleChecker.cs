using Assets.root.Runtime.Movement.Interfaces;

namespace Assets.root.Runtime.Collision.Interfaces
{
    public interface IObstacleChecker
    {
        bool IsHitObstacle { get; }

        void UpdateObstacleCheck(IMovementHandler movementHandler);
    }
}