using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class ValueListenerEvents : BaseTest
	{
		private static readonly InvokeEvents[] invokeEvents =
		{
			InvokeEvents.Any,
			InvokeEvents.FromValue,
			InvokeEvents.ToValue,
			InvokeEvents.FromValueToValue
		};

		private static readonly InvokeParameters[] invokeParameters =
		{
			InvokeParameters.Single,
			InvokeParameters.Multiple,
			InvokeParameters.Both
		};

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableBool()
		{
			yield return StartListening_Awake<ScriptableBoolListener, ScriptableBool, bool>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableBool()
		{
			yield return StartListening_Start<ScriptableBoolListener, ScriptableBool, bool>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableBool()
		{
			yield return StartListening_OnEnable<ScriptableBoolListener, ScriptableBool, bool>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableBool()
		{
			yield return StopListening_OnDisable<ScriptableBoolListener, ScriptableBool, bool>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableBool()
		{
			yield return StopListening_OnDestroy<ScriptableBoolListener, ScriptableBool, bool>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableBool()
		{
			SetTargetValue_NewValue<ScriptableBoolListener, ScriptableBool, bool>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableBool()
		{
			SetTargetValue_SameValue<ScriptableBoolListener, ScriptableBool, bool>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableBool()
		{
			SetTargetValue_Null<ScriptableBoolListener, ScriptableBool, bool>();
		}

		[Test]
		public void SetValue_ScriptableBool([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableBoolListener, ScriptableBool, bool>(invokeOn, parameters, true);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableByte()
		{
			yield return StartListening_Awake<ScriptableByteListener, ScriptableByte, byte>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableByte()
		{
			yield return StartListening_Start<ScriptableByteListener, ScriptableByte, byte>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableByte()
		{
			yield return StartListening_OnEnable<ScriptableByteListener, ScriptableByte, byte>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableByte()
		{
			yield return StopListening_OnDisable<ScriptableByteListener, ScriptableByte, byte>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableByte()
		{
			yield return StopListening_OnDestroy<ScriptableByteListener, ScriptableByte, byte>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableByte()
		{
			SetTargetValue_NewValue<ScriptableByteListener, ScriptableByte, byte>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableByte()
		{
			SetTargetValue_SameValue<ScriptableByteListener, ScriptableByte, byte>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableByte()
		{
			SetTargetValue_Null<ScriptableByteListener, ScriptableByte, byte>();
		}

		[Test]
		public void SetValue_ScriptableByte([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableByteListener, ScriptableByte, byte>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableSByte()
		{
			yield return StartListening_Awake<ScriptableSByteListener, ScriptableSByte, sbyte>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableSByte()
		{
			yield return StartListening_Start<ScriptableSByteListener, ScriptableSByte, sbyte>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableSByte()
		{
			yield return StartListening_OnEnable<ScriptableSByteListener, ScriptableSByte, sbyte>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableSByte()
		{
			yield return StopListening_OnDisable<ScriptableSByteListener, ScriptableSByte, sbyte>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableSByte()
		{
			yield return StopListening_OnDestroy<ScriptableSByteListener, ScriptableSByte, sbyte>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableSByte()
		{
			SetTargetValue_NewValue<ScriptableSByteListener, ScriptableSByte, sbyte>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableSByte()
		{
			SetTargetValue_SameValue<ScriptableSByteListener, ScriptableSByte, sbyte>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableSByte()
		{
			SetTargetValue_Null<ScriptableSByteListener, ScriptableSByte, sbyte>();
		}

		[Test]
		public void SetValue_ScriptableSByte([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableSByteListener, ScriptableSByte, sbyte>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableShort()
		{
			yield return StartListening_Awake<ScriptableShortListener, ScriptableShort, short>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableShort()
		{
			yield return StartListening_Start<ScriptableShortListener, ScriptableShort, short>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableShort()
		{
			yield return StartListening_OnEnable<ScriptableShortListener, ScriptableShort, short>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableShort()
		{
			yield return StopListening_OnDisable<ScriptableShortListener, ScriptableShort, short>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableShort()
		{
			yield return StopListening_OnDestroy<ScriptableShortListener, ScriptableShort, short>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableShort()
		{
			SetTargetValue_NewValue<ScriptableShortListener, ScriptableShort, short>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableShort()
		{
			SetTargetValue_SameValue<ScriptableShortListener, ScriptableShort, short>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableShort()
		{
			SetTargetValue_Null<ScriptableShortListener, ScriptableShort, short>();
		}

		[Test]
		public void SetValue_ScriptableShort([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableShortListener, ScriptableShort, short>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableUShort()
		{
			yield return StartListening_Awake<ScriptableUShortListener, ScriptableUShort, ushort>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableUShort()
		{
			yield return StartListening_Start<ScriptableUShortListener, ScriptableUShort, ushort>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableUShort()
		{
			yield return StartListening_OnEnable<ScriptableUShortListener, ScriptableUShort, ushort>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableUShort()
		{
			yield return StopListening_OnDisable<ScriptableUShortListener, ScriptableUShort, ushort>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableUShort()
		{
			yield return StopListening_OnDestroy<ScriptableUShortListener, ScriptableUShort, ushort>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableUShort()
		{
			SetTargetValue_NewValue<ScriptableUShortListener, ScriptableUShort, ushort>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableUShort()
		{
			SetTargetValue_SameValue<ScriptableUShortListener, ScriptableUShort, ushort>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableUShort()
		{
			SetTargetValue_Null<ScriptableUShortListener, ScriptableUShort, ushort>();
		}

		[Test]
		public void SetValue_ScriptableUShort([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableUShortListener, ScriptableUShort, ushort>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableInt()
		{
			yield return StartListening_Awake<ScriptableIntListener, ScriptableInt, int>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableInt()
		{
			yield return StartListening_Start<ScriptableIntListener, ScriptableInt, int>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableInt()
		{
			yield return StartListening_OnEnable<ScriptableIntListener, ScriptableInt, int>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableInt()
		{
			yield return StopListening_OnDisable<ScriptableIntListener, ScriptableInt, int>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableInt()
		{
			yield return StopListening_OnDestroy<ScriptableIntListener, ScriptableInt, int>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableInt()
		{
			SetTargetValue_NewValue<ScriptableIntListener, ScriptableInt, int>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableInt()
		{
			SetTargetValue_SameValue<ScriptableIntListener, ScriptableInt, int>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableInt()
		{
			SetTargetValue_Null<ScriptableIntListener, ScriptableInt, int>();
		}

		[Test]
		public void SetValue_ScriptableInt([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableIntListener, ScriptableInt, int>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableUInt()
		{
			yield return StartListening_Awake<ScriptableUIntListener, ScriptableUInt, uint>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableUInt()
		{
			yield return StartListening_Start<ScriptableUIntListener, ScriptableUInt, uint>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableUInt()
		{
			yield return StartListening_OnEnable<ScriptableUIntListener, ScriptableUInt, uint>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableUInt()
		{
			yield return StopListening_OnDisable<ScriptableUIntListener, ScriptableUInt, uint>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableUInt()
		{
			yield return StopListening_OnDestroy<ScriptableUIntListener, ScriptableUInt, uint>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableUInt()
		{
			SetTargetValue_NewValue<ScriptableUIntListener, ScriptableUInt, uint>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableUInt()
		{
			SetTargetValue_SameValue<ScriptableUIntListener, ScriptableUInt, uint>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableUInt()
		{
			SetTargetValue_Null<ScriptableUIntListener, ScriptableUInt, uint>();
		}

		[Test]
		public void SetValue_ScriptableUInt([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableUIntListener, ScriptableUInt, uint>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableLong()
		{
			yield return StartListening_Awake<ScriptableLongListener, ScriptableLong, long>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableLong()
		{
			yield return StartListening_Start<ScriptableLongListener, ScriptableLong, long>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableLong()
		{
			yield return StartListening_OnEnable<ScriptableLongListener, ScriptableLong, long>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableLong()
		{
			yield return StopListening_OnDisable<ScriptableLongListener, ScriptableLong, long>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableLong()
		{
			yield return StopListening_OnDestroy<ScriptableLongListener, ScriptableLong, long>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableLong()
		{
			SetTargetValue_NewValue<ScriptableLongListener, ScriptableLong, long>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableLong()
		{
			SetTargetValue_SameValue<ScriptableLongListener, ScriptableLong, long>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableLong()
		{
			SetTargetValue_Null<ScriptableLongListener, ScriptableLong, long>();
		}

		[Test]
		public void SetValue_ScriptableLong([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableLongListener, ScriptableLong, long>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableULong()
		{
			yield return StartListening_Awake<ScriptableULongListener, ScriptableULong, ulong>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableULong()
		{
			yield return StartListening_Start<ScriptableULongListener, ScriptableULong, ulong>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableULong()
		{
			yield return StartListening_OnEnable<ScriptableULongListener, ScriptableULong, ulong>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableULong()
		{
			yield return StopListening_OnDisable<ScriptableULongListener, ScriptableULong, ulong>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableULong()
		{
			yield return StopListening_OnDestroy<ScriptableULongListener, ScriptableULong, ulong>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableULong()
		{
			SetTargetValue_NewValue<ScriptableULongListener, ScriptableULong, ulong>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableULong()
		{
			SetTargetValue_SameValue<ScriptableULongListener, ScriptableULong, ulong>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableULong()
		{
			SetTargetValue_Null<ScriptableULongListener, ScriptableULong, ulong>();
		}

		[Test]
		public void SetValue_ScriptableULong([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableULongListener, ScriptableULong, ulong>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableFloat()
		{
			yield return StartListening_Awake<ScriptableFloatListener, ScriptableFloat, float>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableFloat()
		{
			yield return StartListening_Start<ScriptableFloatListener, ScriptableFloat, float>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableFloat()
		{
			yield return StartListening_OnEnable<ScriptableFloatListener, ScriptableFloat, float>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableFloat()
		{
			yield return StopListening_OnDisable<ScriptableFloatListener, ScriptableFloat, float>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableFloat()
		{
			yield return StopListening_OnDestroy<ScriptableFloatListener, ScriptableFloat, float>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableFloat()
		{
			SetTargetValue_NewValue<ScriptableFloatListener, ScriptableFloat, float>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableFloat()
		{
			SetTargetValue_SameValue<ScriptableFloatListener, ScriptableFloat, float>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableFloat()
		{
			SetTargetValue_Null<ScriptableFloatListener, ScriptableFloat, float>();
		}

		[Test]
		public void SetValue_ScriptableFloat([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableFloatListener, ScriptableFloat, float>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableDouble()
		{
			yield return StartListening_Awake<ScriptableDoubleListener, ScriptableDouble, double>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableDouble()
		{
			yield return StartListening_Start<ScriptableDoubleListener, ScriptableDouble, double>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableDouble()
		{
			yield return StartListening_OnEnable<ScriptableDoubleListener, ScriptableDouble, double>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableDouble()
		{
			yield return StopListening_OnDisable<ScriptableDoubleListener, ScriptableDouble, double>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableDouble()
		{
			yield return StopListening_OnDestroy<ScriptableDoubleListener, ScriptableDouble, double>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableDouble()
		{
			SetTargetValue_NewValue<ScriptableDoubleListener, ScriptableDouble, double>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableDouble()
		{
			SetTargetValue_SameValue<ScriptableDoubleListener, ScriptableDouble, double>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableDouble()
		{
			SetTargetValue_Null<ScriptableDoubleListener, ScriptableDouble, double>();
		}

		[Test]
		public void SetValue_ScriptableDouble([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableDoubleListener, ScriptableDouble, double>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableDecimal()
		{
			yield return StartListening_Awake<ScriptableDecimalListener, ScriptableDecimal, decimal>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableDecimal()
		{
			yield return StartListening_Start<ScriptableDecimalListener, ScriptableDecimal, decimal>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableDecimal()
		{
			yield return StartListening_OnEnable<ScriptableDecimalListener, ScriptableDecimal, decimal>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableDecimal()
		{
			yield return StopListening_OnDisable<ScriptableDecimalListener, ScriptableDecimal, decimal>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableDecimal()
		{
			yield return StopListening_OnDestroy<ScriptableDecimalListener, ScriptableDecimal, decimal>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableDecimal()
		{
			SetTargetValue_NewValue<ScriptableDecimalListener, ScriptableDecimal, decimal>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableDecimal()
		{
			SetTargetValue_SameValue<ScriptableDecimalListener, ScriptableDecimal, decimal>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableDecimal()
		{
			SetTargetValue_Null<ScriptableDecimalListener, ScriptableDecimal, decimal>();
		}

		[Test]
		public void SetValue_ScriptableDecimal([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableDecimalListener, ScriptableDecimal, decimal>(invokeOn, parameters, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableString()
		{
			yield return StartListening_Awake<ScriptableStringListener, ScriptableString, string>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableString()
		{
			yield return StartListening_Start<ScriptableStringListener, ScriptableString, string>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableString()
		{
			yield return StartListening_OnEnable<ScriptableStringListener, ScriptableString, string>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableString()
		{
			yield return StopListening_OnDisable<ScriptableStringListener, ScriptableString, string>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableString()
		{
			yield return StopListening_OnDestroy<ScriptableStringListener, ScriptableString, string>();
		}

		[Test]
		public void SetTargetValue_NewValue_ScriptableString()
		{
			SetTargetValue_NewValue<ScriptableStringListener, ScriptableString, string>();
		}

		[Test]
		public void SetTargetValue_SameValue_ScriptableString()
		{
			SetTargetValue_SameValue<ScriptableStringListener, ScriptableString, string>();
		}

		[Test]
		public void SetTargetValue_Null_ScriptableString()
		{
			SetTargetValue_Null<ScriptableStringListener, ScriptableString, string>();
		}

		[Test]
		public void SetValue_ScriptableString([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(invokeEvents))] InvokeEvents invokeOn)
		{
			SetValue<ScriptableStringListener, ScriptableString, string>(invokeOn, parameters, "1");
		}

		private IEnumerator StartListening_Awake<TListener, TType, TValue>() where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = scriptableValue;
			listener.StartListening = StartListenEvents.Awake;
			TListener newListener = Instantiate(listener);
			// Destroy the old listener to ensure that only the new listener is listening.
			Destroy(listener);

			yield return null;

			Assert.AreEqual(StartListenEvents.Awake, newListener.StartListening);
			Assert.IsTrue(newListener.IsListening);
			Assert.IsTrue(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsTrue(scriptableValue.ValueChangedHasSubscribers);
		}

		public IEnumerator StartListening_Start<TListener, TType, TValue>() where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = scriptableValue;
			listener.StartListening = StartListenEvents.Start;

			TListener newListener = Instantiate(listener);
			// Destroy the old listener to ensure that only the new listener is listening.
			Destroy(listener);

			yield return null;

			Assert.AreEqual(StartListenEvents.Start, newListener.StartListening);
			Assert.IsTrue(newListener.IsListening);
			Assert.IsTrue(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsTrue(scriptableValue.ValueChangedHasSubscribers);
		}

		public IEnumerator StartListening_OnEnable<TListener, TType, TValue>() where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = scriptableValue;
			listener.StartListening = StartListenEvents.OnEnable;
			listener.StopListening = StopListenEvents.OnDisable;

			TListener newListener = Instantiate(listener);
			// Destroy the old listener to ensure that only the new listener is listening.
			Destroy(listener);
			newListener.enabled = false;

			yield return null;

			Assert.AreEqual(StartListenEvents.OnEnable, newListener.StartListening);
			Assert.IsFalse(newListener.IsListening);
			Assert.IsFalse(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsFalse(scriptableValue.ValueChangedHasSubscribers);

			newListener.enabled = true;

			yield return null;

			Assert.IsTrue(newListener.IsListening);
			Assert.IsTrue(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsTrue(scriptableValue.ValueChangedHasSubscribers);
		}

		public IEnumerator StopListening_OnDisable<TListener, TType, TValue>() where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = scriptableValue;
			listener.StopListening = StopListenEvents.OnDisable;
			listener.enabled = false;

			yield return null;

			Assert.AreEqual(StopListenEvents.OnDisable, listener.StopListening);
			Assert.IsFalse(listener.IsListening);
			Assert.IsFalse(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsFalse(scriptableValue.ValueChangedHasSubscribers);
		}

		public IEnumerator StopListening_OnDestroy<TListener, TType, TValue>() where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = scriptableValue;
			listener.StopListening = StopListenEvents.OnDestroy;
			Destroy(listener);

			yield return null;

			Assert.AreEqual(StopListenEvents.OnDestroy, listener.StopListening);
			Assert.IsFalse(listener.IsListening);
			Assert.IsFalse(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsFalse(scriptableValue.ValueChangedHasSubscribers);
		}

		public void SetTargetValue_NewValue<TListener, TType, TValue>() where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = scriptableValue;
			TType newValue = CreateInstance<TType>();
			listener.TargetValue = newValue;

			Assert.AreEqual(newValue, listener.TargetValue);
			Assert.IsFalse(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsFalse(scriptableValue.ValueChangedHasSubscribers);
			Assert.IsTrue(newValue.ValueChangingHasSubscribers);
			Assert.IsTrue(newValue.ValueChangedHasSubscribers);
		}

		public void SetTargetValue_SameValue<TListener, TType, TValue>() where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = scriptableValue;

			Assert.AreEqual(scriptableValue, listener.TargetValue);
			Assert.IsTrue(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsTrue(scriptableValue.ValueChangedHasSubscribers);
		}

		public void SetTargetValue_Null<TListener, TType, TValue>() where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = null;

			Assert.IsNull(listener.TargetValue);
			Assert.IsFalse(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsFalse(scriptableValue.ValueChangedHasSubscribers);
		}

		public void SetValue<TListener, TEvent, TValue>(InvokeEvents invokeOn, InvokeParameters invokeParameters, TValue newValue)
			where TListener : ScriptableValueListener<TValue> where TEvent : ScriptableValue<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableValue = CreateInstance<TEvent>();
			listener.TargetValue = scriptableValue;
			listener.InvokeOn = invokeOn;
			listener.InvokeParameters = invokeParameters;

			Assert.AreEqual(invokeOn, listener.InvokeOn);
			Assert.AreEqual(invokeParameters, listener.InvokeParameters);

			bool changingSingleInvoked = false;
			bool changedSingleInvoked = false;
			bool changingMultiInvoked = false;
			bool changedMultiInvoked = false;

			switch (invokeParameters)
			{
				case InvokeParameters.Single:
					listener.OnValueChangingSingle.AddListener(a =>
					{
						Assert.AreEqual(newValue, a);
						changingSingleInvoked = true;
					});

					listener.OnValueChangedSingle.AddListener(a =>
					{
						Assert.AreEqual(newValue, a);
						changedSingleInvoked = true;
					});

					break;
				case InvokeParameters.Multiple:
					listener.OnValueChangingMultiple.AddListener((a, b) =>
					{
						Assert.AreEqual(default, a);
						Assert.AreEqual(newValue, b);
						changingMultiInvoked = true;
					});

					listener.OnValueChangedMultiple.AddListener((a, b) =>
					{
						Assert.AreEqual(default, a);
						Assert.AreEqual(newValue, b);
						changedMultiInvoked = true;
					});

					break;
				case InvokeParameters.Both:
					listener.OnValueChangingSingle.AddListener(a =>
					{
						Assert.AreEqual(newValue, a);
						changingSingleInvoked = true;
					});

					listener.OnValueChangedSingle.AddListener(a =>
					{
						Assert.AreEqual(newValue, a);
						changedSingleInvoked = true;
					});

					listener.OnValueChangingMultiple.AddListener((a, b) =>
					{
						Assert.AreEqual(default, a);
						Assert.AreEqual(newValue, b);
						changingMultiInvoked = true;
					});

					listener.OnValueChangedMultiple.AddListener((a, b) =>
					{
						Assert.AreEqual(default, a);
						Assert.AreEqual(newValue, b);
						changedMultiInvoked = true;
					});

					break;
			}

			switch (invokeOn)
			{
				case InvokeEvents.FromValue:
					listener.FromValue = default;
					break;
				case InvokeEvents.ToValue:
					listener.ToValue = newValue;
					break;
				case InvokeEvents.FromValueToValue:
					listener.FromValue = default;
					listener.ToValue = newValue;
					break;
			}

			scriptableValue.Value = newValue;

			switch (invokeParameters)
			{
				case InvokeParameters.Single:
					Assert.IsTrue(changingSingleInvoked);
					Assert.IsTrue(changedSingleInvoked);
					Assert.IsFalse(changingMultiInvoked);
					Assert.IsFalse(changedMultiInvoked);
					break;
				case InvokeParameters.Multiple:
					Assert.IsTrue(changingMultiInvoked);
					Assert.IsTrue(changedMultiInvoked);
					Assert.IsFalse(changingSingleInvoked);
					Assert.IsFalse(changedSingleInvoked);
					break;
				case InvokeParameters.Both:
					Assert.IsTrue(changingSingleInvoked);
					Assert.IsTrue(changedSingleInvoked);
					Assert.IsTrue(changingMultiInvoked);
					Assert.IsTrue(changedMultiInvoked);
					break;
			}
		}
	}
}