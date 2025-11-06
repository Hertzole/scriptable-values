#nullable enable

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hertzole.ScriptableValues.Helpers
{
    /// <summary>
    ///     Helper methods for ScriptableValues.
    /// </summary>
    public static class EqualityHelper
    {
        /// <summary>
        ///     Helper method for checking if two types are the same.
        /// </summary>
        /// <remarks>This takes Unity objects into consideration.</remarks>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals<T>(T x, T y)
        {
            // Do special checking on Unity objects.
            if (typeof(T).IsSubclassOf(typeof(Object)) || typeof(T) == typeof(Object))
            {
                Object? xUnityObject = x as Object;
                Object? yUnityObject = y as Object;

                return xUnityObject == yUnityObject;
            }

            return EqualityComparer<T>.Default.Equals(x, y);
        }

        /// <summary>
        ///     Helper method for checking if an object is null.
        /// </summary>
        /// <remarks>This takes Unity objects into consideration.</remarks>
        /// <param name="obj">The object to check.</param>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns><c>true</c> if the object is null; otherwise <c>false</c>.</returns>
        public static bool IsNull<T>(T obj)
        {
            // Do special checking on Unity objects.
            if (typeof(T).IsSubclassOf(typeof(Object)) || typeof(T) == typeof(Object))
            {
                Object? unityObject = obj as Object;
                return unityObject == null;
            }

            return obj == null;
        }

        /// <summary>
        ///     Helper method to check if the given object is the same type as the provided generic type.
        /// </summary>
        /// <param name="value">The object value to check.</param>
        /// <param name="newValue">The value as the generic value.</param>
        /// <typeparam name="TType">The type to match.</typeparam>
        /// <returns><c>true</c> if the value is the same type; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSameType<TType>(object? value, [NotNullWhen(true)] out TType? newValue)
        {
            if (value is TType newValueT)
            {
                newValue = newValueT;
                return true;
            }

            ThrowHelper.ThrowIfNull(value, nameof(value));

#if DEBUG
            Debug.LogError($"{typeof(TType)} is not assignable from {value.GetType()}.");
#endif
            newValue = default;
            return false;
        }
    }
}