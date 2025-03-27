using System;
using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Hertzole.ScriptableValues.Generator;

public readonly struct ArrayBuilder<T> : IDisposable
{
	private readonly Writer writer;

	public ArrayBuilder()
	{
		writer = new Writer();
	}

	public ArrayBuilder(int capacity)
	{
		writer = new Writer(capacity);
	}

	public ArrayBuilder(ReadOnlySpan<T> items)
	{
		writer = new Writer(items.Length);
		writer.AddRange(items);
	}

	public void Add(T item)
	{
		writer.Add(item);
	}

	public void AddRange(ReadOnlySpan<T> items)
	{
		writer.AddRange(items);
	}

	public void Clear()
	{
		writer.Clear();
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

	public ReadOnlySpan<T> AsSpan()
	{
		return writer.AsSpan();
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return writer.AsSpan().ToString();
	}

	private sealed class Writer : IDisposable
	{
		private T[] array;
		private int index = 0;

		public Writer(int capacity = 0)
		{
			array = capacity == 0 ? Array.Empty<T>() : ArrayPool<T>.Shared.Rent(capacity);
		}

		public void Add(T value)
		{
			EnsureCapacity(1);

			array[index++] = value;
		}

		public void AddRange(ReadOnlySpan<T> items)
		{
			EnsureCapacity(items.Length);

			items.CopyTo(array.AsSpan(index));
			index += items.Length;
		}

		public void Clear()
		{
			Array.Clear(array, 0, index);
			index = 0;
		}

		private void EnsureCapacity(int requestedSize)
		{
			if (requestedSize > array.Length - index)
			{
				ResizeBuffer(requestedSize);
			}
		}

		private void ResizeBuffer(int capacity)
		{
			int minimumSize = index + capacity;

			T[] oldBuffer = array;
			T[]? newBuffer = ArrayPool<T>.Shared.Rent(minimumSize);

			Array.Copy(oldBuffer, newBuffer, oldBuffer.Length);

			array = newBuffer;

			if (oldBuffer.Length > 0)
			{
				ArrayPool<T>.Shared.Return(oldBuffer, typeof(T) != typeof(char));
			}
		}

		public void Dispose()
		{
			if (array.Length > 0)
			{
				ArrayPool<T>.Shared.Return(array, typeof(T) != typeof(char));
			}
		}

		public ReadOnlySpan<T> AsSpan()
		{
			return new ReadOnlySpan<T>(array, 0, index);
		}
	}
}