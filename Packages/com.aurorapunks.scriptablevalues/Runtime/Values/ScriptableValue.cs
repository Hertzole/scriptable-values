using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace AuroraPunks.ScriptableValues
{
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
	}
	
	public abstract partial class ScriptableValue<T> : ScriptableValue
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

#if UNITY_INCLUDE_TESTS
		internal bool ValueChangingHasSubscribers { get { return OnValueChanging != null; } }
		internal bool ValueChangedHasSubscribers { get { return OnValueChanged != null; } }
#endif
		
		protected virtual T GetValue()
		{
			return value;
		}

		protected virtual void SetValue(T newValue, bool notify)
		{
			if (Application.isPlaying && IsReadOnly)
			{
				Debug.LogError($"'{name}' is marked as read only and cannot be changed at runtime.");
				return;
			}
			
			if (SetEqualityCheck && EqualityHelper.Equals(newValue, value))
			{
				return;
			}

			PreviousValue = temporaryValue;
			if (notify)
			{
				onValueChanging.Invoke(PreviousValue, newValue);
				OnValueChanging?.Invoke(PreviousValue, newValue);
			}

			value = newValue;
			temporaryValue = newValue;
			if (notify)
			{
				onValueChanged.Invoke(PreviousValue, newValue);
				OnValueChanged?.Invoke(PreviousValue, Value);
			}
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

			if (ResetValueOnStart && !IsReadOnly)
			{
				value = DefaultValue;
				PreviousValue = DefaultValue;
				temporaryValue = DefaultValue;
			}

			OnValueChanging = null;
			OnValueChanged = null;
		}

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