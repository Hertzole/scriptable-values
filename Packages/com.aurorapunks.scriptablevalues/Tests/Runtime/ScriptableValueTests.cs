using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class ScriptableValueTests : BaseTest
	{
		private static readonly bool[] bools = { true, false };
		private static readonly int[] intStartValues = { 0, 2 };

		[Test]
		public void SetValue_ScriptableBool([ValueSource(nameof(bools))] bool value)
		{
			TestSetValue<ScriptableBool, bool>(value, !value);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableBool([ValueSource(nameof(bools))] bool value)
		{
			TestSetValue_WithoutNotify<ScriptableBool, bool>(value, !value);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableBool([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(bools))] bool value)
		{
			TestSetValue_OnValidate<ScriptableBool, bool>(equalsCheck, value, !value);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableBool([ValueSource(nameof(bools))] bool value)
		{
			TestSetValue_ReadOnly<ScriptableBool, bool>(value, !value);
		}

		[Test]
		public void SetValue_SameValue_ScriptableBool()
		{
			TestSetValue_SameValue<ScriptableBool, bool>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableBool()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableBool, bool>(default);
		}

		[Test]
		public void SetValue_ScriptableByte()
		{
			TestSetValue<ScriptableByte, byte>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableByte()
		{
			TestSetValue_WithoutNotify<ScriptableByte, byte>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableByte([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableByte, byte>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableByte()
		{
			TestSetValue_ReadOnly<ScriptableByte, byte>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableByte()
		{
			TestSetValue_SameValue<ScriptableByte, byte>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableByte()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableByte, byte>(default);
		}

		[Test]
		public void SetValue_ScriptableSByte()
		{
			TestSetValue<ScriptableSByte, sbyte>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableSByte()
		{
			TestSetValue_WithoutNotify<ScriptableSByte, sbyte>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableSByte([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableSByte, sbyte>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableSByte()
		{
			TestSetValue_ReadOnly<ScriptableSByte, sbyte>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableSByte()
		{
			TestSetValue_SameValue<ScriptableSByte, sbyte>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableSByte()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableSByte, sbyte>(default);
		}

		[Test]
		public void SetValue_ScriptableShort()
		{
			TestSetValue<ScriptableShort, short>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableShort()
		{
			TestSetValue_WithoutNotify<ScriptableShort, short>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableShort([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableShort, short>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableShort()
		{
			TestSetValue_ReadOnly<ScriptableShort, short>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableShort()
		{
			TestSetValue_SameValue<ScriptableShort, short>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableShort()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableShort, short>(default);
		}

		[Test]
		public void SetValue_ScriptableUShort()
		{
			TestSetValue<ScriptableUShort, ushort>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableUShort()
		{
			TestSetValue_WithoutNotify<ScriptableUShort, ushort>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableUShort([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableUShort, ushort>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableUShort()
		{
			TestSetValue_ReadOnly<ScriptableUShort, ushort>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableUShort()
		{
			TestSetValue_SameValue<ScriptableUShort, ushort>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableUShort()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableUShort, ushort>(default);
		}

		[Test]
		public void SetValue_ScriptableInt([ValueSource(nameof(intStartValues))] int startValue)
		{
			TestSetValue<ScriptableInt, int>(1, startValue);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableInt()
		{
			TestSetValue_WithoutNotify<ScriptableInt, int>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableInt([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableInt, int>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableInt()
		{
			TestSetValue_ReadOnly<ScriptableInt, int>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableInt()
		{
			TestSetValue_SameValue<ScriptableInt, int>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableInt()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableInt, int>(default);
		}

		[Test]
		public void SetValue_ScriptableUInt()
		{
			TestSetValue<ScriptableUInt, uint>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableUInt()
		{
			TestSetValue_WithoutNotify<ScriptableUInt, uint>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableUInt([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableUInt, uint>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableUInt()
		{
			TestSetValue_ReadOnly<ScriptableUInt, uint>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableUInt()
		{
			TestSetValue_SameValue<ScriptableUInt, uint>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableUInt()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableUInt, uint>(default);
		}

		[Test]
		public void SetValue_ScriptableLong()
		{
			TestSetValue<ScriptableLong, long>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableLong()
		{
			TestSetValue_WithoutNotify<ScriptableLong, long>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableLong([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableLong, long>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableLong()
		{
			TestSetValue_ReadOnly<ScriptableLong, long>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableLong()
		{
			TestSetValue_SameValue<ScriptableLong, long>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableLong()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableLong, long>(default);
		}

		[Test]
		public void SetValue_ScriptableULong()
		{
			TestSetValue<ScriptableULong, ulong>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableULong()
		{
			TestSetValue_WithoutNotify<ScriptableULong, ulong>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableULong([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableULong, ulong>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableULong()
		{
			TestSetValue_ReadOnly<ScriptableULong, ulong>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableULong()
		{
			TestSetValue_SameValue<ScriptableULong, ulong>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableULong()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableULong, ulong>(default);
		}

		[Test]
		public void SetValue_ScriptableFloat()
		{
			TestSetValue<ScriptableFloat, float>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableFloat()
		{
			TestSetValue_WithoutNotify<ScriptableFloat, float>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableFloat([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableFloat, float>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableFloat()
		{
			TestSetValue_ReadOnly<ScriptableFloat, float>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableFloat()
		{
			TestSetValue_SameValue<ScriptableFloat, float>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableFloat()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableFloat, float>(default);
		}

		[Test]
		public void SetValue_ScriptableDouble()
		{
			TestSetValue<ScriptableDouble, double>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableDouble()
		{
			TestSetValue_WithoutNotify<ScriptableDouble, double>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableDouble([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableDouble, double>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableDouble()
		{
			TestSetValue_ReadOnly<ScriptableDouble, double>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableDouble()
		{
			TestSetValue_SameValue<ScriptableDouble, double>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableDouble()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableDouble, double>(default);
		}

		[Test]
		public void SetValue_ScriptableDecimal()
		{
			TestSetValue<ScriptableDecimal, decimal>(1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableDecimal()
		{
			TestSetValue_WithoutNotify<ScriptableDecimal, decimal>(1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableDecimal([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableDecimal, decimal>(equalsCheck, 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableDecimal()
		{
			TestSetValue_ReadOnly<ScriptableDecimal, decimal>(1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableDecimal()
		{
			TestSetValue_SameValue<ScriptableDecimal, decimal>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableDecimal()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableDecimal, decimal>(default);
		}

		[Test]
		public void SetValue_ScriptableString()
		{
			TestSetValue<ScriptableString, string>("1");
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableString()
		{
			TestSetValue_WithoutNotify<ScriptableString, string>("1");
		}

		[Test]
		public void SetValue_OnValidate_ScriptableString([ValueSource(nameof(bools))] bool equalsCheck)
		{
			TestSetValue_OnValidate<ScriptableString, string>(equalsCheck, "1");
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableString()
		{
			TestSetValue_ReadOnly<ScriptableString, string>("1");
		}

		[Test]
		public void SetValue_SameValue_ScriptableString()
		{
			TestSetValue_SameValue<ScriptableString, string>(default);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableString()
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableString, string>(default);
		}

		private void TestSetValue<TType, TValue>(TValue value, TValue startValue = default) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.value = startValue;

			Assert.AreNotEqual(instance.Value, value, "Value should not be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) =>
			{
				Assert.AreEqual(originalValue, oldValue, "Old value should be the original value.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangingInvoked = true;
			};

			instance.OnValueChanged += (oldValue, newValue) =>
			{
				Assert.AreEqual(originalValue, oldValue, "Old value should be the original value.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangedInvoked = true;
			};

			instance.Value = value;

			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");
		}

		private void TestSetValue_WithoutNotify<TType, TValue>(TValue value, TValue startValue = default) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.value = startValue;

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

		private void TestSetValue_ReadOnly<TType, TValue>(TValue value, TValue startValue = default) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.value = startValue;
			instance.name = "Instance";

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

		private void TestSetValue_SameValue<TType, TValue>(TValue value, TValue startValue = default) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.value = startValue;
			instance.name = "Instance";

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

		private void TestSetValue_SameValue_NoEqualsCheck<TType, TValue>(TValue value, TValue startValue = default) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.value = startValue;

			instance.SetEqualityCheck = false;

			Assert.AreEqual(instance.Value, value, "Value should be equal to the start value.");

			TValue originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += (oldValue, newValue) =>
			{
				Assert.AreEqual(originalValue, oldValue, "Old value should be the original value.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangingInvoked = true;
			};

			instance.OnValueChanged += (oldValue, newValue) =>
			{
				Assert.AreEqual(originalValue, oldValue, "Old value should be the original value.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangedInvoked = true;
			};

			instance.Value = value;

			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");
		}

		private void TestSetValue_OnValidate<TType, TValue>(bool equalsCheck, TValue value, TValue startValue = default) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.value = startValue;
			instance.SetEqualityCheck = equalsCheck;

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
				Assert.AreEqual(originalValue, oldValue, "Old value should be the original value.");
				Assert.AreEqual(value, newValue, "New value should be the value being set.");
				valueChangedInvoked = true;
			};

			instance.value = value;
			instance.CallOnValidate_TestOnly();

			Assert.AreEqual(value, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");
		}
	}
}