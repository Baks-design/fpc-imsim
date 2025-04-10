namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ICrouchHandler
    {
        bool IsCrouching { get; }
        bool IsDuringCrouchAnimation { get; }

        void HandleCrouch(PlayerController input);
    }
}