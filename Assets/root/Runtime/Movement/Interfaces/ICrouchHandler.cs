using System;

namespace Assets.root.Runtime.Movement.Interfaces
{
    public interface ICrouchHandler
    {
        bool IsCrouching { get; }

        event Action<bool> OnStanceChanged;

        void ToggleCrouch();
        void SetCrouch(bool isCrouching);
        void UpdateCharacterHeight(bool force);
    }
}