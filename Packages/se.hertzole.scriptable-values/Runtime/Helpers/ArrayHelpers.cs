using System;
using System.Buffers;
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
	}
}