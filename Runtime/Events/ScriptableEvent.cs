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

#if UNITY_INCLUDE_TESTS
		/// <summary>
		///     A test only check to see if the event has any subscribers.
		/// </summary>
		internal bool InvokedHasSubscribers
		{
			get { return OnInvoked != null; }
		}
#endif

#if UNITY_EDITOR
		// Used for the CreateAssetMenu attribute order.
		internal const int ORDER = ScriptableValue.ORDER + 50;
#endif

		/// <summary>
		///     Called when the event is invoked.
		/// </summary>
		public event EventHandler? OnInvoked;

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

			OnInvoked?.Invoke(sender, EventArgs.Empty);
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

		/// <inheritdoc />
		protected override void OnStart()
		{
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