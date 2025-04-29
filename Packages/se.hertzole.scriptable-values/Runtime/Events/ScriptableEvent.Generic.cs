#nullable enable

using System;
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
	///     A <see cref="ScriptableObject" /> that can be invoked to trigger an event with an argument.
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
		internal T? editorInvokeValue = default;
#pragma warning restore CS0414 // The field is assigned but its value is never used
#endif

		// The arguments that was passed to the event when it was invoked.
		// This is used to set the previous value.
		private T? currentArgs;
		private T? previousArgs;

		/// <summary>
		///     The arguments that was passed to the event when it was invoked.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public T? PreviousArgs
		{
			get { return previousArgs; }
			private set { SetField(ref previousArgs, value, ScriptableEvent.previousArgsChanging, ScriptableEvent.previousArgsChanged); }
		}

#if UNITY_INCLUDE_TESTS
		internal bool InvokedHasSubscribers
		{
			get { return OnInvoked != null; }
		}
#endif

#if UNITY_EDITOR
		// Used for the CreateAssetMenu attribute order.
		internal const int ORDER = ScriptableEvent.ORDER + 1;
#endif

		/// <summary>
		///     Called when the event is invoked.
		/// </summary>
		public event EventHandler<T>? OnInvoked;

		/// <summary>
		///     Invokes the event with the specified argument and this <see cref="ScriptableEvent{T}" /> as the sender.
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
			if (!OnBeforeInvoked(sender, args))
			{
				return;
			}

			// Skip at least one frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(1 + skipFrames);

			PreviousArgs = currentArgs;
			currentArgs = args;

			OnInvoked?.Invoke(sender, args);
			onInvoked.Invoke(args);

			OnAfterInvoked(sender, args);
		}

		/// <summary>
		///     Called before the event is invoked. This can be used to prevent the event from being invoked.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <param name="args">The argument that was passed to the event.</param>
		/// <returns><c>true</c> if the event should be invoked; otherwise, <c>false</c>.</returns>
		protected virtual bool OnBeforeInvoked(object sender, T args)
		{
			return true;
		}

		/// <summary>
		///     Called after the event has been invoked.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <param name="args">The argument that was passed to the event.</param>
		protected virtual void OnAfterInvoked(object sender, T args) { }

		/// <inheritdoc />
		protected override void OnStart()
		{
			// Remove any subscribers that are left over from play mode.
			// Don't warn if there are any subscribers left over because we already do that in OnExitPlayMode.
			ClearSubscribers();
			PreviousArgs = default;
		}

		/// <summary>
		///     Warns if there are any left-over subscribers to the event.
		/// </summary>
		/// <remarks>This will only be called in the Unity editor and builds with the DEBUG flag.</remarks>
		protected override void WarnIfLeftOverSubscribers()
		{
			base.WarnIfLeftOverSubscribers();
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
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

			OnInvoked = null;
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