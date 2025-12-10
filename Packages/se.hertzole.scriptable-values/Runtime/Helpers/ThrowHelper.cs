#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.ScriptableValues.Helpers
{
    internal static class ThrowHelper
    {
        [DoesNotReturn]
        public static void ThrowWrongExpectedValueType<T>(object? value)
        {
            throw new ArgumentException($"Expected {typeof(T)}, but was {value?.GetType()}");
        }
    }
}