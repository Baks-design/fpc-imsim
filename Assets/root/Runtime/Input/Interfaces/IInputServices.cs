namespace Assets.root.Runtime.Input.Handlers
{
    public interface IInputServices
    {
        bool IsCurrentDeviceMouse();
        void OnDeviceChangedSubscribe();
        void OnDeviceChangedUnsubscribe();
        void SetCursorState(bool lockCursor);
    }
}