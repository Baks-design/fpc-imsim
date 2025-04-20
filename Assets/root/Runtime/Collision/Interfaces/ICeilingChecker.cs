namespace Assets.root.Runtime.Collision.Interfaces
{
    public interface ICeilChecker
    {
        bool IsHitCeil { get; }

        bool UpdateCeilCheck();
    }
}