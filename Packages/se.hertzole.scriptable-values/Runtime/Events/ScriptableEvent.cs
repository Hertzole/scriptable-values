#nullable enable

using System;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableObject" /> that can be invoked to trigger an event without any arguments.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Runtime Event", menuName = "Hertzole/Scriptable Values/Events/Runtime Event", order = ORDER)]
#endif
	public partial class ScriptableEvent : RuntimeScriptableObject
	{
		[SerializeField]
		[EditorTooltip("Called when the event is invoked.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal UnityEvent onInvoked = new UnityEvent();

		private readonly DelegateHandlerList<EventHandler, object, EventArgs> onInvokedInternal = new DelegateHandlerList<EventHandler, object, EventArgs>();

#if UNITY_INCLUDE_TESTS
		/// <summary>
		///     A test only check to see if the event has any subscribers.
		/// </summary>
		internal bool InvokedHasSubscribers
		{
			get { return onInvokedInternal.ListenersCount > 0; }
		}
#endif

#if UNITY_EDITOR
		// Used for the CreateAssetMenu attribute order.
		internal const int ORDER = ScriptableValue.ORDER + 50;
#endif

		/// <summary>
		///     Called when the event is invoked.
		/// </summary>
		public event EventHandler OnInvoked
		{
			add { RegisterInvokedListener(value); }
			remove { UnregisterInvokedListener(value); }
		}

		/// <summary>
		///     Invokes the event with this <see cref="ScriptableEvent" /> as the sender.
		/// </summary>
		public void Invoke()
		{
			InvokeInternal(this, 1);
		}

		/// <summary>
		///     Invokes the event with the specified sender.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		public void Invoke(object sender)
		{
			InvokeInternal(sender, 1);
		}

		/// <summary>
		///     Invokes the event with the specified sender and argument.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <param name="skipFrames">How many frames of stack trace to skip.</param>
		private void InvokeInternal(object sender, int skipFrames)
		{
			if (!OnBeforeInvoked(sender))
			{
				return;
			}

			// Skip at least one frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(1 + skipFrames);

			onInvokedInternal.Invoke(sender, EventArgs.Empty);
			onInvoked.Invoke();

			OnAfterInvoked(sender);
		}

		/// <summary>
		///     Called before the event is invoked. This can be used to prevent the event from being invoked.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <returns><c>true</c> if the event should be invoked; otherwise, <c>false</c>.</returns>
		protected virtual bool OnBeforeInvoked(object sender)
		{
			return true;
		}

		/// <summary>
		///     Called after the event has been invoked.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		protected virtual void OnAfterInvoked(object sender) { }

		/// <summary>
		///     Registers a callback to be called when <see cref="ScriptableEvent" /> is invoked.
		/// </summary>
		/// <param name="callback">The callback method to call.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void RegisterInvokedListener(EventHandler callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onInvokedInternal.RegisterCallback(callback);
		}

		/// <summary>
		///     Registers a callback to be called when the <see cref="ScriptableEvent" /> is invoked with additional context.
		/// </summary>
		/// <remarks>This method can be used to avoid closure allocations on your events.</remarks>
		/// <param name="callback">The callback to register.</param>
		/// <param name="context">The context to pass to the callback.</param>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="callback" /> is <c>null</c>. Or <paramref name="context" /> is
		///     <c>null</c>.
		/// </exception>
		public void RegisterInvokedListener<TContext>(EventHandlerWithContext<TContext> callback, TContext context)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));
			ThrowHelper.ThrowIfNull(context, nameof(context));

			onInvokedInternal.RegisterCallback(callback, context);
		}

		/// <summary>
		///     Unregisters a callback from the <see cref="ScriptableEvent" /> event.
		/// </summary>
		/// <param name="callback">The callback method to unregister.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void UnregisterInvokedListener(EventHandler callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onInvokedInternal.RemoveCallback(callback);
		}

		/// <summary>
		///     Unregisters a callback with context from the <see cref="ScriptableEvent" /> event.
		/// </summary>
		/// <param name="callback">The callback method to unregister.</param>
		/// <typeparam name="TContext">The type of the context that was used in the callback.</typeparam>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void UnregisterInvokedListener<TContext>(EventHandlerWithContext<TContext> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onInvokedInternal.RemoveCallback(callback);
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
		}

		/// <summary>
		///     Warns if there are any left-over subscribers to the event.
		/// </summary>
		/// <remarks>This will only be called in the Unity editor and builds with the DEBUG flag.</remarks>
		protected override void WarnIfLeftOverSubscribers()
		{
			base.WarnIfLeftOverSubscribers();
			EventHelper.WarnIfLeftOverSubscribers(onInvokedInternal, nameof(OnInvoked), this);
		}

		/// <summary>
		///     Removes any subscribers from the event.
		/// </summary>
		/// <param name="warnIfLeftOver">
		///     If <c>true</c>, a warning will be printed in the console if there are any subscribers.
		///     The warning will only be printed in the editor and debug builds.
		/// </param>
		public void ClearSubscribers(bool warnIfLeftOver = false)
		{
#if DEBUG
			if (warnIfLeftOver)
			{
				WarnIfLeftOverSubscribers();
			}
#endif

			onInvokedInternal.Reset();
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override void OnExitPlayMode()
		{
			WarnIfLeftOverSubscribers();
		}
#endif
	}
}