#nullable enable

using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.ScriptableValues.Helpers
{
    internal partial class Guard
    {
        private static class ThrowHelper
        {
            /// <exception cref="ArgumentNullException" />
            [DoesNotReturn]
            public static void ThrowArgumentNullExceptionForIsNotNull<T>(string name)
            {
                throw new ArgumentNullException(name, $"{AssertString(name)} ({typeof(T).Name}) must be not null.");
            }

            /// <exception cref="ReadOnlyException" />
            [DoesNotReturn]
            public static void ThrowIsReadOnlyException(string name)
            {
                throw new ReadOnlyException($"{AssertString(name)} is read-only and cannot be modified.");
            }

            /// <exception cref="ArgumentOutOfRangeException" />
            [DoesNotReturn]
            public static void ThrowArgumentOutOfRangeExceptionForIsGreaterThanOrEqualTo<T>(T actualValue, T compareValue, string paramName)
                where T : IComparable<T>
            {
                throw new ArgumentOutOfRangeException(paramName, actualValue,
                    $"{AssertString(paramName)} must be greater than or equal to {AssertString(compareValue)} (Was {AssertString(actualValue)}).");
            }

            /// <exception cref="ArgumentOutOfRangeException" />
            [DoesNotReturn]
            public static void ThrowArgumentOutOfRangeExceptionForIsLessThan<T>(T actualValue, T compareValue, string paramName)
                where T : IComparable<T>
            {
                throw new ArgumentOutOfRangeException(paramName, actualValue,
                    $"{AssertString(paramName)} must be less than {AssertString(compareValue)} (Was {AssertString(actualValue)}).");
            }

            /// <exception cref="ArgumentOutOfRangeException" />
            [DoesNotReturn]
            public static void ThrowArgumentOutOfRangeExceptionForIsInRange<T>(T value, T minValue, T maxValue, string paramName) where T : IComparable<T>
            {
                throw new ArgumentOutOfRangeException(paramName, value,
                    $"{AssertString(paramName)} must be between {AssertString(minValue)} and {AssertString(maxValue)} (Was {AssertString(value)}).");
            }

            private static string AssertString(object? obj)
            {
                return obj switch
                {
                    string _ => $"\"{obj}\"",
                    null => "null",
                    _ => $"<{obj}>"
                };
            }
        }
    }
}