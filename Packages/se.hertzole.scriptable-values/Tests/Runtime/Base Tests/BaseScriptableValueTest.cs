﻿using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues.Tests
{
	public abstract partial class BaseScriptableValueTest<TType, TValue> : BaseRuntimeTest where TType : ScriptableValue<TValue>
	{
		protected void TestSetValue(TValue value, TValue startValue = default)
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;
			bool notifyChangingInvoked = false;
			bool notifyChangedInvoked = false;

			instance.OnValueChanging += OnValueChanging;
			instance.OnValueChanged += OnValueChanged;
			((INotifyPropertyChanging) instance).PropertyChanging += OnPropertyChanging;
			((INotifyPropertyChanged) instance).PropertyChanged += OnPropertyChanged;

			// Act
			instance.Value = value;

			// Assert
			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");
			Assert.IsTrue(notifyChangingInvoked, "PropertyChanging should be invoked.");
			Assert.IsTrue(notifyChangedInvoked, "PropertyChanged should be invoked.");

			// Cleanup
			instance.OnValueChanging -= OnValueChanging;
			instance.OnValueChanged -= OnValueChanged;
			return;

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

			void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
			{
				if (e.PropertyName == "Value")
				{
					notifyChangingInvoked = true;
				}
			}

			void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == "Value")
				{
					notifyChangedInvoked = true;
				}
			}
		}

		protected void TestSetValue_WithoutNotify(TValue value, TValue startValue = default)
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			Assert.AreNotEqual(instance.Value, value, "Value should not be equal to the start value.");

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;
			bool notifyChangingInvoked = false;
			bool notifyChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) => { valueChangingInvoked = true; };
			instance.OnValueChanged += (oldValue, newValue) => { valueChangedInvoked = true; };
			((INotifyPropertyChanging) instance).PropertyChanging += (sender, args) =>
			{
				if (args == ScriptableValue.valueChangingArgs)
				{
					notifyChangingInvoked = true;
				}
			};

			((INotifyPropertyChanged) instance).PropertyChanged += (sender, args) =>
			{
				if (args == ScriptableValue.valueChangedArgs)
				{
					notifyChangedInvoked = true;
				}
			};

			// Act
			instance.SetValueWithoutNotify(value);

			// Assert
			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsFalse(valueChangingInvoked, "OnValueChanging should not be invoked.");
			Assert.IsFalse(valueChangedInvoked, "OnValueChanged should not be invoked.");
			Assert.IsFalse(notifyChangingInvoked, "PropertyChanging should not be invoked.");
			Assert.IsFalse(notifyChangedInvoked, "PropertyChanged should not be invoked.");
		}

		protected void TestSetValue_ReadOnly(TValue value, TValue startValue = default)
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			instance.IsReadOnly = true;

			Assert.AreNotEqual(instance.Value, value, "Value should not be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;
			bool notifyChangingInvoked = false;
			bool notifyChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) => { valueChangingInvoked = true; };
			instance.OnValueChanged += (oldValue, newValue) => { valueChangedInvoked = true; };
			((INotifyPropertyChanging) instance).PropertyChanging += (sender, args) =>
			{
				if (args == ScriptableValue.valueChangingArgs)
				{
					notifyChangingInvoked = true;
				}
			};

			((INotifyPropertyChanged) instance).PropertyChanged += (sender, args) =>
			{
				if (args == ScriptableValue.valueChangedArgs)
				{
					notifyChangedInvoked = true;
				}
			};

			LogAssert.Expect(LogType.Error, $"'{instance.name}' is marked as read only and cannot be changed at runtime.");

			// Act
			instance.Value = value;

			// Assert
			Assert.AreEqual(originalValue, instance.Value, "Value should not be changed.");
			Assert.IsFalse(valueChangingInvoked, "OnValueChanging should not be invoked.");
			Assert.IsFalse(valueChangedInvoked, "OnValueChanged should not be invoked.");
			Assert.IsFalse(notifyChangingInvoked, "PropertyChanging should not be invoked.");
			Assert.IsFalse(notifyChangedInvoked, "PropertyChanged should not be invoked.");
		}

		protected void TestSetValue_SameValue(TValue value, TValue startValue = default)
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			instance.SetEqualityCheck = true;

			Assert.AreEqual(instance.Value, value, "Value should be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;
			bool notifyChangingInvoked = false;
			bool notifyChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) => { valueChangingInvoked = true; };
			instance.OnValueChanged += (oldValue, newValue) => { valueChangedInvoked = true; };
			((INotifyPropertyChanging) instance).PropertyChanging += (sender, args) =>
			{
				if (args == ScriptableValue.valueChangingArgs)
				{
					notifyChangingInvoked = true;
				}
			};

			((INotifyPropertyChanged) instance).PropertyChanged += (sender, args) =>
			{
				if (args == ScriptableValue.valueChangedArgs)
				{
					notifyChangedInvoked = true;
				}
			};

			// Act
			instance.Value = value;

			// Assert
			Assert.AreEqual(originalValue, instance.Value, "Value should not be changed.");
			Assert.IsFalse(valueChangingInvoked, "OnValueChanging should not be invoked.");
			Assert.IsFalse(valueChangedInvoked, "OnValueChanged should not be invoked.");
			Assert.IsFalse(notifyChangingInvoked, "PropertyChanging should not be invoked.");
			Assert.IsFalse(notifyChangedInvoked, "PropertyChanged should not be invoked.");
		}

		protected void TestSetValue_SameValue_NoEqualsCheck(TValue value, TValue startValue = default)
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValue();

			instance.SetEqualityCheck = false;

			Assert.AreEqual(instance.Value, value, "Value should be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;
			bool notifyChangingInvoked = false;
			bool notifyChangedInvoked = false;

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

			((INotifyPropertyChanging) instance).PropertyChanging += (sender, args) =>
			{
				if (args == ScriptableValue.valueChangingArgs)
				{
					notifyChangingInvoked = true;
				}
			};

			((INotifyPropertyChanged) instance).PropertyChanged += (sender, args) =>
			{
				if (args == ScriptableValue.valueChangedArgs)
				{
					notifyChangedInvoked = true;
				}
			};

			// Act
			instance.Value = value;

			// Assert
			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");
			Assert.IsTrue(notifyChangingInvoked, "PropertyChanging should be invoked.");
			Assert.IsTrue(notifyChangedInvoked, "PropertyChanged should be invoked.");
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