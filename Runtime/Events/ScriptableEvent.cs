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
	///     A ScriptableObject that can be invoked to trigger an event without any arguments.
	///     <para>Base class for <see cref="ScriptableEvent{T}" /></para>
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Runtime Event", menuName = "Hertzole/Scriptable Values/Events/Runtime Event", order = ORDER)]
#endif
	public class ScriptableEvent : RuntimeScriptableObject
	{
		[SerializeField]
		[EditorTooltip("Called when the event is invoked.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal UnityEvent onInvoked = new UnityEvent();

		private readonly SimpleEventList onInvokedInternal = new SimpleEventList();
		
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

		protected virtual void OnDestroy()
		{
			onInvokedInternal.Dispose();
		}

		/// <summary>
		///     Invokes the event with the scriptable object as the sender.
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
			// Skip at least one frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(1 + skipFrames);

			onInvokedInternal.Invoke(in sender);
			onInvoked.Invoke();
		}

		public void RegisterInvokedListener(EventHandler action)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));
			
			onInvokedInternal.AddListener(action);
		}

		public void RegisterInvokedListener<TArgs>(EventHandlerWithContext<TArgs> action, TArgs args)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));
			ThrowHelper.ThrowIfNull(args, nameof(args));
			
			onInvokedInternal.AddListener(action, args);
		}
		
		public void UnregisterInvokedListener(EventHandler action)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));
			
			onInvokedInternal.RemoveListener(action);
		}
		
		public void UnregisterInvokedListener<TArgs>(EventHandlerWithContext<TArgs> action)
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
		}

		/// <summary>
		///     Removes any subscribers from the event.
		/// </summary>
		/// <param name="warnIfLeftOver">
		///     If true, a warning will be printed in the console if there are any subscribers.
		///     The warning will only be printed in the editor and debug builds.
		/// </param>
		public virtual void ClearSubscribers(bool warnIfLeftOver = false)
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