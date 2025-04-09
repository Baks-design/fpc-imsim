using System;
using UnityEngine;

namespace Assets.root.Runtime.Look.Settings
{
    [Serializable]
    public class CameraRotationSettings
    {
        [Range(0f, 90f)] public float topClamp = 60f;
        [Range(0f, -90f)] public float bottomClamp = -60f;
        [Range(0f, 25f)] public float verticalSpeedRotation = 20f;
        [Range(0f, 25f)] public float horizontalSpeedRotation = 20f;
    }
}