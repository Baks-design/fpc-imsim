using UnityEngine;

namespace Assets.root.Runtime.Input.Interfaces
{
    public interface ICameraInput
    {
        Vector2 Look();
        bool ZoomWasPressed();
        bool ZoomIsPressed();
        bool ZoomWasReleased();
    }
}