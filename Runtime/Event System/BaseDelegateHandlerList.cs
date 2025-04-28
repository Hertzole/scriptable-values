#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
	public abstract class BaseDelegateHandlerList<TDelegate, TAction> : IDelegateList<TDelegate>
		where TDelegate : Delegate where TAction : Delegate
	{
		internal List<StructClosure<TAction>>? callbacks = null;

		public int ListenersCount
		{
			get { return callbacks?.Count ?? 0; }
		}

		/// <summary>
		///     Adds a callback to the list of callbacks.
		/// </summary>
		/// <param name="callback">The callback to add.</param>
		/// <exception cref="ArgumentException">A callback with the same method already exists in the callbacks list.</exception>
		public void AddCallback(TDelegate callback)
		{
			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			StructClosure<TAction> closure = new StructClosure<TAction>(action, null);

			// If the list is not initialized (it already exists), check for duplicates.
			if (!InitializeListIfNeeded())
			{
				ThrowHelper.ThrowIfContains(callbacks, in closure);
			}

			callbacks.Add(closure);
		}

		/// <summary>
		///     Adds a callback with the specified context to the list of callbacks.
		/// </summary>
		/// <param name="callback">The callback to add.</param>
		/// <param name="context">The context to use in the closure.</param>
		/// <typeparam name="TContextDelegate">The type of the delegate to add.</typeparam>
		/// <typeparam name="TContext">The type of the context to use in the closure.</typeparam>
		/// <exception cref="ArgumentException">A callback with the same method already exists in the callbacks list.</exception>
		public void AddCallback<TContextDelegate, TContext>(TContextDelegate callback, TContext context) where TContextDelegate : Delegate
		{
			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TContextDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			StructClosure<TAction> closure = new StructClosure<TAction>(action, context);

			// If the list is not initialized (it already exists), check for duplicates.
			if (!InitializeListIfNeeded())
			{
				ThrowHelper.ThrowIfContains(callbacks, in closure);
			}

			callbacks.Add(closure);
		}

		/// <summary>
		///     Removes the specified callback from the list of callbacks.
		/// </summary>
		/// <param name="callback">The callback to remove.</param>
		/// <returns><c>true</c> if <paramref name="callback" /> was successfully removed; otherwise, <c>false</c>.</returns>
		public bool RemoveCallback(TDelegate callback)
		{
			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			InitializeListIfNeeded();
			return callbacks.Remove(new StructClosure<TAction>(action, null));
		}

		/// <summary>
		///     Removes the specified callback from the list of callbacks.
		/// </summary>
		/// <param name="callback">The callback to remove.</param>
		/// <typeparam name="TContextDelegate">The type of the delegate to remove.</typeparam>
		/// <returns><c>true</c> if <paramref name="callback" /> was successfully removed; otherwise, <c>false</c>.</returns>
		public bool RemoveCallback<TContextDelegate>(TContextDelegate callback) where TContextDelegate : Delegate
		{
			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TContextDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			InitializeListIfNeeded();
			return callbacks.Remove(new StructClosure<TAction>(action, null));
		}

		/// <summary>
		///     Clears the list of callbacks.
		/// </summary>
		public void Clear()
		{
			callbacks?.Clear();
		}

		internal void AddFrom(BaseDelegateHandlerList<TDelegate, TAction> other)
		{
			ThrowHelper.ThrowIfNull(other, nameof(other));

			if (other.ListenersCount == 0)
			{
				return;
			}

			if (callbacks == null)
			{
				callbacks = new List<StructClosure<TAction>>(other.callbacks!);
			}
			else
			{
				// Check for duplicates.
				for (int i = 0; i < other.callbacks!.Count; i++)
				{
					ThrowHelper.ThrowIfContains(callbacks, other.callbacks[i]);
				}

				callbacks.AddRange(other.callbacks!);
			}
		}

		internal void RemoveFrom(BaseDelegateHandlerList<TDelegate, TAction> other)
		{
			ThrowHelper.ThrowIfNull(other, nameof(other));

			if (other.ListenersCount == 0 || ListenersCount == 0)
			{
				return;
			}

			for (int i = 0; i < other.callbacks!.Count; i++)
			{
				callbacks!.Remove(other.callbacks[i]);
			}
		}

		SpanOwner<Delegate> IDelegateList.GetDelegates()
		{
			return callbacks == null || callbacks.Count == 0 ? SpanOwner<Delegate>.Empty : EventHelper.GetListeners(callbacks);
		}

		internal SpanOwner<StructClosure<TAction>> GetCallbacks()
		{
			if (callbacks == null || callbacks.Count == 0)
			{
				return SpanOwner<StructClosure<TAction>>.Empty;
			}

			SpanOwner<StructClosure<TAction>> span = SpanOwner<StructClosure<TAction>>.Allocate(callbacks.Count);
			span.CopyFrom(callbacks);

			return span;
		}

		/// <summary>
		///     Helper method to initialize the list of callbacks if it's null.
		/// </summary>
		/// <returns><c>true</c> if the list was initialized; otherwise, <c>false</c></returns>
		[MemberNotNull(nameof(callbacks))]
		private bool InitializeListIfNeeded()
		{
			if (callbacks != null)
			{
				return false;
			}

			// Start with capacity of 2 because we will be adding at least one item, and usually it doesn't go above 2.
			callbacks = new List<StructClosure<TAction>>(2);
			return true;
		}
	}
}