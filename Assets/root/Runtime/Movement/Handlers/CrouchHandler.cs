using System;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    public class CrouchHandler : ICrouchHandler
    {
        readonly MovementCrouchSettings settings;
        readonly CharacterController character;
        readonly Transform cameraYaw;
        readonly Transform aimPoint;
        bool m_IsCrouching;
        bool isCrouching;
        float m_TargetCharacterHeight;

        public bool IsCrouching => m_IsCrouching;

        public event Action<bool> OnStanceChanged = delegate { };

        public CrouchHandler(
            MovementCrouchSettings settings,
            CharacterController character,
            Transform cameraYaw,
            Transform aimPoint)
        {
            this.settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            this.character = character != null ? character : throw new ArgumentNullException(nameof(character));
            this.cameraYaw = cameraYaw != null ? cameraYaw : throw new ArgumentNullException(nameof(cameraYaw));
            this.aimPoint = aimPoint != null ? aimPoint : throw new ArgumentNullException(nameof(aimPoint));
            isCrouching = false;
        }

        public void ToggleCrouch()
        {
            isCrouching = !isCrouching;
            SetCrouch(isCrouching);
        }

        public void SetCrouch(bool isCrouching)
        {
            m_IsCrouching = isCrouching;
            m_TargetCharacterHeight = m_IsCrouching ? settings.CapsuleHeightCrouching : settings.CapsuleHeightStanding;
            OnStanceChanged.Invoke(m_IsCrouching);
        }

        public void UpdateCharacterHeight(bool force)
        {
            // Update height instantly
            if (force)
            {
                character.height = m_TargetCharacterHeight;
                character.center = 0.5f * character.height * Vector3.up;
                cameraYaw.transform.localPosition = m_TargetCharacterHeight * settings.CameraHeightRatio * Vector3.up;
                aimPoint.transform.localPosition = character.center;
            }
            // Update smooth height
            else if (character.height != m_TargetCharacterHeight)
            {
                // Smoothly interpolate height
                character.height = Mathf.Lerp(
                    character.height,
                    m_TargetCharacterHeight,
                    settings.CrouchingSharpness * Time.deltaTime
                );

                // Adjust center position
                character.center = 0.5f * character.height * Vector3.up;

                // Adjust camera position
                cameraYaw.localPosition = Vector3.Lerp(
                    cameraYaw.localPosition,
                    settings.CameraHeightRatio * m_TargetCharacterHeight * Vector3.up,
                    settings.CrouchingSharpness * Time.deltaTime
                );

                // Adjust aim point
                aimPoint.localPosition = character.center;
            }
        }
    }
}