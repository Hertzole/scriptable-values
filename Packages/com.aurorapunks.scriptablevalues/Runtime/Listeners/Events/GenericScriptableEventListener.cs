using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     When listeners should invoke their events.
	/// </summary>
	public enum EventInvokeEvents
	{
		/// <summary>When any argument is specified.</summary>
		Any = 0,
		/// <summary>When the argument matches a specific value.</summary>
		FromValue = 1,
		/// <summary>When the argument matches a specific value.</summary>
		ToValue = 2
	}

	public abstract class ScriptableEventListener<TValue> : ScriptableListenerBase
	{
		[SerializeField]
		[Tooltip("The event to listen to.")]
		private ScriptableEvent<TValue> targetEvent = default;
		[SerializeField]
		[Tooltip("When the listener should invoke its events.")]
		private EventInvokeEvents invokeOn = EventInvokeEvents.Any;
		[SerializeField]
		[Tooltip("What the argument needs to have been for the event to be invoked.")]
		private TValue fromValue = default;
		[SerializeField]
		[Tooltip("What the argument needs to be for the event to be invoked.")]
		private TValue toValue = default;
		[SerializeField]
		[Tooltip("The event to invoke when the target event is raised.")]
		private UnityEvent<TValue> onInvoked = new UnityEvent<TValue>();

		/// <summary>
		///     The event to listen to.
		/// </summary>
		public ScriptableEvent<TValue> TargetEvent { get { return targetEvent; } set { SetTargetEvent(value); } }
		/// <summary>
		///     When the listener should invoke its events.
		/// </summary>
		public EventInvokeEvents InvokeOn { get { return invokeOn; } set { invokeOn = value; } }
		/// <summary>
		///     What the argument needs to have been for the event to be invoked.
		/// </summary>
		public TValue FromValue { get { return fromValue; } set { fromValue = value; } }
		/// <summary>
		///     What the argument needs to be for the event to be invoked.
		/// </summary>
		public TValue ToValue { get { return toValue; } set { toValue = value; } }

		/// <summary>
		///     The event to invoke when the target event is raised.
		/// </summary>
		public UnityEvent<TValue> OnInvoked { get { return onInvoked; } }

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
		protected virtual void SetTargetEvent(ScriptableEvent<TValue> newEvent)
		{
			// If the event is the same, do nothing.
			if (newEvent == targetEvent)
			{
				return;
			}

			// If we're already listening to an event, stop listening to it.
			if (targetEvent != null && IsListening)
			{
				targetEvent.OnInvoked -= OnEventInvoked;
			}

			targetEvent = newEvent;

			// If we're supposed to be listening to the new event, start listening to it.
			if (targetEvent != null && IsListening)
			{
				targetEvent.OnInvoked += OnEventInvoked;
			}
		}

		/// <summary>
		///     Called when the target event is invoked.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <param name="args">The arguments.</param>
		private void OnEventInvoked(object sender, TValue args)
		{
			if (ShouldInvoke(invokeOn, targetEvent.PreviousArgs, args, fromValue, toValue))
			{
				onInvoked.Invoke(args);
			}
		}

		/// <summary>
		///     Determines if the event should be invoked.
		/// </summary>
		/// <param name="invokeOn"></param>
		/// <param name="previousValue"></param>
		/// <param name="newValue"></param>
		/// <param name="fromValue"></param>
		/// <param name="toValue"></param>
		/// <returns>True if the event should be invoked; otherwise, false.</returns>
		private static bool ShouldInvoke(EventInvokeEvents invokeOn, TValue previousValue, TValue newValue, TValue fromValue, TValue toValue)
		{
			switch (invokeOn)
			{
				case EventInvokeEvents.FromValue: // If the old value is the from value.
					return EqualityHelper.Equals(previousValue, fromValue);
				case EventInvokeEvents.ToValue:
					return EqualityHelper.Equals(newValue, toValue); // If the new value is the to value.
				default: // If anything happened (includes any)
					return true;
			}
		}
	}
}