using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableValue<T> : RuntimeScriptableObject
	{
		public delegate void OldNewValue<in TValue>(TValue previousValue, TValue newValue);

		[SerializeField] 
		private T value = default;
		[SerializeField]
		private T defaultValue = default;
		[SerializeField]
		[Tooltip("If true, the value will be reset to the default value on play mode start/game boot.")]
		private bool resetValueOnStart = true;
		[SerializeField]
		[Tooltip("If true, an equality check will be run before setting the value to make sure the new value is not the same as the old one.")]
		private bool setEqualityCheck = true;

		public T Value { get { return GetValue(); } set { SetValue(value, true); } }
		public T PreviousValue { get; private set; }
		public T DefaultValue { get { return defaultValue; } set { defaultValue = value; } }
		public bool ResetValueOnStart { get { return resetValueOnStart; } set { resetValueOnStart = value; } }
		public bool SetEqualityCheck { get { return setEqualityCheck; } set { setEqualityCheck = value; } }

		public event OldNewValue<T> OnValueChanging;
		public event OldNewValue<T> OnValueChanged;

		protected virtual T GetValue()
		{
			return value;
		}

		protected virtual void SetValue(T newValue, bool notify)
		{
			if (setEqualityCheck && EqualityHelper.Equals(newValue, value))
			{
				return;
			}

			T oldValue = Value;
			PreviousValue = oldValue;
			if (notify && OnValueChanging != null)
			{
				OnValueChanging.Invoke(oldValue, newValue);
			}

			value = newValue;
			if (notify && OnValueChanged != null)
			{
				OnValueChanged.Invoke(oldValue, Value);
			}
		}

		public void SetValueWithoutNotify(T newValue)
		{
			SetValue(newValue, false);
		}

		public override void ResetValues()
		{
			if (resetValueOnStart)
			{
				value = DefaultValue;
				PreviousValue = DefaultValue;
			}

#if DEBUG
			EventHelper.WarnIfLeftOverSubscribers(OnValueChanging, nameof(OnValueChanging), this);
			EventHelper.WarnIfLeftOverSubscribers(OnValueChanged, nameof(OnValueChanged), this);
#endif
		}
	}
}