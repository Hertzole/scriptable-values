using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     When listeners should start listening.
	/// </summary>
	public enum StartListenEvents
	{
		/// <summary>Start listening in Awake.</summary>
		Awake = 0,
		/// <summary>Start listening in Start.</summary>
		Start = 1,
		/// <summary>Start listening in OnEnable.</summary>
		OnEnable = 2
	}

	/// <summary>
	///     When listeners should stop listening.
	/// </summary>
	public enum StopListenEvents
	{
		/// <summary>Stop listening in OnDestroy.</summary>
		OnDestroy = 0,
		/// <summary>Stop listening in OnDisable.</summary>
		OnDisable = 1
	}

	/// <summary>
	///     When listeners should invoke their events.
	/// </summary>
	public enum InvokeEvents
	{
		/// <summary>When any value changed.</summary>
		Any = 0,
		/// <summary>When the old value matches a specific value.</summary>
		FromValue = 1,
		/// <summary>When the new value matches a specific value.</summary>
		ToValue = 2,
		/// <summary>When the old value and new value matches a specific value.</summary>
		FromValueToValue = 3
	}

	/// <summary>
	///     How many parameters listeners should use when invoking their events.
	/// </summary>
	public enum InvokeParameters
	{
		/// <summary>Invoke with a single parameter.</summary>
		Single = 1,
		/// <summary>Invoke with multiple parameters.</summary>
		Multiple = 2,
		/// <summary>Invoke with both single and multiple parameters..</summary>
		Both = 3
	}

	/// <summary>
	///     Base class for a component that listens to <see cref="ScriptableValue{T}" />.
	/// </summary>
	/// <typeparam name="TValue">The type to listen for.</typeparam>
	public abstract class ScriptableValueListener<TValue> : ScriptableListenerBase
	{
		[SerializeField]
		[Tooltip("The value to listen to.")]
		private ScriptableValue<TValue> targetValue = default;
		[SerializeField]
		[Tooltip("When listeners should invoke their events.")]
		private InvokeEvents invokeOn = InvokeEvents.Any;
		[SerializeField]
		[Tooltip("What the old value needs to be for the event to be invoked.")]
		private TValue fromValue = default;
		[SerializeField]
		[Tooltip("What the new value needs to be for the event to be invoked.")]
		private TValue toValue = default;

		[SerializeField]
		[Tooltip("How many parameters listeners should use when invoking their events.")]
		private InvokeParameters invokeParameters = InvokeParameters.Single;
		[SerializeField]
		[Tooltip("The event to invoke when the value is changing.")]
		private UnityEvent<TValue> onValueChangingSingle = new UnityEvent<TValue>();
		[SerializeField]
		[Tooltip("The event to invoke when the value has changed.")]
		private UnityEvent<TValue> onValueChangedSingle = new UnityEvent<TValue>();
		[SerializeField]
		[Tooltip("The event to invoke when the value is changing.")]
		private UnityEvent<TValue, TValue> onValueChangingMultiple = new UnityEvent<TValue, TValue>();
		[SerializeField]
		[Tooltip("The event to invoke when the value has changed.")]
		private UnityEvent<TValue, TValue> onValueChangedMultiple = new UnityEvent<TValue, TValue>();

		/// <summary>
		///     The value to listen to.
		/// </summary>
		public ScriptableValue<TValue> TargetValue { get { return targetValue; } set { SetTargetValue(value); } }
		/// <summary>
		///     When listeners should invoke their events.
		/// </summary>
		public InvokeEvents InvokeOn { get { return invokeOn; } set { invokeOn = value; } }
		/// <summary>
		///     What the old value needs to be for the event to be invoked.
		/// </summary>
		public TValue FromValue { get { return fromValue; } set { fromValue = value; } }
		/// <summary>
		///     What the new value needs to be for the event to be invoked.
		/// </summary>
		public TValue ToValue { get { return toValue; } set { toValue = value; } }
		/// <summary>
		///     How many parameters listeners should use when invoking their events.
		/// </summary>
		public InvokeParameters InvokeParameters { get { return invokeParameters; } set { invokeParameters = value; } }

		/// <summary>
		///     The event to invoke when the value is changing.
		/// </summary>
		public UnityEvent<TValue> OnValueChangingSingle { get { return onValueChangingSingle; } }
		/// <summary>
		///     The event to invoke when the value has changed.
		/// </summary>
		public UnityEvent<TValue> OnValueChangedSingle { get { return onValueChangedSingle; } }
		/// <summary>
		///     The event to invoke when the value is changing.
		/// </summary>
		public UnityEvent<TValue, TValue> OnValueChangingMultiple { get { return onValueChangingMultiple; } }
		/// <summary>
		///     The event to invoke when the value has changed.
		/// </summary>
		public UnityEvent<TValue, TValue> OnValueChangedMultiple { get { return onValueChangedMultiple; } }

		/// <inheritdoc />
		protected override void ToggleListening(bool listen)
		{
			base.ToggleListening(listen);
			
			// If the target value is null, just stop here.
			if (targetValue == null)
			{
				return;
			}

			// Subscribe or unsubscribe to the target value's events.
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

		/// <summary>
		///     Called when the target value is changing.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
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

		/// <summary>
		///     Called when the target value has changed.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
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

		/// <summary>
		///     Sets the target value.
		/// </summary>
		/// <param name="newValue"></param>
		protected virtual void SetTargetValue(ScriptableValue<TValue> newValue)
		{
			// If it's the same value, just stop here.
			if (newValue == targetValue)
			{
				return;
			}

			// If we're listening to the old value, unsubscribe from its events.
			if (targetValue != null && IsListening)
			{
				targetValue.OnValueChanging -= OnCurrentValueChanging;
				targetValue.OnValueChanged -= OnCurrentValueChanged;
			}

			targetValue = newValue;

			// If we're listening to the new value, subscribe to its events.
			if (targetValue != null && IsListening)
			{
				targetValue.OnValueChanging += OnCurrentValueChanging;
				targetValue.OnValueChanged += OnCurrentValueChanged;
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