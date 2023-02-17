using System;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A ScriptableObject that can be invoked to trigger an event with an argument.
	/// </summary>
	/// <typeparam name="T">The type of the argument.</typeparam>
	public abstract class ScriptableEvent<T> : ScriptableEvent
	{
		[SerializeField]
		[Tooltip("Called when the event is invoked.")]
		private UnityEvent<T> onInvokedWithArgs = new UnityEvent<T>();

#if UNITY_EDITOR // This does not need to be included in the build as it's only used in the editor.
#pragma warning disable CS0414 // The field is assigned but its value is never used
		/// <summary>
		///     Used in the editor to invoke the event with an argument from the editor.
		/// </summary>
		[SerializeField]
		private T editorInvokeValue = default;
#pragma warning restore CS0414 // The field is assigned but its value is never used
#endif

		// The arguments that was passed to the event when it was invoked.
		// This is used to set the previous value.
		private T currentArgs;

		/// <summary>
		///     The arguments that was passed to the event when it was invoked.
		/// </summary>
		public T PreviousArgs { get; private set; }

#if UNITY_INCLUDE_TESTS
		internal new bool InvokedHasSubscribers { get { return OnInvoked != null; } }
#endif

		/// <summary>
		///     Called when the event is invoked.
		/// </summary>
		public new event EventHandler<T> OnInvoked;

		// We must override the base class' invoke method to ensure that the event is invoked with the correct arguments.
		/// <summary>
		///     Invokes the event with the scriptable object as the sender.
		/// </summary>
		public new void Invoke()
		{
			InvokeInternal(this, currentArgs, 1);
		}

		// We must override the base class' invoke method to ensure that the event is invoked with the correct arguments.
		/// <summary>
		///     Invokes the event with the specified sender.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		public new void Invoke(object sender)
		{
			InvokeInternal(sender, currentArgs, 1);
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

			OnInvoked?.Invoke(sender, args);
			onInvokedWithArgs.Invoke(args);
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
		public override void ClearSubscribers(bool warnIfLeftOver = false)
		{
#if DEBUG
			if (warnIfLeftOver)
			{
				EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
			}
#endif

			OnInvoked = null;
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override void OnExitPlayMode()
		{
			base.OnExitPlayMode();
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
		}
#endif
	}

	/// <summary>
	///     A ScriptableObject that can be invoked to trigger an event without any arguments.
	///     <para>Base class for <see cref="ScriptableEvent{T}" /></para>
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Runtime Event", menuName = "Aurora Punks/Scriptable Values/Events/Runtime Event", order = ORDER)]
#endif
	public class ScriptableEvent : RuntimeScriptableObject
	{
		[SerializeField]
		[Tooltip("Called when the event is invoked.")]
		private UnityEvent onInvoked = new UnityEvent();

#if UNITY_INCLUDE_TESTS
		/// <summary>
		///     A test only check to see if the event has any subscribers.
		/// </summary>
		internal bool InvokedHasSubscribers { get { return OnInvoked != null; } }
#endif

#if UNITY_EDITOR
		// Used for the CreateAssetMenu attribute order.
		internal const int ORDER = ScriptableValue.ORDER + 50;
#endif

		/// <summary>
		///     Called when the event is invoked.
		/// </summary>
		public event EventHandler OnInvoked;

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

			OnInvoked?.Invoke(sender, EventArgs.Empty);
			onInvoked.Invoke();
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
				EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
			}
#endif

			OnInvoked = null;
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override void OnExitPlayMode()
		{
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
		}
#endif
	}
}