using System;
using System.Buffers;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace Hertzole.ScriptableValues
{
	public delegate void CollectionChangedEventHandler<T>(CollectionChangedArgs<T> e);

	public delegate void CollectionChangedWithContextEventHandler<T, TContext>(CollectionChangedArgs<T> e, TContext context);

	[StructLayout(LayoutKind.Auto)]
	public readonly struct CollectionChangedArgs<T> : IEquatable<CollectionChangedArgs<T>>
	{
		public readonly NotifyCollectionChangedAction Action;
		public readonly int NewIndex;
		public readonly int OldIndex;
		public readonly ReadOnlyMemory<T> OldItems;
		public readonly ReadOnlyMemory<T> NewItems;

		private CollectionChangedArgs(NotifyCollectionChangedAction action,
			int newIndex = -1,
			ReadOnlyMemory<T> newItems = default,
			int oldIndex = -1,
			ReadOnlyMemory<T> oldItems = default)
		{
			Action = action;
			NewIndex = newIndex;
			OldIndex = oldIndex;
			OldItems = oldItems;
			NewItems = newItems;
		}

		public static CollectionChangedArgs<T> Add(T newItem, int startingIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(1))
			{
				Memory<T> memory = owner.Memory.Slice(0, 1);
				memory.Span[0] = newItem;
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Add, startingIndex, memory);
			}
		}

		public static CollectionChangedArgs<T> Add(ReadOnlySpan<T> newItems, int startingIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(newItems.Length))
			{
				Memory<T> memory = owner.Memory.Slice(0, newItems.Length);
				newItems.CopyTo(memory.Span);
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Add, startingIndex, memory);
			}
		}

		public static CollectionChangedArgs<T> Remove(T oldItem, int startingIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(1))
			{
				Memory<T> memory = owner.Memory.Slice(0, 1);
				memory.Span[0] = oldItem;
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Remove, oldIndex: startingIndex, oldItems: memory);
			}
		}

		public static CollectionChangedArgs<T> Remove(ReadOnlySpan<T> oldItems, int startingIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(oldItems.Length))
			{
				Memory<T> memory = owner.Memory.Slice(0, oldItems.Length);
				oldItems.CopyTo(memory.Span);
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Remove, oldIndex: startingIndex, oldItems: memory);
			}
		}

		public static CollectionChangedArgs<T> Replace(T oldItem, T newItem, int index)
		{
			using (IMemoryOwner<T> oldOwner = MemoryPool<T>.Shared.Rent(1))
			{
				Memory<T> oldMemory = oldOwner.Memory.Slice(0, 1);
				oldMemory.Span[0] = oldItem;

				using (IMemoryOwner<T> newOwner = MemoryPool<T>.Shared.Rent(1))
				{
					Memory<T> newMemory = newOwner.Memory.Slice(0, 1);
					newMemory.Span[0] = newItem;
					return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Replace, index, newMemory, index, oldMemory);
				}
			}
		}

		public static CollectionChangedArgs<T> Replace(ReadOnlySpan<T> oldItems, ReadOnlySpan<T> newItems, int startingIndex)
		{
			using IMemoryOwner<T> oldOwner = MemoryPool<T>.Shared.Rent(oldItems.Length);
			Memory<T> oldMemory = oldOwner.Memory.Slice(0, oldItems.Length);
			oldItems.CopyTo(oldMemory.Span);

			using IMemoryOwner<T> newOwner = MemoryPool<T>.Shared.Rent(newItems.Length);
			Memory<T> newMemory = newOwner.Memory.Slice(0, newItems.Length);
			newItems.CopyTo(newMemory.Span);

			return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Replace, startingIndex, newMemory, startingIndex, oldMemory);
		}

		public static CollectionChangedArgs<T> Reset()
		{
			return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Reset);
		}

		public static implicit operator NotifyCollectionChangedEventArgs(CollectionChangedArgs<T> args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Add:
					return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, args.NewItems.ToArray(), args.NewIndex);
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Remove:
					return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, args.OldItems.ToArray(), args.OldIndex);
				case NotifyCollectionChangedAction.Replace:
					return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, args.NewItems.ToArray(), args.OldItems.ToArray(),
						args.NewIndex);
				case NotifyCollectionChangedAction.Reset:
					return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			}

			throw new NotSupportedException($"Can't create NotifyCollectionChangedEventArgs from this action ({args.Action}).");
		}

		public bool Equals(CollectionChangedArgs<T> other)
		{
			return Action == other.Action && NewIndex == other.NewIndex && OldIndex == other.OldIndex && OldItems.Equals(other.OldItems) &&
			       NewItems.Equals(other.NewItems);
		}

		public override bool Equals(object obj)
		{
			return obj is CollectionChangedArgs<T> other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (int) Action;
				hashCode = (hashCode * 397) ^ NewIndex;
				hashCode = (hashCode * 397) ^ OldIndex;
				hashCode = (hashCode * 397) ^ OldItems.GetHashCode();
				hashCode = (hashCode * 397) ^ NewItems.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(CollectionChangedArgs<T> left, CollectionChangedArgs<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(CollectionChangedArgs<T> left, CollectionChangedArgs<T> right)
		{
			return !left.Equals(right);
		}
	}
}