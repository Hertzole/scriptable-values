using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using System.Diagnostics;
#endif

namespace AuroraPunks.ScriptableValues
{
	public abstract partial class ScriptableValue<T> : RuntimeScriptableObject
	{
		public delegate void OldNewValue<in TValue>(TValue previousValue, TValue newValue);

		[SerializeField]
		[Tooltip("The current value. This can be changed at runtime.")]
		private T value = default;
		[SerializeField]
		[Tooltip("The default value. This is used when the value is reset.")]
		private T defaultValue = default;
		[SerializeField]
		[Tooltip("If true, the value will be reset to the default value on play mode start/game boot.")]
		private bool resetValueOnStart = true;
		[SerializeField]
		[Tooltip("If true, an equality check will be run before setting the value to make sure the new value is not the same as the old one.")]
		private bool setEqualityCheck = true;
		[SerializeField]
		[Tooltip("Called before the current value is set.")]
		private UnityEvent<T, T> onValueChanging = new UnityEvent<T, T>();
		[SerializeField]
		[Tooltip("Called after the current value is set.")]
		private UnityEvent<T, T> onValueChanged = new UnityEvent<T, T>();

		/// <summary>
		///     The current value. This can be changed at runtime.
		/// </summary>
		public T Value
		{
			get { return GetValue(); }
			set
			{
#if UNITY_EDITOR
				AddStackTrace(new StackTrace(1, true));
#endif
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
		///     If true, the value will be reset to the default value on play mode start/game boot.
		/// </summary>
		public bool ResetValueOnStart { get { return resetValueOnStart; } set { resetValueOnStart = value; } }
		/// <summary>
		///     If true, an equality check will be run before setting the value to make sure the new value is not the same as the
		///     old one.
		/// </summary>
		public bool SetEqualityCheck { get { return setEqualityCheck; } set { setEqualityCheck = value; } }

		/// <summary>
		///     Called before the current value is set.
		/// </summary>
		public event OldNewValue<T> OnValueChanging;
		/// <summary>
		///     Called after the current value is set.
		/// </summary>
		public event OldNewValue<T> OnValueChanged;

		protected virtual T GetValue()
		{
			return value;
		}

		protected virtual void SetValue(T newValue, bool notify)
		{
			if (setEqualityCheck && EqualityHelper.Equals(newValue, PreviousValue))
			{
				return;
			}

			T oldValue = PreviousValue;
			PreviousValue = Value;
			if (notify)
			{
				onValueChanging.Invoke(oldValue, newValue);
				OnValueChanging?.Invoke(oldValue, newValue);
			}

			value = newValue;
			if (notify)
			{
				onValueChanged.Invoke(oldValue, newValue);
				OnValueChanged?.Invoke(oldValue, Value);
			}
		}

		/// <summary>
		///     Sets the current value without invoking the OnValueChanging and OnValueChanged events.
		/// </summary>
		/// <param name="newValue">The new value.</param>
		public void SetValueWithoutNotify(T newValue)
		{
#if UNITY_EDITOR
			AddStackTrace(new StackTrace(true));
#endif
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

			if (resetValueOnStart)
			{
				value = DefaultValue;
				PreviousValue = DefaultValue;
			}

#if DEBUG
			EventHelper.WarnIfLeftOverSubscribers(OnValueChanging, nameof(OnValueChanging), this);
			EventHelper.WarnIfLeftOverSubscribers(OnValueChanged, nameof(OnValueChanged), this);
#endif

			OnValueChanging = null;
			OnValueChanged = null;
		}

#if UNITY_EDITOR
		protected virtual void OnValidate()
		{
			SetValueOnValidateInternal();
		}

		private void SetValueOnValidateInternal()
		{
			if (!EqualityHelper.Equals(PreviousValue, value))
			{
				AddStackTrace(new StackTrace(1, true));
			SetValue(value, true);
		}
		}
#endif
	}
}