using System;
using UnityEngine;

namespace Assets.root.Runtime.Cam.Settings
{
    [CreateAssetMenu(menuName = "Settings/Camera/Rotation")]
    public class CameraRotationSettings : ScriptableObject
    {
        [Range(0f, 90f)] public float topClampY = 60f;
        [Range(0f, -90f)] public float bottomClampY = -60f;
        [Range(0f, 50f)] public float speedRotationX = 20f;
        [Range(0f, 50f)] public float speedRotationY = 20f;
        [Range(0f, 50f)] public float smoothRotationX = 20f;
        [Range(0f, 50f)] public float smoothRotationY = 20f;
    }
}