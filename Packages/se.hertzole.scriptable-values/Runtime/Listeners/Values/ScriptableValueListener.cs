#nullable enable

using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Hertzole.ScriptableValues
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
		[EditorTooltip("The value to listen to.")]
		private ScriptableValue<TValue>? targetValue = null;
		[SerializeField]
		[EditorTooltip("When listeners should invoke their events.")]
		private InvokeEvents invokeOn = InvokeEvents.Any;
		[SerializeField]
		[EditorTooltip("What the old value needs to be for the event to be invoked.")]
		private TValue? fromValue = default;
		[SerializeField]
		[EditorTooltip("What the new value needs to be for the event to be invoked.")]
		private TValue? toValue = default;

		[SerializeField]
		[EditorTooltip("How many parameters listeners should use when invoking their events.")]
		private InvokeParameters invokeParameters = InvokeParameters.Single;
		[SerializeField]
		[EditorTooltip("The event to invoke when the value is changing.")]
		private UnityEvent<TValue> onValueChangingSingle = new UnityEvent<TValue>();
		[SerializeField]
		[EditorTooltip("The event to invoke when the value has changed.")]
		private UnityEvent<TValue> onValueChangedSingle = new UnityEvent<TValue>();
		[SerializeField]
		[EditorTooltip("The event to invoke when the value is changing.")]
		private UnityEvent<TValue, TValue> onValueChangingMultiple = new UnityEvent<TValue, TValue>();
		[SerializeField]
		[EditorTooltip("The event to invoke when the value has changed.")]
		private UnityEvent<TValue, TValue> onValueChangedMultiple = new UnityEvent<TValue, TValue>();

		/// <summary>
		///     The value to listen to.
		/// </summary>
		public ScriptableValue<TValue>? TargetValue
		{
			get { return targetValue; }
			set { SetTargetValue(value); }
		}
		/// <summary>
		///     When listeners should invoke their events.
		/// </summary>
		public InvokeEvents InvokeOn
		{
			get { return invokeOn; }
			set { invokeOn = value; }
		}
		/// <summary>
		///     What the old value needs to be for the event to be invoked.
		/// </summary>
		public TValue? FromValue
		{
			get { return fromValue; }
			set { fromValue = value; }
		}
		/// <summary>
		///     What the new value needs to be for the event to be invoked.
		/// </summary>
		public TValue? ToValue
		{
			get { return toValue; }
			set { toValue = value; }
		}
		/// <summary>
		///     How many parameters listeners should use when invoking their events.
		/// </summary>
		public InvokeParameters InvokeParameters
		{
			get { return invokeParameters; }
			set { invokeParameters = value; }
		}

		/// <summary>
		///     The event to invoke when the value is changing.
		/// </summary>
		public UnityEvent<TValue> OnValueChangingSingle
		{
			get { return onValueChangingSingle; }
		}
		/// <summary>
		///     The event to invoke when the value has changed.
		/// </summary>
		public UnityEvent<TValue> OnValueChangedSingle
		{
			get { return onValueChangedSingle; }
		}
		/// <summary>
		///     The event to invoke when the value is changing.
		/// </summary>
		public UnityEvent<TValue, TValue> OnValueChangingMultiple
		{
			get { return onValueChangingMultiple; }
		}
		/// <summary>
		///     The event to invoke when the value has changed.
		/// </summary>
		public UnityEvent<TValue, TValue> OnValueChangedMultiple
		{
			get { return onValueChangedMultiple; }
		}

		/// <inheritdoc />
		protected override void SetListening(bool listen)
		{
			base.SetListening(listen);

			// If the target value is null, just stop here.
			if (targetValue == null)
			{
				return;
			}

			// Subscribe or unsubscribe to the target value's events.
			SetListeningToObject(targetValue, listen);
		}

		/// <summary>
		///     Called when the target value is changing.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		private void OnCurrentValueChanging(TValue oldValue, TValue newValue)
		{
			if (ShouldInvoke(invokeOn, oldValue, newValue, fromValue, toValue))
			{
				if (!OnBeforeValueChangingInvoked(oldValue, newValue))
				{
					return;
				}

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

				OnAfterValueChangingInvoked(oldValue, newValue);
			}
		}

		/// <summary>
		///     Called before the value changing event.
		/// </summary>
		/// <param name="oldValue">The current value.</param>
		/// <param name="newValue">The new value being set.</param>
		/// <returns><c>true</c> if the event should be invoked; otherwise, <c>false</c>.</returns>
		protected virtual bool OnBeforeValueChangingInvoked(TValue oldValue, TValue newValue)
		{
			return true;
		}

		/// <summary>
		///     Called after the value changing event.
		/// </summary>
		/// <param name="oldValue">The previous value.</param>
		/// <param name="newValue">The new value that was set.</param>
		protected virtual void OnAfterValueChangingInvoked(TValue oldValue, TValue newValue) { }

		/// <summary>
		///     Called when the target value has changed.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		private void OnCurrentValueChanged(TValue oldValue, TValue newValue)
		{
			if (ShouldInvoke(invokeOn, oldValue, newValue, fromValue, toValue))
			{
				if (!OnBeforeValueChangedInvoked(oldValue, newValue))
				{
					return;
				}

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

				OnAfterValueChangedInvoked(oldValue, newValue);
			}
		}

		/// <summary>
		///     Called before the value changed event.
		/// </summary>
		/// <param name="oldValue">The current value.</param>
		/// <param name="newValue">The new value being set.</param>
		/// <returns><c>true</c> if the event should be invoked; otherwise, <c>false</c>.</returns>
		protected virtual bool OnBeforeValueChangedInvoked(TValue oldValue, TValue newValue)
		{
			return true;
		}

		/// <summary>
		///     Called after the value changed event.
		/// </summary>
		/// <param name="oldValue">The previous value.</param>
		/// <param name="newValue">The new value that was set.</param>
		protected virtual void OnAfterValueChangedInvoked(TValue oldValue, TValue newValue) { }

		/// <summary>
		///     Sets the target value.
		/// </summary>
		/// <param name="newValue"></param>
		private void SetTargetValue(ScriptableValue<TValue>? newValue)
		{
			// If it's the same value, just stop here.
			if (newValue == targetValue)
			{
				return;
			}

			// If we're listening to the old value, unsubscribe from its events.
			if (targetValue != null && IsListening)
			{
				SetListeningToObject(targetValue, false);
			}

			targetValue = newValue;

			// If we're listening to the new value, subscribe to its events.
			if (targetValue != null && IsListening)
			{
				SetListeningToObject(targetValue, true);
			}
		}

		private void SetListeningToObject(ScriptableValue<TValue> target, bool listen)
		{
			ThrowHelper.ThrowIfNull(target, nameof(target));

			if (listen)
			{
				target.OnValueChanging += OnCurrentValueChanging;
				target.OnValueChanged += OnCurrentValueChanged;
			}
			else
			{
				target.OnValueChanging -= OnCurrentValueChanging;
				target.OnValueChanged -= OnCurrentValueChanged;
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
		/// <returns><c>true</c> if the event should be invoked; otherwise, <c>false</c>.</returns>
		private static bool ShouldInvoke(InvokeEvents invokeOn, TValue? previousValue, TValue? newValue, TValue? fromValue, TValue? toValue)
		{
			switch (invokeOn)
			{
				case InvokeEvents.FromValue: // If the old value is the from value.
					return EqualityHelper.Equals(previousValue, fromValue);
				case InvokeEvents.ToValue:
					return EqualityHelper.Equals(newValue, toValue); // If the new value is the to value.
				case InvokeEvents.FromValueToValue:
					return EqualityHelper.Equals(previousValue, fromValue) &&
					       EqualityHelper.Equals(newValue, toValue); // If the old value is the from value and the new value is the to value.
				case InvokeEvents.Any:
				default: // If anything happened (includes any)
					return true;
			}
		}
	}
}