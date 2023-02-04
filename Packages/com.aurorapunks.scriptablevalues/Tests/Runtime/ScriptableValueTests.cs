using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class ScriptableValueTests : BaseRuntimeTest
	{
		// Bool

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
		public void SetValue_SameValue_ScriptableBool([ValueSource(nameof(bools))] bool value)
		{
			TestSetValue_SameValue<ScriptableBool, bool>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableBool([ValueSource(nameof(bools))] bool value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableBool, bool>(value, value);
		}

		// Byte

		[Test]
		public void SetValue_ScriptableByte([ValueSource(nameof(bytes))] byte value)
		{
			TestSetValue<ScriptableByte, byte>(value, (byte) (value - 1));
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableByte([ValueSource(nameof(bytes))] byte value)
		{
			TestSetValue_WithoutNotify<ScriptableByte, byte>(value, (byte) (value - 1));
		}

		[Test]
		public void SetValue_OnValidate_ScriptableByte([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(bytes))] byte value)
		{
			TestSetValue_OnValidate<ScriptableByte, byte>(equalsCheck, value, (byte) (value - 1));
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableByte([ValueSource(nameof(bytes))] byte value)
		{
			TestSetValue_ReadOnly<ScriptableByte, byte>(value, (byte) (value - 1));
		}

		[Test]
		public void SetValue_SameValue_ScriptableByte([ValueSource(nameof(bytes))] byte value)
		{
			TestSetValue_SameValue<ScriptableByte, byte>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableByte([ValueSource(nameof(bytes))] byte value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableByte, byte>(value, value);
		}

		// SByte

		[Test]
		public void SetValue_ScriptableSByte([ValueSource(nameof(sbytes))] sbyte value)
		{
			TestSetValue<ScriptableSByte, sbyte>(value, (sbyte) (value - 1));
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableSByte([ValueSource(nameof(sbytes))] sbyte value)
		{
			TestSetValue_WithoutNotify<ScriptableSByte, sbyte>(value, (sbyte) (value - 1));
		}

		[Test]
		public void SetValue_OnValidate_ScriptableSByte([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(sbytes))] sbyte value)
		{
			TestSetValue_OnValidate<ScriptableSByte, sbyte>(equalsCheck, value, (sbyte) (value - 1));
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableSByte([ValueSource(nameof(sbytes))] sbyte value)
		{
			TestSetValue_ReadOnly<ScriptableSByte, sbyte>(value, (sbyte) (value - 1));
		}

		[Test]
		public void SetValue_SameValue_ScriptableSByte([ValueSource(nameof(sbytes))] sbyte value)
		{
			TestSetValue_SameValue<ScriptableSByte, sbyte>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableSByte([ValueSource(nameof(sbytes))] sbyte value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableSByte, sbyte>(value, value);
		}

		// Short

		[Test]
		public void SetValue_ScriptableShort([ValueSource(nameof(shorts))] short value)
		{
			TestSetValue<ScriptableShort, short>(value, (short) (value - 1));
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableShort([ValueSource(nameof(shorts))] short value)
		{
			TestSetValue_WithoutNotify<ScriptableShort, short>(value, (short) (value - 1));
		}

		[Test]
		public void SetValue_OnValidate_ScriptableShort([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(shorts))] short value)
		{
			TestSetValue_OnValidate<ScriptableShort, short>(equalsCheck, value, (short) (value - 1));
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableShort([ValueSource(nameof(shorts))] short value)
		{
			TestSetValue_ReadOnly<ScriptableShort, short>(value, (short) (value - 1));
		}

		[Test]
		public void SetValue_SameValue_ScriptableShort([ValueSource(nameof(shorts))] short value)
		{
			TestSetValue_SameValue<ScriptableShort, short>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableShort([ValueSource(nameof(shorts))] short value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableShort, short>(value, value);
		}

		// UShort

		[Test]
		public void SetValue_ScriptableUShort([ValueSource(nameof(ushorts))] ushort value)
		{
			TestSetValue<ScriptableUShort, ushort>(value, (ushort) (value - 1));
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableUShort([ValueSource(nameof(ushorts))] ushort value)
		{
			TestSetValue_WithoutNotify<ScriptableUShort, ushort>(value, (ushort) (value - 1));
		}

		[Test]
		public void SetValue_OnValidate_ScriptableUShort([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(ushorts))] ushort value)
		{
			TestSetValue_OnValidate<ScriptableUShort, ushort>(equalsCheck, value, (ushort) (value - 1));
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableUShort([ValueSource(nameof(ushorts))] ushort value)
		{
			TestSetValue_ReadOnly<ScriptableUShort, ushort>(value, (ushort) (value - 1));
		}

		[Test]
		public void SetValue_SameValue_ScriptableUShort([ValueSource(nameof(ushorts))] ushort value)
		{
			TestSetValue_SameValue<ScriptableUShort, ushort>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableUShort([ValueSource(nameof(ushorts))] ushort value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableUShort, ushort>(value, value);
		}

		// Int

		[Test]
		public void SetValue_ScriptableInt([ValueSource(nameof(ints))] int value)
		{
			TestSetValue<ScriptableInt, int>(value, value - 1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableInt([ValueSource(nameof(ints))] int value)
		{
			TestSetValue_WithoutNotify<ScriptableInt, int>(value, value - 1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableInt([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(ints))] int value)
		{
			TestSetValue_OnValidate<ScriptableInt, int>(equalsCheck, value, value - 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableInt([ValueSource(nameof(ints))] int value)
		{
			TestSetValue_ReadOnly<ScriptableInt, int>(value, value - 1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableInt([ValueSource(nameof(ints))] int value)
		{
			TestSetValue_SameValue<ScriptableInt, int>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableInt([ValueSource(nameof(ints))] int value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableInt, int>(value, value);
		}

		// UInt

		[Test]
		public void SetValue_ScriptableUInt([ValueSource(nameof(uints))] uint value)
		{
			TestSetValue<ScriptableUInt, uint>(value, value - 1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableUInt([ValueSource(nameof(uints))] uint value)
		{
			TestSetValue_WithoutNotify<ScriptableUInt, uint>(value, value - 1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableUInt([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(uints))] uint value)
		{
			TestSetValue_OnValidate<ScriptableUInt, uint>(equalsCheck, value, value - 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableUInt([ValueSource(nameof(uints))] uint value)
		{
			TestSetValue_ReadOnly<ScriptableUInt, uint>(value, value - 1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableUInt([ValueSource(nameof(uints))] uint value)
		{
			TestSetValue_SameValue<ScriptableUInt, uint>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableUInt([ValueSource(nameof(uints))] uint value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableUInt, uint>(value, value);
		}

		// Long

		[Test]
		public void SetValue_ScriptableLong([ValueSource(nameof(longs))] long value)
		{
			TestSetValue<ScriptableLong, long>(value, value - 1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableLong([ValueSource(nameof(longs))] long value)
		{
			TestSetValue_WithoutNotify<ScriptableLong, long>(value, value - 1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableLong([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(longs))] long value)
		{
			TestSetValue_OnValidate<ScriptableLong, long>(equalsCheck, value, value - 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableLong([ValueSource(nameof(longs))] long value)
		{
			TestSetValue_ReadOnly<ScriptableLong, long>(value, value - 1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableLong([ValueSource(nameof(longs))] long value)
		{
			TestSetValue_SameValue<ScriptableLong, long>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableLong([ValueSource(nameof(longs))] long value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableLong, long>(value, value);
		}

		// ULong

		[Test]
		public void SetValue_ScriptableULong([ValueSource(nameof(ulongs))] ulong value)
		{
			TestSetValue<ScriptableULong, ulong>(value, value - 1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableULong([ValueSource(nameof(ulongs))] ulong value)
		{
			TestSetValue_WithoutNotify<ScriptableULong, ulong>(value, value - 1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableULong([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(ulongs))] ulong value)
		{
			TestSetValue_OnValidate<ScriptableULong, ulong>(equalsCheck, value, value - 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableULong([ValueSource(nameof(ulongs))] ulong value)
		{
			TestSetValue_ReadOnly<ScriptableULong, ulong>(value, value - 1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableULong([ValueSource(nameof(ulongs))] ulong value)
		{
			TestSetValue_SameValue<ScriptableULong, ulong>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableULong([ValueSource(nameof(ulongs))] ulong value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableULong, ulong>(value, value);
		}

		// Float

		[Test]
		public void SetValue_ScriptableFloat([ValueSource(nameof(floats))] float value)
		{
			TestSetValue<ScriptableFloat, float>(value, value - 1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableFloat([ValueSource(nameof(floats))] float value)
		{
			TestSetValue_WithoutNotify<ScriptableFloat, float>(value, value - 1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableFloat([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(floats))] float value)
		{
			TestSetValue_OnValidate<ScriptableFloat, float>(equalsCheck, value, value - 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableFloat([ValueSource(nameof(floats))] float value)
		{
			TestSetValue_ReadOnly<ScriptableFloat, float>(value, value - 1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableFloat([ValueSource(nameof(floats))] float value)
		{
			TestSetValue_SameValue<ScriptableFloat, float>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableFloat([ValueSource(nameof(floats))] float value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableFloat, float>(value, value);
		}

		// Double

		[Test]
		public void SetValue_ScriptableDouble([ValueSource(nameof(doubles))] double value)
		{
			TestSetValue<ScriptableDouble, double>(value, value - 1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableDouble([ValueSource(nameof(doubles))] double value)
		{
			TestSetValue_WithoutNotify<ScriptableDouble, double>(value, value - 1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableDouble([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(doubles))] double value)
		{
			TestSetValue_OnValidate<ScriptableDouble, double>(equalsCheck, value, value - 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableDouble([ValueSource(nameof(doubles))] double value)
		{
			TestSetValue_ReadOnly<ScriptableDouble, double>(value, value - 1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableDouble([ValueSource(nameof(doubles))] double value)
		{
			TestSetValue_SameValue<ScriptableDouble, double>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableDouble([ValueSource(nameof(doubles))] double value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableDouble, double>(value, value);
		}

		// Decimal

		[Test]
		public void SetValue_ScriptableDecimal([ValueSource(nameof(decimals))] decimal value)
		{
			TestSetValue<ScriptableDecimal, decimal>(value, value - 1);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableDecimal([ValueSource(nameof(decimals))] decimal value)
		{
			TestSetValue_WithoutNotify<ScriptableDecimal, decimal>(value, value - 1);
		}

		[Test]
		public void SetValue_OnValidate_ScriptableDecimal([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(decimals))] decimal value)
		{
			TestSetValue_OnValidate<ScriptableDecimal, decimal>(equalsCheck, value, value - 1);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableDecimal([ValueSource(nameof(decimals))] decimal value)
		{
			TestSetValue_ReadOnly<ScriptableDecimal, decimal>(value, value - 1);
		}

		[Test]
		public void SetValue_SameValue_ScriptableDecimal([ValueSource(nameof(decimals))] decimal value)
		{
			TestSetValue_SameValue<ScriptableDecimal, decimal>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableDecimal([ValueSource(nameof(decimals))] decimal value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableDecimal, decimal>(value, value);
		}

		// String

		[Test]
		public void SetValue_ScriptableString([ValueSource(nameof(strings))] string value)
		{
			TestSetValue<ScriptableString, string>(value, value + "1");
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableString([ValueSource(nameof(strings))] string value)
		{
			TestSetValue_WithoutNotify<ScriptableString, string>(value, value + "1");
		}

		[Test]
		public void SetValue_OnValidate_ScriptableString([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(strings))] string value)
		{
			TestSetValue_OnValidate<ScriptableString, string>(equalsCheck, value, value + "1");
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableString([ValueSource(nameof(strings))] string value)
		{
			TestSetValue_ReadOnly<ScriptableString, string>(value, value + "1");
		}

		[Test]
		public void SetValue_SameValue_ScriptableString([ValueSource(nameof(strings))] string value)
		{
			TestSetValue_SameValue<ScriptableString, string>(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableString([ValueSource(nameof(strings))] string value)
		{
			TestSetValue_SameValue_NoEqualsCheck<ScriptableString, string>(value, value);
		}
		
		// GameObject

		[Test]
		public void SetValue_ScriptableGameObject_FromNull()
		{
			var go = CreateGameObject();
			
			TestSetValue<ScriptableGameObject, GameObject>(go, null);
		}
		
		[Test]
		public void SetValue_ScriptableGameObject_ToNull()
		{
			var go = CreateGameObject();
			
			TestSetValue<ScriptableGameObject, GameObject>(null, go);
		}

		[Test]
		public void SetValue_WithoutNotify_ScriptableBool_FromNull()
		{
			var go = CreateGameObject();
			
			TestSetValue_WithoutNotify<ScriptableGameObject, GameObject>(go, null);
		}
		
		[Test]
		public void SetValue_WithoutNotify_ScriptableBool_ToNull()
		{
			var go = CreateGameObject();
			
			TestSetValue_WithoutNotify<ScriptableGameObject, GameObject>(null, go);
		}
		
		[Test]
		public void SetValue_OnValidate_ScriptableGameObject([ValueSource(nameof(bools))] bool equalsCheck)
		{
			GameObject go = CreateGameObject();
			
			TestSetValue_OnValidate<ScriptableGameObject, GameObject>(equalsCheck, go, null);
		}

		[Test]
		public void SetValue_ReadOnly_ScriptableGameObject()
		{
			GameObject go = CreateGameObject();

			TestSetValue_ReadOnly<ScriptableGameObject, GameObject>(go);
		}

		[Test]
		public void SetValue_SameValue_ScriptableString()
		{
			GameObject go = CreateGameObject();
			
			TestSetValue_SameValue<ScriptableGameObject, GameObject>(go, go);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck_ScriptableGameObject()
		{
			GameObject go = CreateGameObject();

			TestSetValue_SameValue_NoEqualsCheck<ScriptableGameObject, GameObject>(go, go);
		}

		[UnityTest]
		public IEnumerator SetValue_WithoutNotify_ScriptableBool_Destroyed()
		{
			var instance = CreateInstance<ScriptableGameObject>();
			instance.SetEqualityCheck = true;
			instance.ResetValues();
			
			var go = CreateGameObject();
			GameObject expectedValue = go;
			
			var originalValue = instance.Value;
			Debug.Log(originalValue);

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += OnValueChanging;
			instance.OnValueChanged += OnValueChanged;

			instance.Value = go;

			Assert.AreEqual(go, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");

			valueChangingInvoked = false;
			valueChangedInvoked = false;
			
			Destroy(go);
			expectedValue = null;
			
			yield return null;

			Assert.IsNull(go);
			
			instance.Value = null;
			
			Assert.AreEqual(expectedValue, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");

			void OnValueChanging(GameObject oldValue, GameObject newValue)
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(expectedValue, newValue, "New value should be the value being set.");
				valueChangingInvoked = true;
			}
			
			void OnValueChanged(GameObject oldValue, GameObject newValue)
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(expectedValue, newValue, "New value should be the value being set.");
				valueChangedInvoked = true;
			}
		}

		private TType TestSetValue<TType, TValue>(TValue value, TValue startValue = default, TType instance = null) where TType : ScriptableValue<TValue>
		{
			if (instance == null)
			{
				instance = CreateInstance<TType>();
				instance.DefaultValue = startValue;
				instance.ResetValueOnStart = true;
				instance.SetEqualityCheck = true;
				instance.ResetValues();
				
				Assert.AreNotEqual(instance.Value, value, "Value should not be equal to the start value.");
			}

			TValue originalValue = instance.Value;
			Debug.Log(originalValue);

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += OnValueChanging;
			instance.OnValueChanged += OnValueChanged;

			instance.Value = value;

			Debug.Log(instance.Value);

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

		private void TestSetValue_WithoutNotify<TType, TValue>(TValue value, TValue startValue = default) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValues();

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
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValues();

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
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValues();

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
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.ResetValues();

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

		private void TestSetValue_OnValidate<TType, TValue>(bool equalsCheck, TValue value, TValue startValue = default) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.DefaultValue = startValue;
			instance.ResetValueOnStart = true;
			instance.SetEqualityCheck = equalsCheck;
			instance.ResetValues();

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
	}
}