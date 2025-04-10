namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ICeilChecker
    {
        bool IsHitCeil { get; }

        void UpdateCeilCheck();
        void DrawDebugGizmos();
    }
}