#nullable enable

using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Hertzole.ScriptableValues.Helpers
{
    internal static partial class Guard
    {
        /// <summary>
        ///     Throws an <see cref="ArgumentNullException" /> if the given value is null.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNull<T>([System.Diagnostics.CodeAnalysis.NotNull][NoEnumeration] T? value, string name) where T : class
        {
            if (!EqualityHelper.IsNull(value))
            {
                return;
            }

            ThrowHelper.ThrowArgumentNullExceptionForIsNotNull<T>(name);
        }

        /// <summary>
        ///     Throws a <see cref="ReadOnlyException" /> if the given value is read-only.
        /// </summary>
        /// <exception cref="ReadOnlyException" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotReadOnly(bool isReadOnly, string name)
        {
#if UNITY_EDITOR
            // Read-only does not matter when not playing.
            if (!Application.isPlaying)
            {
                return;
            }
#endif

            if (!isReadOnly)
            {
                return;
            }

            ThrowHelper.ThrowIsReadOnlyException(name);
        }

        /// <inheritdoc cref="IsNotReadOnly(bool, string)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotReadOnly<T>(T value, string name) where T : ICanBeReadOnly
        {
            IsNotReadOnly(value.IsReadOnly, name);
        }

        /// <inheritdoc cref="IsNotReadOnly" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotReadOnly<T>(ICollection<T> list, string name)
        {
            IsNotReadOnly(list.IsReadOnly, name);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentNullException" /> if the given value is null and T is a non-nullable type.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullAndNullsAreIllegal<T>(object? value, string paramName)
        {
            if (default(T) == null || value != null)
            {
                return;
            }

            ThrowHelper.ThrowArgumentNullExceptionForIsNotNull<T>(paramName);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException" /> if the actual value is less than the compare value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsGreaterThanOrEqualTo<T>(T actualValue, T compareValue, string paramName) where T : IComparable<T>
        {
            if (actualValue.CompareTo(compareValue) >= 0)
            {
                return;
            }

            ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsGreaterThanOrEqualTo(actualValue, compareValue, paramName);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException" /> if the actual value is not less than the compare value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsLessThan<T>(T actualValue, T compareValue, string paramName) where T : IComparable<T>
        {
            if (actualValue.CompareTo(compareValue) < 0)
            {
                return;
            }

            ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsLessThan(compareValue, actualValue, paramName);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException" /> if the value is not in the specified range.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsInRange<T>(T value, T minValue, T maxValue, string paramName) where T : IComparable<T>
        {
            if (value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0)
            {
                return;
            }

            ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsInRange(value, minValue, maxValue, paramName);
        }
    }
}