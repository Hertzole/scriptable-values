using System;
using System.Buffers;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
	public delegate void CollectionChangedEventHandler<T>(CollectionChangedArgs<T> e);
	
	public delegate  void CollectionChangedWithContextEventHandler<T, TContext>(CollectionChangedArgs<T> e, TContext context);
	
	[StructLayout(LayoutKind.Auto)]
	public readonly struct CollectionChangedArgs<T>
	{
		public readonly NotifyCollectionChangedAction Action;
		public readonly int NewIndex;
		public readonly int OldIndex;
		public readonly ReadOnlyMemory<T> OldItems;
		public readonly ReadOnlyMemory<T> NewItems;

		private CollectionChangedArgs(NotifyCollectionChangedAction action, int newIndex, int oldIndex, ReadOnlyMemory<T> oldItems, ReadOnlyMemory<T> newItems)
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
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Add, startingIndex, -1, ReadOnlyMemory<T>.Empty, memory);
			}
		}

		public static CollectionChangedArgs<T> Add(ReadOnlySpan<T> newItems, int startingIndex)
		{
			using (IMemoryOwner<T> owner = MemoryPool<T>.Shared.Rent(newItems.Length))
			{
				Memory<T> memory = owner.Memory.Slice(0, newItems.Length);
				newItems.CopyTo(memory.Span);
				return new CollectionChangedArgs<T>(NotifyCollectionChangedAction.Add, startingIndex, -1, ReadOnlyMemory<T>.Empty, memory);
			}
		}
		
		public static implicit  operator NotifyCollectionChangedEventArgs(CollectionChangedArgs<T> args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Add:
					return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, args.NewItems.ToArray(), args.NewIndex);
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Remove:
					break;
				case NotifyCollectionChangedAction.Replace:
					break;
				case NotifyCollectionChangedAction.Reset:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			throw new NotImplementedException();
		}
	}
}