using UnityEngine;

namespace Assets.root.Runtime.Look.Interfaces
{
    public interface ICameraFOV
    {
        Awaitable ToggleZoom(bool zoomIn);
        Awaitable ToggleRunFOV(bool runIn);
    }
}