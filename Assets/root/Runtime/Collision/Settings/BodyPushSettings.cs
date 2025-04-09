using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
    [Serializable]
    public class BodyPushSettings
    {
        public LayerMask pushLayers = ~0;
        public bool canPush = true;
        [Range(0.5f, 5f)] public float strength = 1f;
    }
}