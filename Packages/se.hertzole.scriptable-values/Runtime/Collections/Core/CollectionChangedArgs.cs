#nullable enable

using System;
using System.Buffers;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Represents the method that will handle an event when a collection changes.
	/// </summary>
	/// <typeparam name="T">The type of the items in the collection.</typeparam>
	public delegate void CollectionChangedEventHandler<T>(CollectionChangedArgs<T> e);

	/// <summary>
	///     Represents the method that will handle an event when a collection changes with additional context.
	/// </summary>
	/// <typeparam name="T">The type of the items in the collection.</typeparam>
	/// <typeparam name="TContext">The type of the context.</typeparam>
	public delegate void CollectionChangedWithContextEventHandler<T, in TContext>(CollectionChangedArgs<T> e, TContext context);

	public static class CollectionChangedArgsExtensions
	{
		/// <summary>
		///     Converts the <see cref="CollectionChangedArgs{T}" /> to a <see cref="NotifyCollectionChangedEventArgs" />.
		/// </summary>
		/// <param name="args">The current args.</param>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <returns>The converted <see cref="NotifyCollectionChangedEventArgs" />.</returns>
		/// <exception cref="NotSupportedException"><c>args.Action</c> is not supported.</exception>
		public static NotifyCollectionChangedEventArgs ToNotifyCollectionChangedEventArgs<T>(this CollectionChangedArgs<T> args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Add:
					return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, args.NewItems.ToArray(), args.NewIndex);
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
	}

	/// <summary>
	///     Provides data for the <see cref="CollectionChangedEventHandler{T}" /> and
	///     <see cref="CollectionChangedWithContextEventHandler{T, TContext}" /> events.
	/// </summary>
	/// <typeparam name="T">The type of the items in the collection.</typeparam>
	[StructLayout(LayoutKind.Auto)]
	public readonly struct CollectionChangedArgs<T> : IEquatable<CollectionChangedArgs<T>>
	{
		/// <summary>
		///     Gets the action that specifies how the collection changed.
		/// </summary>
		public readonly NotifyCollectionChangedAction Action;
		/// <summary>
		///     Gets the index at which the new item was added.
		/// </summary>
		public readonly int NewIndex;
		/// <summary>
		///     Gets the index at which the old item was removed.
		/// </summary>
		public readonly int OldIndex;
		/// <summary>
		///     Gets the items that were added.
		/// </summary>
		public readonly ReadOnlyMemory<T> OldItems;
		/// <summary>
		///     Gets the items that were removed.
		/// </summary>
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

		/// <summary>
		///     Creates a new instance of <see cref="CollectionChangedArgs{T}" /> with the <c>Add</c> action with only one item.
		/// </summary>
		/// <param name="newItem">The new item that was added.</param>
		/// <param name="startingIndex">The index at which the new item was added.</param>
		/// <returns>A new instance of <see cref="CollectionChangedArgs{T}" />.</returns>
		public static CollectionChangedArgs<T> Add(T newItem, int startingIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(1))
			{
				Memory<T> memory = owner.Memory.Slice(0, 1);
				memory.Span[0] = newItem;
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Add, startingIndex, memory);
			}
		}

		/// <summary>
		///     Creates a new instance of <see cref="CollectionChangedArgs{T}" /> with the <c>Add</c> action with multiple items.
		/// </summary>
		/// <param name="newItems">The new items that were added.</param>
		/// <param name="startingIndex">The index at which the new items were added.</param>
		/// <returns>A new instance of <see cref="CollectionChangedArgs{T}" />.</returns>
		public static CollectionChangedArgs<T> Add(ReadOnlySpan<T> newItems, int startingIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(newItems.Length))
			{
				Memory<T> memory = owner.Memory.Slice(0, newItems.Length);
				newItems.CopyTo(memory.Span);
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Add, startingIndex, memory);
			}
		}

		/// <summary>
		///     Creates a new instance of <see cref="CollectionChangedArgs{T}" /> with the <c>Remove</c> action with only one item.
		/// </summary>
		/// <param name="oldItem">The old item that was removed.</param>
		/// <param name="removedIndex">The index at which the old item was removed.</param>
		/// <returns>A new instance of <see cref="CollectionChangedArgs{T}" />.</returns>
		public static CollectionChangedArgs<T> Remove(T oldItem, int removedIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(1))
			{
				Memory<T> memory = owner.Memory.Slice(0, 1);
				memory.Span[0] = oldItem;
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Remove, oldIndex: removedIndex, oldItems: memory);
			}
		}

		/// <summary>
		///     Creates a new instance of <see cref="CollectionChangedArgs{T}" /> with the <c>Remove</c> action with multiple
		///     items.
		/// </summary>
		/// <param name="oldItems">The old items that were removed.</param>
		/// <param name="removedIndex">The index at which the old items were removed.</param>
		/// <returns>A new instance of <see cref="CollectionChangedArgs{T}" />.</returns>
		public static CollectionChangedArgs<T> Remove(ReadOnlySpan<T> oldItems, int removedIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(oldItems.Length))
			{
				Memory<T> memory = owner.Memory.Slice(0, oldItems.Length);
				oldItems.CopyTo(memory.Span);
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Remove, oldIndex: removedIndex, oldItems: memory);
			}
		}

		/// <summary>
		///     Creates a new instance of <see cref="CollectionChangedArgs{T}" /> with the <c>Replace</c> action with only one
		///     item.
		/// </summary>
		/// <param name="oldItem">The old item that was replaced.</param>
		/// <param name="newItem">The new item that was added.</param>
		/// <param name="index">The index at which the item was replaced.</param>
		/// <returns>A new instance of <see cref="CollectionChangedArgs{T}" />.</returns>
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

		/// <summary>
		///     Creates a new instance of <see cref="CollectionChangedArgs{T}" /> with the <c>Replace</c> action with multiple
		///     items.
		/// </summary>
		/// <param name="oldItems">The old items that were replaced.</param>
		/// <param name="newItems">The new items that were added.</param>
		/// <param name="startingIndex">The index at which the items were replaced.</param>
		/// <returns>A new instance of <see cref="CollectionChangedArgs{T}" />.</returns>
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

		/// <summary>
		///     Creates a new instance of <see cref="CollectionChangedArgs{T}" /> with the <c>Reset</c> action.
		/// </summary>
		/// <param name="items">The items that were cleared.</param>
		/// <param name="newIndex">The index at which the items were cleared.</param>
		/// <param name="oldIndex">The index at which the items were cleared.</param>
		/// <returns>A new instance of <see cref="CollectionChangedArgs{T}" />.</returns>
		public static CollectionChangedArgs<T> Clear(ReadOnlySpan<T> items, int newIndex = 0, int oldIndex = 0)
		{
			using IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(items.Length);
			Memory<T> memory = owner.Memory.Slice(0, items.Length);
			items.CopyTo(memory.Span);

			return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Reset, newIndex, oldIndex: oldIndex, oldItems: memory);
		}

		public bool Equals(CollectionChangedArgs<T> other)
		{
			return Action == other.Action && NewIndex == other.NewIndex && OldIndex == other.OldIndex && OldItems.Equals(other.OldItems) &&
			       NewItems.Equals(other.NewItems);
		}

		public override bool Equals(object? obj)
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