namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IGroundChecker
    {
        bool IsGrounded { get; }
        bool IsPreviouslyGrounded { get; }

        void UpdateGroundCheck();
        void DrawDebugGizmos();
    }
}