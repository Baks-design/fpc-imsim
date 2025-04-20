using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.root.Runtime.Utilities.Helpers
{
    public static class Mathfs
    {
        const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

        #region Math operations
        /// <summary>Returns the square root of the given value</summary>
        [MethodImpl(INLINE)] public static float Sqrt(float value) => MathF.Sqrt(value);
        #endregion

        #region Clamping
        /// <summary>Returns the value clamped between <c>min</c> and <c>max</c></summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public static float Clamp(float value, float min, float max) => value < min ? min : value > max ? max : value;

        /// <summary>Returns the value clamped between 0 and 1</summary>
        public static float Clamp01(float value) => value < 0f ? 0f : value > 1f ? 1f : value;
        #endregion

        #region Min & Max
        /// <summary>Returns the largest of the two values</summary>
        [MethodImpl(INLINE)] public static float Max(float a, float b) => a > b ? a : b;
        #endregion

        #region Value & Vector interpolation
        /// <summary>Blends between a and b, based on the t-value. When t = 0 it returns a, when t = 1 it returns b,
        ///  and any values between are blended linearly </summary>
        /// <param name="a">The start value, when t is 0</param>
        /// <param name="b">The start value, when t is 1</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the lerp</param>
        [MethodImpl(INLINE)] public static float Lerp(float a, float b, float t) => (1f - t) * a + t * b;

        /// <summary>Given a value between a and b, returns its normalized location in that range, as a t-value (interpolant) from 0 to 1</summary>
        /// <param name="a">The start of the range, where it would return 0</param>
        /// <param name="b">The end of the range, where it would return 1</param>
        /// <param name="value">A value between a and b. Note: values outside this range are still valid, and will be extrapolated</param>
        [MethodImpl(INLINE)] public static float InverseLerp(float a, float b, float value) => (value - a) / (b - a);

        /// <summary>Exponential interpolation, the multiplicative version of lerp, useful for values such as scaling or zooming</summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the eerp</param>
        [MethodImpl(INLINE)]
        public static float Eerp(float a, float b, float t)
        => t switch
        {
            <= 0f => a,
            >= 1f => b,
            _ => MathF.Pow(a, 1f - t) * MathF.Pow(b, t)
        };

        /// <summary>Inverse exponential interpolation, the multiplicative version of InverseLerp, 
        /// useful for values such as scaling or zooming</summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="v">A value between a and b. Note: values outside this range are still valid, and will be extrapolated</param>
        [MethodImpl(INLINE)]
        public static float InverseEerp(float a, float b, float v) => MathF.Log(a / v) / MathF.Log(a / b);

        /// <summary>
        /// Exponential Position Decaying Interpolation</summary>
        /// </summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the eerp</param>
        /// <param name="h">Constant</param>
        /// <returns></returns>
        [MethodImpl(INLINE)]
        public static float ExpPosDecay(float a, float b, float t, float h = 16f) => b + (a - b) * MathF.Exp(-h * t);

        /// <summary>
        /// Exponential Position Decaying Interpolation</summary>
        /// </summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the eerp</param>
        /// <param name="h">Constant</param>
        /// <returns></returns>
        [MethodImpl(INLINE)]
        public static Vector2 ExpPosDecay(Vector2 a, Vector2 b, float t, float h = 16f) => b + (a - b) * MathF.Exp(-h * t);

        /// <summary>
        /// Exponential Position Decaying Interpolation</summary>
        /// </summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the eerp</param>
        /// <param name="h">Constant</param>
        /// <returns></returns>
        [MethodImpl(INLINE)]
        public static Vector3 ExpPosDecay(Vector3 a, Vector3 b, float t, float h = 16f) => b + (a - b) * MathF.Exp(-h * t);

        /// <summary>
        /// Exponential Position Decaying Interpolation</summary>
        /// </summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the eerp</param>
        /// <param name="h">Constant</param>
        /// <returns></returns>
        public static float InverseExpPosDecay(float a, float b, float f, float h = 16f) => -MathF.Log((f - b) / (a - b)) / h;

        /// <summary>
        /// Exponential Rotation Decaying Interpolation
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="deltaTime"></param>
        /// <param name="smoothingFactor"></param>
        /// <returns></returns>
        [MethodImpl(INLINE)]
        public static Quaternion ExpRotDecay(Quaternion a, Quaternion b, float t, float h = 16f)
        => Quaternion.Slerp(a, b, 1f - MathF.Exp(-h * t));
        #endregion
    }
}