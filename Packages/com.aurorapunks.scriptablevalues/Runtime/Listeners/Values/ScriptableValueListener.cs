using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	public enum StartListenEvents
	{
		Awake = 0,
		Start = 1,
		OnEnable = 2
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

	public enum InvokeParameters
	{
		Single = 1,
		Multiple = 2,
		Both = 3
	}

	public abstract class ScriptableValueListener<TValue> : MonoBehaviour
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
		private InvokeParameters invokeParameters = InvokeParameters.Single;
		[SerializeField] 
		private UnityEvent<TValue> onValueChangingSingle = new UnityEvent<TValue>();
		[SerializeField]
		private UnityEvent<TValue> onValueChangedSingle = new UnityEvent<TValue>();
		[SerializeField]
		private UnityEvent<TValue, TValue> onValueChangingMultiple = new UnityEvent<TValue, TValue>();
		[SerializeField]
		private UnityEvent<TValue, TValue> onValueChangedMultiple = new UnityEvent<TValue, TValue>();

		protected bool isListening;

		public ScriptableValue<TValue> Value { get { return value; } set { this.value = value; } }
		public StartListenEvents StartListening { get { return startListening; } set { startListening = value; } }
		public StopListenEvents StopListening { get { return stopListening; } set { stopListening = value; } }
		public InvokeEvents InvokeOn { get { return invokeOn; } set { invokeOn = value; } }
		public TValue FromValue { get { return fromValue; } set { fromValue = value; } }
		public TValue ToValue { get { return toValue; } set { toValue = value; } }
		public InvokeParameters InvokeParameters { get { return invokeParameters; } set { invokeParameters = value; } }

		public UnityEvent<TValue> OnValueChangingSingle { get { return onValueChangingSingle; } }
		public UnityEvent<TValue> OnValueChangedSingle { get { return onValueChangedSingle; } }
		public UnityEvent<TValue, TValue> OnValueChangingMultiple { get { return onValueChangingMultiple; } }
		public UnityEvent<TValue, TValue> OnValueChangedMultiple { get { return onValueChangedMultiple; } }

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
				switch (invokeParameters)
				{
					case InvokeParameters.Single:
						onValueChangingSingle.Invoke(newValue);
						break;
					case InvokeParameters.Multiple:
						onValueChangingMultiple.Invoke(oldValue, newValue);
						break;
					case InvokeParameters.Both:
						onValueChangingSingle.Invoke(newValue);
						onValueChangingMultiple.Invoke(oldValue, newValue);
						break;
				}
			}
		}

		protected virtual void OnCurrentValueChanged(TValue oldValue, TValue newValue)
		{
			if (invokeOn == InvokeEvents.Any || // If anything happened
			    (invokeOn == InvokeEvents.FromValue && IsEqual(oldValue, fromValue)) || // If the old value is the from value.
			    (invokeOn == InvokeEvents.ToValue && IsEqual(newValue, toValue)) || // If the new value is the to value.
			    (invokeOn == InvokeEvents.FromValueToValue && IsEqual(oldValue, fromValue) && IsEqual(newValue, toValue))) // If the old value is the from value and the new value is the to value. 
			{
				switch (invokeParameters)
				{
					case InvokeParameters.Single:
						onValueChangedSingle.Invoke(newValue);
						break;
					case InvokeParameters.Multiple:
						onValueChangedMultiple.Invoke(oldValue, newValue);
						break;
					case InvokeParameters.Both:
						onValueChangedSingle.Invoke(newValue);
						onValueChangedMultiple.Invoke(oldValue, newValue);
						break;
				}
			}
		}

		private static bool IsEqual(TValue a, TValue b)
		{
			return EqualityHelper.Equals(a, b);
		}
	}
}