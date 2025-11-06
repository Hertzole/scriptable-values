using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Hertzole.ScriptableValues.Helpers
{
    internal static class ArrayHelpers
    {
        public static void EnsureCapacity<T>(ref T[] array, int capacity)
        {
            if (array.Length <= capacity)
            {
                int length = array.Length;
                T[] newArray = ArrayPool<T>.Shared.Rent(capacity * 2);
                Array.Copy(array, newArray, length);
                ArrayPool<T>.Shared.Return(array, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
                array = newArray;
            }
        }

        public static bool SequenceEquals<T>(in ReadOnlySpan<T> left, in ReadOnlySpan<T> right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < left.Length; i++)
            {
                if (!comparer.Equals(left[i], right[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}