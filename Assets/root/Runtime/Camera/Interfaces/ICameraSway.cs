using UnityEngine;

namespace Assets.root.Runtime.Look.Interfaces
{
    public interface ICameraSway
    {
        void Sway(Vector3 inputVector, float rawXInput);
        void ResetSway();
    }
}