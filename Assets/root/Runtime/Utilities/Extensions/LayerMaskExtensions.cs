using UnityEngine;

namespace Assets.root.Runtime.Utilities.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer) => (mask.value & (1 << layer)) != 0;
    }
}