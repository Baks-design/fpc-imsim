namespace Assets.root.Runtime.Look.Interfaces
{
    public interface ICameraFOV
    {
        float CurrentFOV { get; }

        void ToggleZoom();
        void ToggleRunFOV(bool returning);
        void ResetFOV();
    }
}