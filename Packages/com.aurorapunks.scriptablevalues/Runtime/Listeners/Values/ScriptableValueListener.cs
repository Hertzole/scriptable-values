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
		private ScriptableValue<TValue> targetValue = default;
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

		public bool IsListening { get; private set; }

		public ScriptableValue<TValue> TargetValue { get { return targetValue; } set { SetTargetValue(value); } }
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
			IsListening = false;

			if (!IsListening && startListening == StartListenEvents.Awake)
			{
				ToggleListening(true);
			}
		}

		protected void Start()
		{
			if (!IsListening && startListening == StartListenEvents.Start)
			{
				ToggleListening(true);
			}
		}

		protected virtual void OnEnable()
		{
			if (!IsListening && startListening == StartListenEvents.OnEnable)
			{
				ToggleListening(true);
			}
		}

		protected virtual void OnDisable()
		{
			if (IsListening && stopListening == StopListenEvents.OnDisable)
			{
				ToggleListening(false);
			}
		}

		protected virtual void OnDestroy()
		{
			if (IsListening && stopListening == StopListenEvents.OnDestroy)
			{
				ToggleListening(false);
			}
		}

		protected virtual void ToggleListening(bool listen)
		{
			IsListening = listen;

			if (targetValue == null)
			{
				return;
			}

			if (listen)
			{
				targetValue.OnValueChanging += OnCurrentValueChanging;
				targetValue.OnValueChanged += OnCurrentValueChanged;
			}
			else
			{
				targetValue.OnValueChanging -= OnCurrentValueChanging;
				targetValue.OnValueChanged -= OnCurrentValueChanged;
			}
		}

		protected virtual void OnCurrentValueChanging(TValue oldValue, TValue newValue)
		{
			if (ShouldInvoke(invokeOn, oldValue, newValue, fromValue, toValue))
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
			if (ShouldInvoke(invokeOn, oldValue, newValue, fromValue, toValue))
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

		protected virtual void SetTargetValue(ScriptableValue<TValue> newValue)
		{
			if (newValue == targetValue)
			{
				return;
			}

			if (targetValue != null && IsListening)
			{
				targetValue.OnValueChanging -= OnCurrentValueChanging;
				targetValue.OnValueChanged -= OnCurrentValueChanged;
			}

			targetValue = newValue;

			if (targetValue != null && IsListening)
			{
				targetValue.OnValueChanging += OnCurrentValueChanging;
				targetValue.OnValueChanged += OnCurrentValueChanged;
			}
		}

		private static bool ShouldInvoke(InvokeEvents invokeOn, TValue previousValue, TValue newValue, TValue fromValue, TValue toValue)
		{
			switch (invokeOn)
			{
				case InvokeEvents.FromValue: // If the old value is the from value.
					return EqualityHelper.Equals(previousValue, fromValue);
				case InvokeEvents.ToValue:
					return EqualityHelper.Equals(newValue, toValue); // If the new value is the to value.
				case InvokeEvents.FromValueToValue:
					return EqualityHelper.Equals(previousValue, fromValue) && EqualityHelper.Equals(newValue, toValue); // If the old value is the from value and the new value is the to value.
				default: // If anything happened (includes any)
					return true;
			}
		}
	}
}