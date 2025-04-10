using Assets.root.Runtime.Movement;

namespace Assets.root.Runtime.Cam.Interfaces
{
    public interface ICameraFOV
    {
        float CurrentFOV { get; }

        void ToggleZoom(PlayerController input);
        void ToggleRunFOV(bool returning);
        void ResetFOV();
    }
}