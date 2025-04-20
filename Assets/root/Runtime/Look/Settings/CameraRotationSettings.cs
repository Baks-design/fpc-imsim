using System;
using UnityEngine;

namespace Assets.root.Runtime.Look.Settings
{
    [CreateAssetMenu(menuName = "Settings/Look/Rotation")]
    public class CameraRotationSettings : ScriptableObject
    {
        [Header("Limits")]
        [Range(0f, 90f)] public float topClampY = 60f;
        [Range(0f, -90f)] public float bottomClampY = -60f;
        [Header("Speed")]
        [Range(0f, 50f)] public float speedRotationX = 20f;
        [Range(0f, 50f)] public float speedRotationY = 20f;
        [Min(0.01f)] public float decaySpeed = 20f;
    }
}