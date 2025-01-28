using System;
using System.Collections.Generic;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A ScriptableObject that can be invoked to trigger an event with an argument.
	/// </summary>
	/// <typeparam name="T">The type of the argument.</typeparam>
	public abstract partial class ScriptableEvent<T> : RuntimeScriptableObject
	{
		[FormerlySerializedAs("onInvokedWithArgs")]
		[SerializeField]
		[EditorTooltip("Called when the event is invoked.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal UnityEvent<T> onInvoked = new UnityEvent<T>();

#if UNITY_EDITOR // This does not need to be included in the build as it's only used in the editor.
#pragma warning disable CS0414 // The field is assigned but its value is never used
		/// <summary>
		///     Used in the editor to invoke the event with an argument from the editor.
		/// </summary>
		[SerializeField]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal T editorInvokeValue = default;
#pragma warning restore CS0414 // The field is assigned but its value is never used
#endif

		internal readonly EventHandlerList<T> onInvokedInternal = new EventHandlerList<T>();

		// The arguments that was passed to the event when it was invoked.
		// This is used to set the previous value.
		private T currentArgs;
		private T previousArgs;

		/// <summary>
		///     The arguments that was passed to the event when it was invoked.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public T PreviousArgs
		{
			get { return previousArgs; }
			private set
			{
				if (EqualityComparer<T>.Default.Equals(previousArgs, value))
				{
					return;
				}

				previousArgs = value;
				NotifyPropertyChanged();
			}
		}

#if UNITY_INCLUDE_TESTS
		internal bool InvokedHasSubscribers
		{
			get { return onInvokedInternal.ListenersCount > 0; }
		}
#endif

#if UNITY_EDITOR
		// Used for the CreateAssetMenu attribute order.
		internal const int ORDER = ScriptableEvent.ORDER + 1;
#endif

		/// <summary>
		///     Called when the event is invoked.
		/// </summary>
		public event EventHandler<T> OnInvoked
		{
			add { RegisterInvokedListener(value); }
			remove { UnregisterInvokedListener(value); }
		}

		private void OnDestroy()
		{
			onInvokedInternal.Dispose();
		}

		/// <summary>
		///     Invokes the event with the specified sender and argument.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <param name="args">The argument to send with the event.</param>
		public void Invoke(object sender, T args)
		{
			InvokeInternal(sender, args, 1);
		}

		/// <summary>
		///     Invokes the event with the specified argument. The sender will be the scriptable object.
		/// </summary>
		/// <param name="args">The argument to send with the event.</param>
		public void Invoke(T args)
		{
			InvokeInternal(this, args, 1);
		}

		/// <summary>
		///     Invokes the event with the specified sender and argument.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <param name="args">The argument to send with the event.</param>
		/// <param name="skipFrames">How many frames of stack trace to skip.</param>
		private void InvokeInternal(object sender, T args, int skipFrames)
		{
			// Skip at least one frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(1 + skipFrames);

			PreviousArgs = currentArgs;
			currentArgs = args;

			onInvokedInternal.Invoke(in sender, in args);
			onInvoked.Invoke(args);
		}

		/// <summary>
		///     Registers a callback that will be called when the event has been invoked.
		/// </summary>
		/// <param name="action">The callback to register.</param>
		public void RegisterInvokedListener(EventHandler<T> action)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));

			onInvokedInternal.AddListener(action);
		}

		/// <summary>
		///     Registers a callback that will be called when the event has been invoked. The provided args will be passed to the
		///     callback.
		/// </summary>
		/// <remarks>
		///     This method is useful for avoiding closure allocations when registering a callback.
		/// </remarks>
		/// <param name="action">The callback to register.</param>
		/// <param name="args">The arguments to pass to the callback.</param>
		/// <typeparam name="TArgs">The type of the arguments.</typeparam>
		public void RegisterInvokedListener<TArgs>(EventHandlerWithContext<T, TArgs> action, TArgs args)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));
			ThrowHelper.ThrowIfNull(args, nameof(args));

			onInvokedInternal.AddListener(action, args);
		}

		/// <summary>
		///     Unregisters a callback from the event.
		/// </summary>
		/// <param name="action">The callback to unregister.</param>
		public void UnregisterInvokedListener(EventHandler<T> action)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));

			onInvokedInternal.RemoveListener(action);
		}

		/// <summary>
		///     Unregisters a callback from the event.
		/// </summary>
		/// <param name="action">The callback to unregister.</param>
		/// <typeparam name="TArgs">The type of the arguments that were used when registering.</typeparam>
		public void UnregisterInvokedListener<TArgs>(EventHandlerWithContext<T, TArgs> action)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));

			onInvokedInternal.RemoveListener(action);
		}

		/// <inheritdoc />
		protected override void OnStart()
		{
#if UNITY_EDITOR
			ResetStackTraces();
#endif

			// Remove any subscribers that are left over from play mode.
			// Don't warn if there are any subscribers left over because we already do that in OnExitPlayMode.
			ClearSubscribers();
			PreviousArgs = default;
		}

		/// <summary>
		///     Removes any subscribers from the event.
		/// </summary>
		/// <param name="warnIfLeftOver">
		///     If true, a warning will be printed in the console if there are any subscribers.
		///     The warning will only be printed in the editor and debug builds.
		/// </param>
		public void ClearSubscribers(bool warnIfLeftOver = false)
		{
#if DEBUG
			if (warnIfLeftOver)
			{
				EventHelper.WarnIfLeftOverSubscribers(onInvokedInternal, nameof(OnInvoked), this);
			}
#endif

			onInvokedInternal.ClearListeners();
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override void OnExitPlayMode()
		{
			EventHelper.WarnIfLeftOverSubscribers(onInvokedInternal, nameof(OnInvoked), this);
		}
#endif
	}
}