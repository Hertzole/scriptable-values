using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues.Tests
{
	public abstract partial class BaseScriptableValueTest<TType, TValue> : BaseRuntimeTest where TType : ScriptableValue<TValue>
	{
		protected TType TestSetValue(TValue value, TValue startValue = default, TType instance = null)
		{
			if (instance == null)
			{
				instance = CreateInstance<TType>();
				instance.DefaultValue = startValue;
				instance.ResetValueOnStart = true;
				instance.SetEqualityCheck = true;
				instance.ResetValue();

				Assert.AreNotEqual(instance.Value, value, "Value should not be equal to the start value.");
			}

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += OnValueChanging;
			instance.OnValueChanged += OnValueChanged;

			instance.Value = value;

			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");

			instance.OnValueChanging -= OnValueChanging;
			instance.OnValueChanged -= OnValueChanged;

			return instance;

			void OnValueChanging(TValue oldValue, TValue newValue)
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangingInvoked = true;
			}

			void OnValueChanged(TValue oldValue, TValue newValue)
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangedInvoked = true;
			}
		}

		protected void TestSetValue_WithoutNotify(TValue value, TValue startValue = default)
		{
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			Assert.AreNotEqual(instance.Value, value, "Value should not be equal to the start value.");

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) => { valueChangingInvoked = true; };

			instance.OnValueChanged += (oldValue, newValue) => { valueChangedInvoked = true; };

			instance.SetValueWithoutNotify(value);

			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsFalse(valueChangingInvoked, "OnValueChanging should not be invoked.");
			Assert.IsFalse(valueChangedInvoked, "OnValueChanged should not be invoked.");
		}

		protected void TestSetValue_ReadOnly(TValue value, TValue startValue = default)
		{
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			instance.IsReadOnly = true;

			Assert.AreNotEqual(instance.Value, value, "Value should not be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) => { valueChangingInvoked = true; };

			instance.OnValueChanged += (oldValue, newValue) => { valueChangedInvoked = true; };

			LogAssert.Expect(LogType.Error, $"'{instance.name}' is marked as read only and cannot be changed at runtime.");

			instance.Value = value;

			Assert.AreEqual(originalValue, instance.Value, "Value should not be changed.");
			Assert.IsFalse(valueChangingInvoked, "OnValueChanging should not be invoked.");
			Assert.IsFalse(valueChangedInvoked, "OnValueChanged should not be invoked.");
		}

		protected void TestSetValue_SameValue(TValue value, TValue startValue = default)
		{
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			instance.SetEqualityCheck = true;

			Assert.AreEqual(instance.Value, value, "Value should be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) => { valueChangingInvoked = true; };

			instance.OnValueChanged += (oldValue, newValue) => { valueChangedInvoked = true; };

			instance.Value = value;

			Assert.AreEqual(originalValue, instance.Value, "Value should not be changed.");
			Assert.IsFalse(valueChangingInvoked, "OnValueChanging should not be invoked.");
			Assert.IsFalse(valueChangedInvoked, "OnValueChanged should not be invoked.");
		}

		protected void TestSetValue_SameValue_NoEqualsCheck(TValue value, TValue startValue = default)
		{
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			instance.SetEqualityCheck = false;

			Assert.AreEqual(instance.Value, value, "Value should be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) =>
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangingInvoked = true;
			};

			instance.OnValueChanged += (oldValue, newValue) =>
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangedInvoked = true;
			};

			instance.Value = value;

			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");
		}

#if UNITY_EDITOR
		protected void TestSetValue_OnValidate(bool equalsCheck, TValue value, TValue startValue = default)
		{
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.SetEqualityCheck = equalsCheck;
			instance.ResetValue();

			Assert.AreNotEqual(instance.Value, value, "Value should not be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) =>
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangingInvoked = true;
			};

			instance.OnValueChanged += (oldValue, newValue) =>
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangedInvoked = true;
			};

			instance.value = value;
			instance.CallOnValidate_TestOnly();

			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");
		}
#endif // UNITY_EDITOR

		protected void TestRegisterValueChange(ChangeChoice choice)
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			int invokeCount = 0;
			TValue targetValue = MakeDifferentValue(instance.Value);
			switch (choice)
			{
				case ChangeChoice.Changing:
					instance.RegisterValueChangingListener(Callback);
					break;
				case ChangeChoice.Changed:
					instance.RegisterValueChangedListener(Callback);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
			}

			// Act
			instance.Value = targetValue;

			switch (choice)
			{
				case ChangeChoice.Changing:
					instance.UnregisterValueChangingListener(Callback);
					break;
				case ChangeChoice.Changed:
					instance.UnregisterValueChangedListener(Callback);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
			}

			instance.Value = MakeDifferentValue(targetValue);

			// Assert
			Assert.AreEqual(1, invokeCount, "Callback was not invoked once.");
			return;

			void Callback(TValue oldValue, TValue newValue)
			{
				invokeCount++;
				Assert.AreEqual(targetValue, newValue, "New value is not the same as the instance value.");
			}
		}

		protected void TestRegisterValueChangeWithContext(ChangeChoice choice)
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			TValue targetValue = MakeDifferentValue(instance.Value);
			Context context = new Context(targetValue);
			switch (choice)
			{
				case ChangeChoice.Changing:
					instance.RegisterValueChangingListener(Callback, context);
					break;
				case ChangeChoice.Changed:
					instance.RegisterValueChangedListener(Callback, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
			}

			// Act
			instance.Value = targetValue;

			switch (choice)
			{
				case ChangeChoice.Changing:
					instance.UnregisterValueChangingListener<Context>(Callback);
					break;
				case ChangeChoice.Changed:
					instance.UnregisterValueChangedListener<Context>(Callback);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
			}

			instance.Value = MakeDifferentValue(targetValue);

			// Assert
			Assert.AreEqual(1, context.invokeCount, "Callback was not invoked once.");
			return;

			static void Callback(TValue oldValue, TValue newValue, Context context)
			{
				context.invokeCount++;
				Assert.AreEqual(context.targetValue, newValue, "New value is not the same as the instance value.");
			}
		}

		private class Context
		{
			public int invokeCount;
			public readonly TValue targetValue;

			public Context(TValue targetValue)
			{
				invokeCount = 0;
				this.targetValue = targetValue;
			}
		}
	}

	public enum ChangeChoice
	{
		Changing = 0,
		Changed = 1
	}
}