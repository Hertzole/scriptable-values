using System;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     Listens to a <see cref="ScriptableEvent" /> without arguments.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Event Listener", 1100)]
#endif
	public class ScriptableEventListener : ScriptableListenerBase
	{
		[SerializeField]
		[Tooltip("The event to listen to.")]
		private ScriptableEvent targetEvent = default;
		[SerializeField]
		[Tooltip("The event to invoke when the target event is raised.")]
		private UnityEvent onInvoked = new UnityEvent();

		/// <summary>
		///     The event to listen to.
		/// </summary>
		public ScriptableEvent TargetEvent { get { return targetEvent; } set { SetTargetEvent(value); } }

		/// <summary>
		///     The event to invoke when the target event is raised.
		/// </summary>
		public UnityEvent OnInvoked { get { return onInvoked; } }

		/// <inheritdoc />
		protected override void ToggleListening(bool listen)
		{
			base.ToggleListening(listen);

			if (targetEvent == null)
			{
				return;
			}

			if (listen)
			{
				targetEvent.OnInvoked += OnEventInvoked;
			}
			else
			{
				targetEvent.OnInvoked -= OnEventInvoked;
			}
		}

		/// <summary>
		///     Sets the target event.
		/// </summary>
		/// <param name="newEvent">The new event.</param>
		protected virtual void SetTargetEvent(ScriptableEvent newEvent)
		{
			// If the target event is the same, do nothing.
			if (newEvent == targetEvent)
			{
				return;
			}

			// If we are currently listening to the old event, stop listening.
			if (targetEvent != null && IsListening)
			{
				targetEvent.OnInvoked -= OnEventInvoked;
			}

			targetEvent = newEvent;

			// If we should start listening to the new event, start listening.
			if (targetEvent != null && IsListening)
			{
				targetEvent.OnInvoked += OnEventInvoked;
			}
		}

		/// <summary>
		///     Called when the target event is invoked.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <param name="e">The arguments.</param>
		private void OnEventInvoked(object sender, EventArgs e)
		{
			onInvoked.Invoke();
		}
	}
}