using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	public enum StartListenEvents
	{
		Awake = 0,
		OnEnable = 1
	}

	public enum StopListenEvents
	{
		OnDestroy = 0,
		OnDisable = 1
	}

	public enum InvokeEvents
	{
		Any = 0,
		FromValue = 1,
		ToValue = 2,
		FromValueToValue = 3
	}

	public abstract class ValueListener<TValue> : MonoBehaviour
	{
		[SerializeField]
		private ScriptableValue<TValue> value = default;
		[SerializeField]
		private StartListenEvents startListening = StartListenEvents.Awake;
		[SerializeField]
		private StopListenEvents stopListening = StopListenEvents.OnDestroy;
		[SerializeField]
		private InvokeEvents invokeOn = InvokeEvents.Any;
		[SerializeField]
		private TValue fromValue = default;
		[SerializeField]
		private TValue toValue = default;

		[SerializeField]
		private UnityEvent<TValue, TValue> onValueChanging = new UnityEvent<TValue, TValue>();
		[SerializeField]
		private UnityEvent<TValue, TValue> onValueChanged = new UnityEvent<TValue, TValue>();

		protected bool isListening;

		public ScriptableValue<TValue> Value { get { return value; } set { this.value = value; } }
		public StartListenEvents StartListening { get { return startListening; } set { startListening = value; } }
		public StopListenEvents StopListening { get { return stopListening; } set { stopListening = value; } }
		public InvokeEvents InvokeOn { get { return invokeOn; } set { invokeOn = value; } }
		public TValue FromValue { get { return fromValue; } set { fromValue = value; } }
		public TValue ToValue { get { return toValue; } set { toValue = value; } }

		public UnityEvent<TValue, TValue> OnValueChanging { get { return onValueChanging; } }
		public UnityEvent<TValue, TValue> OnValueChanged { get { return onValueChanged; } }

		protected virtual void Awake()
		{
			isListening = false;

			if (!isListening && startListening == StartListenEvents.Awake)
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
				value.OnValueChanging += OnCurrentValueChanging;
				value.OnValueChanged += OnCurrentValueChanged;
			}
			else
			{
				value.OnValueChanging -= OnCurrentValueChanging;
				value.OnValueChanged -= OnCurrentValueChanged;
			}
		}

		protected virtual void OnCurrentValueChanging(TValue oldValue, TValue newValue)
		{
			if (invokeOn == InvokeEvents.Any || // If anything happened
			    (invokeOn == InvokeEvents.FromValue && IsEqual(oldValue, fromValue)) || // If the old value is the from value.
			    (invokeOn == InvokeEvents.ToValue && IsEqual(newValue, toValue)) || // If the new value is the to value.
			    (invokeOn == InvokeEvents.FromValueToValue && IsEqual(oldValue, fromValue) && IsEqual(newValue, toValue))) // If the old value is the from value and the new value is the to value. 
			{
				onValueChanging.Invoke(oldValue, newValue);
			}
		}

		protected virtual void OnCurrentValueChanged(TValue oldValue, TValue newValue)
		{
			if (invokeOn == InvokeEvents.Any || // If anything happened
			    (invokeOn == InvokeEvents.FromValue && IsEqual(oldValue, fromValue)) || // If the old value is the from value.
			    (invokeOn == InvokeEvents.ToValue && IsEqual(newValue, toValue)) || // If the new value is the to value.
			    (invokeOn == InvokeEvents.FromValueToValue && IsEqual(oldValue, fromValue) && IsEqual(newValue, toValue))) // If the old value is the from value and the new value is the to value. 
			{
				onValueChanged.Invoke(oldValue, newValue);
			}
		}

		private static bool IsEqual(TValue a, TValue b)
		{
			return EqualityHelper.Equals(a, b);
		}
	}
}