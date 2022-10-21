using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	public enum EventInvokeEvents
	{
		Any = 0,
		FromValue = 1,
		ToValue = 2
	}

	public abstract class ScriptableEventListener<TValue> : MonoBehaviour
	{
		[SerializeField]
		private ScriptableEvent<TValue> targetEvent = default;
		[SerializeField]
		private StartListenEvents startListening = StartListenEvents.Awake;
		[SerializeField]
		private StopListenEvents stopListening = StopListenEvents.OnDestroy;
		[SerializeField]
		private EventInvokeEvents invokeOn = EventInvokeEvents.Any;
		[SerializeField]
		private TValue fromValue = default;
		[SerializeField]
		private TValue toValue = default;
		[SerializeField]
		private UnityEvent<TValue> onInvoked = new UnityEvent<TValue>();

		protected bool isListening = false;

		public ScriptableEvent<TValue> TargetEvent { get { return targetEvent; } set { targetEvent = value; } }
		public StartListenEvents StartListening { get { return startListening; } set { startListening = value; } }
		public StopListenEvents StopListening { get { return stopListening; } set { stopListening = value; } }
		public EventInvokeEvents InvokeOn { get { return invokeOn; } set { invokeOn = value; } }
		public TValue FromValue { get { return fromValue; } set { fromValue = value; } }
		public TValue ToValue { get { return toValue; } set { toValue = value; } }

		public UnityEvent<TValue> OnInvoked { get { return onInvoked; } }

		protected virtual void Awake()
		{
			isListening = false;

			if (!isListening && startListening == StartListenEvents.Awake)
			{
				ToggleListening(true);
			}
		}

		protected void Start()
		{
			if (!isListening && startListening == StartListenEvents.Start)
			{
				ToggleListening(true);
			}
		}

		protected virtual void OnEnable()
		{
			if (!isListening && startListening == StartListenEvents.OnEnable)
			{
				ToggleListening(true);
			}
		}

		protected virtual void OnDisable()
		{
			if (isListening && stopListening == StopListenEvents.OnDisable)
			{
				ToggleListening(false);
			}
		}

		protected virtual void OnDestroy()
		{
			if (isListening && stopListening == StopListenEvents.OnDestroy)
			{
				ToggleListening(false);
			}
		}

		protected virtual void ToggleListening(bool listen)
		{
			isListening = listen;

			if (listen)
			{
				targetEvent.OnInvoked += OnEventInvoked;
			}
			else
			{
				targetEvent.OnInvoked -= OnEventInvoked;
			}
		}

		private void OnEventInvoked(object sender, TValue args)
		{
			if (ShouldInvoke(invokeOn, targetEvent.PreviousArgs, args, fromValue, toValue))
			{
				onInvoked.Invoke(args);
			}
		}

		private static bool ShouldInvoke(EventInvokeEvents invokeOn, TValue previousValue, TValue newValue, TValue fromValue, TValue toValue)
		{
			switch (invokeOn)
			{
				case EventInvokeEvents.Any: // If anything happened
					return true;
				case EventInvokeEvents.FromValue: // If the old value is the from value.
					return EqualityHelper.Equals(previousValue, fromValue);
				case EventInvokeEvents.ToValue:
					return EqualityHelper.Equals(newValue, toValue); // If the new value is the to value.
				default:
					return false;
			}
		}
	}
}