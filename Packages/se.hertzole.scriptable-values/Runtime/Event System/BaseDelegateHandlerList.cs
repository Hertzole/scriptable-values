#nullable enable

using System;
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
	internal abstract class BaseDelegateHandlerList<TDelegate, TAction> : IDelegateList<TDelegate>, IDisposable
		where TDelegate : Delegate where TAction : Delegate
	{
		// Keep track of the disposed state.
		private bool isDisposed = false;

		private readonly PooledList<StructClosure<TAction>> callbacks = new PooledList<StructClosure<TAction>>();

		public int ListenersCount
		{
			get { return callbacks.Count; }
		}

		public void RegisterCallback(TDelegate callback)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			callbacks.Add(new StructClosure<TAction>(action, null));
		}

		public void RegisterCallback<TContextDelegate, TContext>(TContextDelegate callback, TContext context) where TContextDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TContextDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			callbacks.Add(new StructClosure<TAction>(action, context));
		}

		public void RemoveCallback(TDelegate callback)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			callbacks.Remove(new StructClosure<TAction>(action, null));
		}

		public void RemoveCallback<TContextDelegate>(TContextDelegate callback) where TContextDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			// Convert the original delegate to the desired delegate type.
			TAction? action = UnsafeUtility.As<TContextDelegate, TAction>(ref callback);

			// Make sure it was converted properly.
			Assert.IsNotNull(action);

			callbacks.Remove(new StructClosure<TAction>(action, null));
		}

		public void Reset()
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			callbacks.Clear();
		}

		public void AddFrom(BaseDelegateHandlerList<TDelegate, TAction> other)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			other.callbacks.AddFrom(callbacks);
		}

		public void RemoveFrom(BaseDelegateHandlerList<TDelegate, TAction> other)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			callbacks.RemoveFrom(other.callbacks);
		}

		public ReadOnlySpan<Delegate> GetDelegates()
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			return EventHelper.GetListeners(callbacks);
		}

		protected ReadOnlySpan<StructClosure<TAction>> GetCallbacks()
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);
			return callbacks.AsSpan();
		}

		~BaseDelegateHandlerList()
		{
			Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);

			ThrowHelper.ThrowIfDisposed(in isDisposed);

			callbacks.Dispose();
			isDisposed = true;
		}
	}
}