using System;
using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Hertzole.ScriptableValues.Generator;

public readonly struct ArrayBuilder<T>
{
	private readonly Writer writer;

	public ArrayBuilder()
	{
		writer = new Writer();
	}

	public void Add(T item)
	{
		writer.Add(item);
	}

	public void Dispose()
	{
		writer.Dispose();
	}

	public ImmutableArray<T> ToImmutable()
	{
		T[]? array = writer.AsSpan().ToArray();

		return Unsafe.As<T[], ImmutableArray<T>>(ref array);
	}

	private sealed class Writer : IDisposable
	{
		private T[] array = Array.Empty<T>();
		private int index = 0;

		public void Add(T value)
		{
			EnsureCapacity(index + 1);

			array[index++] = value;
		}

		private void EnsureCapacity(int capacity)
		{
			if (array.Length <= capacity)
			{
				ResizeBuffer(capacity);
			}
		}

		private void ResizeBuffer(int capacity)
		{
			T[] oldBuffer = array;
			T[]? newBuffer = ArrayPool<T>.Shared.Rent(capacity);

			Array.Copy(oldBuffer, newBuffer, oldBuffer.Length);

			array = newBuffer;

			if (oldBuffer.Length > 0)
			{
				ArrayPool<T>.Shared.Return(oldBuffer, true);
			}
		}

		public void Dispose()
		{
			if (array.Length > 0)
			{
				ArrayPool<T>.Shared.Return(array, true);
			}
		}

		public ReadOnlySpan<T> AsSpan()
		{
			return new ReadOnlySpan<T>(array, 0, index);
		}
	}
}