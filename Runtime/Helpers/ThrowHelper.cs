#nullable enable

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Hertzole.ScriptableValues.Helpers
{
    internal static class ThrowHelper
    {
        [Conditional("DEBUG")]
        public static void ThrowIfNull([NotNull] object? obj, string name)
        {
#if DEBUG
            if (obj == null)
            {
                ThrowNullArgumentException(name);
            }
#endif
        }

        public static void ThrowIfDisposed(in bool isDisposed)
        {
            if (isDisposed)
            {
                ThrowDisposedException();
            }
        }

        /// <summary>
        ///     Throws a <see cref="ReadOnlyException" /> if <paramref name="isReadOnly" /> is true and the application is
        ///     currently playing.
        /// </summary>
        /// <param name="isReadOnly">Weather or not the object is read-only.</param>
        /// <param name="context">The object that is read-only.</param>
        /// <exception cref="ReadOnlyException">If the object is read-only and the application is playing.</exception>
        public static void ThrowIfIsReadOnly(in bool isReadOnly, object context)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif // UNITY_EDITOR

            if (isReadOnly)
            {
                ThrowReadOnlyException(context);
            }
        }

        public static void ThrowIfNullAndNullsAreIllegal<T>(object? value, string paramName)
        {
            if (default(T) != null && value == null)
            {
                ThrowNullArgumentException(paramName);
            }
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException" /> if the value is outside the specified range.
        /// </summary>
        /// <remarks>This only throws in development builds and never release builds.</remarks>
        /// <param name="paramName">The name of the parameter that was wrong.</param>
        /// <param name="value">The actual value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <c>value</c> is below <c>min</c> or above <c>max</c>.</exception>
        [Conditional("DEBUG")]
        public static void ThrowIfOutOfBounds(string paramName, in int value, in int min, in int max)
        {
            if (value < min || value > max)
            {
                ThrowArgumentOutOfRangeException(paramName, $"{paramName} must be between {min} and {max} (Was {value}).");
            }
        }

        /// <summary>
        ///     Check if the specified collection contains the specified value.
        /// </summary>
        /// <param name="collection">The collection to check.</param>
        /// <param name="value">The value to check for.</param>
        /// <typeparam name="T">The type of the value in the collection.</typeparam>
        /// <exception cref="ArgumentException"><paramref name="collection" /> contains <paramref name="value" />.</exception>
        public static void ThrowIfContains<T>(IList<T> collection, in T value)
        {
            if (!collection.Contains(value))
            {
                return;
            }

            ThrowDuplicateItemException(value);
        }

        public static void ThrowIfDestinationListIsReadOnly<T>(IList<T> list, string name)
        {
            if (!list.IsReadOnly)
            {
                return;
            }

            ThrowReadOnlyDestinationList(name);
        }

        [DoesNotReturn]
        public static void ThrowWrongExpectedValueType<T>(object? value)
        {
            throw new ArgumentException($"Expected {typeof(T)}, but was {value?.GetType()}");
        }

        [DoesNotReturn]
        private static void ThrowNullArgumentException(string name)
        {
            throw new ArgumentNullException(name, $"{name} is null.");
        }

        [DoesNotReturn]
        private static void ThrowDisposedException()
        {
            throw new ObjectDisposedException("The object has been disposed.");
        }

        [DoesNotReturn]
        private static void ThrowReadOnlyException(object context)
        {
            throw new ReadOnlyException($"{context} is marked as read only and cannot be modified at runtime.");
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException" /> with the specified message.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DoesNotReturn]
        private static void ThrowArgumentOutOfRangeException(string paramName, string message)
        {
            throw new ArgumentOutOfRangeException(paramName, message);
        }

        /// <exception cref="ArgumentException" />
        [DoesNotReturn]
        private static void ThrowDuplicateItemException<T>(T item)
        {
            throw new ArgumentException($"Item {item} already exists in the collection.");
        }

        /// <exception cref="ArgumentException"></exception>
        [DoesNotReturn]
        private static void ThrowReadOnlyDestinationList(string name)
        {
            throw new ArgumentException("The destination list is read-only and cannot be modified.", name);
        }
    }
}