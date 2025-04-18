
namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ICameraEffectsHandler
    {
        void UpdateCameraEffects(
            IWallChecker wallChecker,
            IMovementHandler movementHandler,
            PlayerController controller);
        void ResetCameraEffects();
    }
}