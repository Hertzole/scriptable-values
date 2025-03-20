using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace Hertzole.ScriptableValues
{
	// Basically coped from https://github.com/CommunityToolkit/dotnet/blob/657c6971a8d42655c648336b781639ed96c2c49f/src/CommunityToolkit.HighPerformance/Buffers/SpanOwner%7BT%7D.cs
	internal readonly ref struct SpanOwner<T>
	{
		private readonly ArrayPool<T> pool;
		private readonly T[] array;

		public Span<T> Span
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return new Span<T>(array, 0, Length); }
		}

		public int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		}

		public static SpanOwner<T> Empty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return new SpanOwner<T>(0, ArrayPool<T>.Shared); }
		}

		private SpanOwner(int length, ArrayPool<T> pool)
		{
			Length = length;
			this.pool = pool;
			array = pool.Rent(length);

			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				array.AsSpan(0, length).Clear();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyFrom(ICollection<T> collection)
		{
			Assert.IsTrue(collection.Count <= Length, "The collection is larger than the allocated array.");

			collection.CopyTo(array, 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SpanOwner<T> Allocate(int size)
		{
			return new SpanOwner<T>(size, ArrayPool<T>.Shared);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			pool.Return(array);
		}
	}
}