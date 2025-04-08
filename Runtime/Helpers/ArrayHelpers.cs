using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Hertzole.ScriptableValues.Helpers
{
	internal static class ArrayHelpers
	{
		public static void EnsureCapacity<T>(ref T[] array, int index)
		{
			if (array.Length == index)
			{
				T[] newArray = ArrayPool<T>.Shared.Rent(index * 2);
				Array.Copy(array, newArray, index);
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