namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface IWallChecker
    {
        bool IsHitWall { get; }
       
        void UpdateWallCheck();
        void DrawDebugGizmos();
    }
}