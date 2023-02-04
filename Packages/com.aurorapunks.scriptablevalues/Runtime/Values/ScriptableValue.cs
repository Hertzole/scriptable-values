using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     Base class for a ScriptableValue without a value.
	/// </summary>
	public abstract class ScriptableValue : RuntimeScriptableObject
	{
		[SerializeField]
		[Tooltip("If read only, the value cannot be changed at runtime.")]
		private bool isReadOnly = false;
		[SerializeField]
		[Tooltip("If true, the value will be reset to the default value on play mode start/game boot.")]
		private bool resetValueOnStart = true;
		[SerializeField]
		[Tooltip("If true, an equality check will be run before setting the value to make sure the new value is not the same as the old one.")]
		private bool setEqualityCheck = true;

		/// <summary>
		///     If read only, the value cannot be changed at runtime.
		/// </summary>
		public bool IsReadOnly { get { return isReadOnly; } set { isReadOnly = value; } }
		/// <summary>
		///     If true, the value will be reset to the default value on play mode start/game boot.
		/// </summary>
		public bool ResetValueOnStart { get { return resetValueOnStart; } set { resetValueOnStart = value; } }
		/// <summary>
		///     If true, an equality check will be run before setting the value to make sure the new value is not the same as the
		///     old one.
		/// </summary>
		public bool SetEqualityCheck { get { return setEqualityCheck; } set { setEqualityCheck = value; } }

#if UNITY_EDITOR
		// Used for the CreateAssetMenu attribute order.
		internal const int ORDER = -1000;
#endif
	}

	/// <summary>
	///     Base class for a ScriptableValue with a value.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	public abstract class ScriptableValue<T> : ScriptableValue
	{
		public delegate void OldNewValue<in TValue>(TValue previousValue, TValue newValue);

		[SerializeField]
		[Tooltip("The current value. This can be changed at runtime.")]
		internal T value = default;
		[SerializeField]
		[Tooltip("The default value. This is used when the value is reset.")]
		private T defaultValue = default;
		[SerializeField]
		[Tooltip("Called before the current value is set.")]
		private UnityEvent<T, T> onValueChanging = new UnityEvent<T, T>();
		[SerializeField]
		[Tooltip("Called after the current value is set.")]
		private UnityEvent<T, T> onValueChanged = new UnityEvent<T, T>();

		// This is mainly use for OnValidate weirdness.
		// We need to have another value that is the value right before it gets modified.
		private T temporaryValue = default;

		private bool valueIsDefault;

		/// <summary>
		///     The current value. This can be changed at runtime.
		/// </summary>
		public T Value
		{
			get { return GetValue(); }
			set
			{
				AddStackTrace(1);

				SetValue(value, true);
			}
		}
		/// <summary>
		///     The previous value before the current value was set.
		/// </summary>
		public T PreviousValue { get; private set; }
		/// <summary>
		///     The default value. This is used when the value is reset.
		/// </summary>
		public T DefaultValue { get { return defaultValue; } set { defaultValue = value; } }

		/// <summary>
		///     Called before the current value is set.
		/// </summary>
		public event OldNewValue<T> OnValueChanging;
		/// <summary>
		///     Called after the current value is set.
		/// </summary>
		public event OldNewValue<T> OnValueChanged;

		/// <summary>
		///     Returns the current value.
		/// </summary>
		/// <returns>The current value.</returns>
		protected virtual T GetValue()
		{
			return value;
		}

		/// <summary>
		///     Sets the current value to a new value.
		/// </summary>
		/// <param name="newValue">The new value that should be set.</param>
		/// <param name="notify">If true, the OnValueChanging/Changed events are invoked.</param>
		protected virtual void SetValue(T newValue, bool notify)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && IsReadOnly)
			{
				Debug.LogError($"'{name}' is marked as read only and cannot be changed at runtime.");
				return;
			}

			// If the equality check is enabled, we don't want to set the value if it's the same as the current value.
			if (SetEqualityCheck && IsSameValue(newValue, value))
			{
				return;
			}

			PreviousValue = temporaryValue;
			// Invoke the OnValueChanging event if notify is true.
			if (notify)
			{
				onValueChanging.Invoke(PreviousValue, newValue);
				OnValueChanging?.Invoke(PreviousValue, newValue);
			}

			// Update the value.
			value = newValue;
			temporaryValue = newValue;

			valueIsDefault = EqualityHelper.Equals(value, default);

			// Invoke the OnValueChanged event if notify is true.
			if (notify)
			{
				onValueChanged.Invoke(PreviousValue, newValue);
				OnValueChanged?.Invoke(PreviousValue, Value);
			}
		}
		
		protected bool IsSameValue(T newValue, T oldValue)
		{
			// If the new value is the default value and the old value isn't, we do allow setting a new value.
			// This is because Unity objects can be destroyed, but this change is not communicated to the ScriptableValue.
			// So if it previously existed but is now null and we want to set it to null, we should allow it.
			if (EqualityHelper.Equals(newValue, default) && !valueIsDefault)
			{
				return false;
			}

			return EqualityHelper.Equals(oldValue, newValue);
		}

		/// <summary>
		///     Sets the current value without invoking the OnValueChanging and OnValueChanged events.
		/// </summary>
		/// <param name="newValue">The new value.</param>
		public void SetValueWithoutNotify(T newValue)
		{
			AddStackTrace();

			SetValue(newValue, false);
		}

		/// <summary>
		///     Resets the value to the default value and removes all event listeners.
		/// </summary>
		public override void ResetValues()
		{
#if UNITY_EDITOR
			ResetStackTraces();
#endif

			// If the value should be reset and it isn't a read only value, we set the value to the default value.
			if (ResetValueOnStart && !IsReadOnly)
			{
				value = DefaultValue;
				PreviousValue = DefaultValue;
				temporaryValue = DefaultValue;
				valueIsDefault = EqualityHelper.Equals(value, default);
			}

			// Remove all subscribers from events, just to make sure we don't have any leaks.
			OnValueChanging = null;
			OnValueChanged = null;
		}

#if UNITY_INCLUDE_TESTS
		/// <summary>
		///     A test only check to see if <see cref="OnValueChanging" /> has subscribers.
		/// </summary>
		internal bool ValueChangingHasSubscribers { get { return OnValueChanging != null; } }
		/// <summary>
		///     A test only check to see if <see cref="OnValueChanged" /> has subscribers.
		/// </summary>
		internal bool ValueChangedHasSubscribers { get { return OnValueChanged != null; } }
#endif

#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			EventHelper.WarnIfLeftOverSubscribers(OnValueChanging, nameof(OnValueChanging), this);
			EventHelper.WarnIfLeftOverSubscribers(OnValueChanged, nameof(OnValueChanged), this);
		}

		internal void CallOnValidate_TestOnly()
		{
			OnValidate();
		}

		protected virtual void OnValidate()
		{
			SetValueOnValidateInternal();
		}

		private void SetValueOnValidateInternal()
		{
			bool equals = false;
			bool originalSetEqualityCheck = SetEqualityCheck;
			// Only do an equality check if we're supposed to and that determines if we can set the new value or not.
			if (SetEqualityCheck)
			{
				equals = EqualityHelper.Equals(PreviousValue, value);
				// We need to turn off the equality check in OnValidate to make sure the value is set and the events are invoked.
				SetEqualityCheck = false;
			}

			// If we can set the value, set it.
			if (!equals)
			{
				SetValue(value, true);
			}

			// Restore the original value.
			SetEqualityCheck = originalSetEqualityCheck;
		}
#endif
	}
}