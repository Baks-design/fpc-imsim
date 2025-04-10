namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ILandHandler
    {
        void HandleLanding(IGroundChecker groundChecker);
        void CancelLanding();
    }
}