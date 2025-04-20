namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IJumpHandler
    {
        bool HasJumpedThisFrame { get; }
        float LastTimeJumped { get; }

        void PerformJump();
    }
}