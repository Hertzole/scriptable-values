#nullable enable

using System;
using System.Collections.Generic;
using Hertzole.ScriptableValues.Helpers;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Assertions;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Base class for handling struct closures with delegates.
	/// </summary>
	/// <typeparam name="TDelegate">The type of delegate to handle.</typeparam>
	/// <typeparam name="TAction">The modified action with the context to use in the closure.</typeparam>
	internal abstract class BaseDelegateHandlerList<TDelegate, TAction> : IDelegateList<TDelegate>
		where TDelegate : Delegate where TAction : Delegate
	{
		private List<StructClosure<TAction>>? callbacks = null;

		public int ListenersCount
		{
			get { return callbacks?.Count ?? 0; }
		}

		public void RegisterCallback(TDelegate callback)
		{
			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			InitializeListIfNeeded();
			callbacks!.Add(new StructClosure<TAction>(action, null));
		}

		public void RegisterCallback<TContextDelegate, TContext>(TContextDelegate callback, TContext context) where TContextDelegate : Delegate
		{
			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TContextDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			InitializeListIfNeeded();
			callbacks!.Add(new StructClosure<TAction>(action, context));
		}

		public void RemoveCallback(TDelegate callback)
		{
			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			InitializeListIfNeeded();
			callbacks!.Remove(new StructClosure<TAction>(action, null));
		}

		public void RemoveCallback<TContextDelegate>(TContextDelegate callback) where TContextDelegate : Delegate
		{
			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TContextDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			InitializeListIfNeeded();
			callbacks!.Remove(new StructClosure<TAction>(action, null));
		}

		public void Reset()
		{
			callbacks?.Clear();
		}

		public void AddFrom(BaseDelegateHandlerList<TDelegate, TAction> other)
		{
			if (other.ListenersCount == 0)
			{
				return;
			}

			Assert.IsNotNull(other.callbacks);

			if (callbacks == null)
			{
				callbacks = new List<StructClosure<TAction>>(other.callbacks!);
			}
			else
			{
				callbacks.AddRange(other.callbacks!);
			}
		}

		public void RemoveFrom(BaseDelegateHandlerList<TDelegate, TAction> other)
		{
			if (other.ListenersCount == 0 || ListenersCount == 0)
			{
				return;
			}

			Assert.IsNotNull(other.callbacks);

			for (int i = 0; i < other.callbacks!.Count; i++)
			{
				callbacks!.Remove(other.callbacks[i]);
			}
		}

		public SpanOwner<Delegate> GetDelegates()
		{
			return callbacks == null ? SpanOwner<Delegate>.Empty : EventHelper.GetListeners(callbacks);
		}

		protected SpanOwner<StructClosure<TAction>> GetCallbacks()
		{
			if (callbacks == null)
			{
				return SpanOwner<StructClosure<TAction>>.Empty;
			}

			SpanOwner<StructClosure<TAction>> span = SpanOwner<StructClosure<TAction>>.Allocate(callbacks.Count);
			span.CopyFrom(callbacks);

			return span;
		}

		private void InitializeListIfNeeded()
		{
			if (callbacks != null)
			{
				return;
			}

			// Start with capacity of 2 because we will be adding at least one item, and usually it doesn't go above 2.
			callbacks = new List<StructClosure<TAction>>(2);
		}
	}
}