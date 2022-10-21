using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class GenericEventListenerTests : BaseTest
	{
		private static readonly EventInvokeEvents[] invokeEvents =
		{
			EventInvokeEvents.Any,
			EventInvokeEvents.FromValue,
			EventInvokeEvents.ToValue
		};

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableBoolEvent()
		{
			yield return StartListening_Awake<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableBoolEvent()
		{
			yield return StartListening_Start<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableBoolEvent()
		{
			yield return StartListening_OnEnable<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableBoolEvent()
		{
			yield return StopListening_OnDisable<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableBoolEvent()
		{
			yield return StopListening_OnDestroy<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableBoolEvent()
		{
			SetTargetEvent_NewEvent<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableBoolEvent()
		{
			SetTargetEvent_SameEvent<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableBoolEvent()
		{
			SetTargetEvent_Null<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[Test]
		public void OnInvoked_ScriptableBoolEvent()
		{
			OnInvoked<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableBoolEvent()
		{
			OnInvoked_ChangeEvent<ScriptableBoolEventListener, ScriptableBoolEvent, bool>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableBoolEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableBoolEventListener, ScriptableBoolEvent, bool>(invokeOn, true);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableBoolEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableBoolEventListener, ScriptableBoolEvent, bool>(invokeOn, true);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableByteEvent()
		{
			yield return StartListening_Awake<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableByteEvent()
		{
			yield return StartListening_Start<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableByteEvent()
		{
			yield return StartListening_OnEnable<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableByteEvent()
		{
			yield return StopListening_OnDisable<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableByteEvent()
		{
			yield return StopListening_OnDestroy<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableByteEvent()
		{
			SetTargetEvent_NewEvent<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableByteEvent()
		{
			SetTargetEvent_SameEvent<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableByteEvent()
		{
			SetTargetEvent_Null<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[Test]
		public void OnInvoked_ScriptableByteEvent()
		{
			OnInvoked<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableByteEvent()
		{
			OnInvoked_ChangeEvent<ScriptableByteEventListener, ScriptableByteEvent, byte>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableByteEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableByteEventListener, ScriptableByteEvent, byte>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableByteEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableByteEventListener, ScriptableByteEvent, byte>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableSByteEvent()
		{
			yield return StartListening_Awake<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableSByteEvent()
		{
			yield return StartListening_Start<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableSByteEvent()
		{
			yield return StartListening_OnEnable<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableSByteEvent()
		{
			yield return StopListening_OnDisable<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableSByteEvent()
		{
			yield return StopListening_OnDestroy<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableSByteEvent()
		{
			SetTargetEvent_NewEvent<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableSByteEvent()
		{
			SetTargetEvent_SameEvent<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableSByteEvent()
		{
			SetTargetEvent_Null<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[Test]
		public void OnInvoked_ScriptableSByteEvent()
		{
			OnInvoked<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableSByteEvent()
		{
			OnInvoked_ChangeEvent<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableSByteEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableSByteEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableSByteEventListener, ScriptableSByteEvent, sbyte>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableShortEvent()
		{
			yield return StartListening_Awake<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableShortEvent()
		{
			yield return StartListening_Start<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableShortEvent()
		{
			yield return StartListening_OnEnable<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableShortEvent()
		{
			yield return StopListening_OnDisable<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableShortEvent()
		{
			yield return StopListening_OnDestroy<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableShortEvent()
		{
			SetTargetEvent_NewEvent<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableShortEvent()
		{
			SetTargetEvent_SameEvent<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableShortEvent()
		{
			SetTargetEvent_Null<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[Test]
		public void OnInvoked_ScriptableShortEvent()
		{
			OnInvoked<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableShortEvent()
		{
			OnInvoked_ChangeEvent<ScriptableShortEventListener, ScriptableShortEvent, short>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableShortEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableShortEventListener, ScriptableShortEvent, short>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableShortEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableShortEventListener, ScriptableShortEvent, short>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableUShortEvent()
		{
			yield return StartListening_Awake<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableUShortEvent()
		{
			yield return StartListening_Start<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableUShortEvent()
		{
			yield return StartListening_OnEnable<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableUShortEvent()
		{
			yield return StopListening_OnDisable<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableUShortEvent()
		{
			yield return StopListening_OnDestroy<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableUShortEvent()
		{
			SetTargetEvent_NewEvent<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableUShortEvent()
		{
			SetTargetEvent_SameEvent<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableUShortEvent()
		{
			SetTargetEvent_Null<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[Test]
		public void OnInvoked_ScriptableUShortEvent()
		{
			OnInvoked<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableUShortEvent()
		{
			OnInvoked_ChangeEvent<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableUShortEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableUShortEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableUShortEventListener, ScriptableUShortEvent, ushort>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableIntEvent()
		{
			yield return StartListening_Awake<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableIntEvent()
		{
			yield return StartListening_Start<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableIntEvent()
		{
			yield return StartListening_OnEnable<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableIntEvent()
		{
			yield return StopListening_OnDisable<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableIntEvent()
		{
			yield return StopListening_OnDestroy<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableIntEvent()
		{
			SetTargetEvent_NewEvent<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableIntEvent()
		{
			SetTargetEvent_SameEvent<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableIntEvent()
		{
			SetTargetEvent_Null<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[Test]
		public void OnInvoked_ScriptableIntEvent()
		{
			OnInvoked<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableIntEvent()
		{
			OnInvoked_ChangeEvent<ScriptableIntEventListener, ScriptableIntEvent, int>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableIntEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableIntEventListener, ScriptableIntEvent, int>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableIntEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableIntEventListener, ScriptableIntEvent, int>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableUIntEvent()
		{
			yield return StartListening_Awake<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableUIntEvent()
		{
			yield return StartListening_Start<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableUIntEvent()
		{
			yield return StartListening_OnEnable<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableUIntEvent()
		{
			yield return StopListening_OnDisable<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableUIntEvent()
		{
			yield return StopListening_OnDestroy<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableUIntEvent()
		{
			SetTargetEvent_NewEvent<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableUIntEvent()
		{
			SetTargetEvent_SameEvent<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableUIntEvent()
		{
			SetTargetEvent_Null<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[Test]
		public void OnInvoked_ScriptableUIntEvent()
		{
			OnInvoked<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableUIntEvent()
		{
			OnInvoked_ChangeEvent<ScriptableUIntEventListener, ScriptableUIntEvent, uint>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableUIntEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableUIntEventListener, ScriptableUIntEvent, uint>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableUIntEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableUIntEventListener, ScriptableUIntEvent, uint>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableLongEvent()
		{
			yield return StartListening_Awake<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableLongEvent()
		{
			yield return StartListening_Start<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableLongEvent()
		{
			yield return StartListening_OnEnable<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableLongEvent()
		{
			yield return StopListening_OnDisable<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableLongEvent()
		{
			yield return StopListening_OnDestroy<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableLongEvent()
		{
			SetTargetEvent_NewEvent<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableLongEvent()
		{
			SetTargetEvent_SameEvent<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableLongEvent()
		{
			SetTargetEvent_Null<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[Test]
		public void OnInvoked_ScriptableLongEvent()
		{
			OnInvoked<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableLongEvent()
		{
			OnInvoked_ChangeEvent<ScriptableLongEventListener, ScriptableLongEvent, long>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableLongEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableLongEventListener, ScriptableLongEvent, long>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableLongEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableLongEventListener, ScriptableLongEvent, long>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableULongEvent()
		{
			yield return StartListening_Awake<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableULongEvent()
		{
			yield return StartListening_Start<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableULongEvent()
		{
			yield return StartListening_OnEnable<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableULongEvent()
		{
			yield return StopListening_OnDisable<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableULongEvent()
		{
			yield return StopListening_OnDestroy<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableULongEvent()
		{
			SetTargetEvent_NewEvent<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableULongEvent()
		{
			SetTargetEvent_SameEvent<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableULongEvent()
		{
			SetTargetEvent_Null<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[Test]
		public void OnInvoked_ScriptableULongEvent()
		{
			OnInvoked<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableULongEvent()
		{
			OnInvoked_ChangeEvent<ScriptableULongEventListener, ScriptableULongEvent, ulong>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableULongEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableULongEventListener, ScriptableULongEvent, ulong>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableULongEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableULongEventListener, ScriptableULongEvent, ulong>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableFloatEvent()
		{
			yield return StartListening_Awake<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableFloatEvent()
		{
			yield return StartListening_Start<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableFloatEvent()
		{
			yield return StartListening_OnEnable<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableFloatEvent()
		{
			yield return StopListening_OnDisable<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableFloatEvent()
		{
			yield return StopListening_OnDestroy<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableFloatEvent()
		{
			SetTargetEvent_NewEvent<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableFloatEvent()
		{
			SetTargetEvent_SameEvent<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableFloatEvent()
		{
			SetTargetEvent_Null<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[Test]
		public void OnInvoked_ScriptableFloatEvent()
		{
			OnInvoked<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableFloatEvent()
		{
			OnInvoked_ChangeEvent<ScriptableFloatEventListener, ScriptableFloatEvent, float>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableFloatEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableFloatEventListener, ScriptableFloatEvent, float>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableFloatEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableFloatEventListener, ScriptableFloatEvent, float>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableDoubleEvent()
		{
			yield return StartListening_Awake<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableDoubleEvent()
		{
			yield return StartListening_Start<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableDoubleEvent()
		{
			yield return StartListening_OnEnable<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableDoubleEvent()
		{
			yield return StopListening_OnDisable<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableDoubleEvent()
		{
			yield return StopListening_OnDestroy<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableDoubleEvent()
		{
			SetTargetEvent_NewEvent<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableDoubleEvent()
		{
			SetTargetEvent_SameEvent<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableDoubleEvent()
		{
			SetTargetEvent_Null<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[Test]
		public void OnInvoked_ScriptableDoubleEvent()
		{
			OnInvoked<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableDoubleEvent()
		{
			OnInvoked_ChangeEvent<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableDoubleEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableDoubleEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableDoubleEventListener, ScriptableDoubleEvent, double>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableDecimalEvent()
		{
			yield return StartListening_Awake<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableDecimalEvent()
		{
			yield return StartListening_Start<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableDecimalEvent()
		{
			yield return StartListening_OnEnable<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableDecimalEvent()
		{
			yield return StopListening_OnDisable<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableDecimalEvent()
		{
			yield return StopListening_OnDestroy<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableDecimalEvent()
		{
			SetTargetEvent_NewEvent<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableDecimalEvent()
		{
			SetTargetEvent_SameEvent<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableDecimalEvent()
		{
			SetTargetEvent_Null<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[Test]
		public void OnInvoked_ScriptableDecimalEvent()
		{
			OnInvoked<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableDecimalEvent()
		{
			OnInvoked_ChangeEvent<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableDecimalEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>(invokeOn, 1);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableDecimalEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableDecimalEventListener, ScriptableDecimalEvent, decimal>(invokeOn, 1);
		}

		[UnityTest]
		public IEnumerator StartListening_Awake_ScriptableStringEvent()
		{
			yield return StartListening_Awake<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[UnityTest]
		public IEnumerator StartListening_Start_ScriptableStringEvent()
		{
			yield return StartListening_Start<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable_ScriptableStringEvent()
		{
			yield return StartListening_OnEnable<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable_ScriptableStringEvent()
		{
			yield return StopListening_OnDisable<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy_ScriptableStringEvent()
		{
			yield return StopListening_OnDestroy<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[Test]
		public void SetTargetEvent_NewEvent_ScriptableStringEvent()
		{
			SetTargetEvent_NewEvent<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[Test]
		public void SetTargetEvent_SameEvent_ScriptableStringEvent()
		{
			SetTargetEvent_SameEvent<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[Test]
		public void SetTargetEvent_Null_ScriptableStringEvent()
		{
			SetTargetEvent_Null<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[Test]
		public void OnInvoked_ScriptableStringEvent()
		{
			OnInvoked<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[Test]
		public void OnInvoked_ChangeEvent_ScriptableStringEvent()
		{
			OnInvoked_ChangeEvent<ScriptableStringEventListener, ScriptableStringEvent, string>();
		}

		[Test]
		public void OnInvoked_WithArgs_ScriptableStringEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs<ScriptableStringEventListener, ScriptableStringEvent, string>(invokeOn, "1");
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent_ScriptableStringEvent([ValueSource(nameof(invokeEvents))] EventInvokeEvents invokeOn)
		{
			OnInvoked_WithArgs_ChangeEvent<ScriptableStringEventListener, ScriptableStringEvent, string>(invokeOn, "1");
		}

		private IEnumerator StartListening_Awake<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = scriptableEvent;
			listener.StartListening = StartListenEvents.Awake;
			TListener newListener = Instantiate(listener);
			// Destroy the old listener to ensure that only the new listener is listening.
			Destroy(listener);

			yield return null;

			Assert.AreEqual(StartListenEvents.Awake, newListener.StartListening);
			Assert.IsTrue(newListener.IsListening);
			Assert.IsTrue(scriptableEvent.InvokedHasSubscribers);
		}

		public IEnumerator StartListening_Start<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = scriptableEvent;
			listener.StartListening = StartListenEvents.Start;

			TListener newListener = Instantiate(listener);
			// Destroy the old listener to ensure that only the new listener is listening.
			Destroy(listener);

			yield return null;

			Assert.AreEqual(StartListenEvents.Start, newListener.StartListening);
			Assert.IsTrue(newListener.IsListening);
			Assert.IsTrue(scriptableEvent.InvokedHasSubscribers);
		}

		public IEnumerator StartListening_OnEnable<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = scriptableEvent;
			listener.StartListening = StartListenEvents.OnEnable;
			listener.StopListening = StopListenEvents.OnDisable;

			TListener newListener = Instantiate(listener);
			// Destroy the old listener to ensure that only the new listener is listening.
			Destroy(listener);
			newListener.enabled = false;

			yield return null;

			Assert.AreEqual(StartListenEvents.OnEnable, newListener.StartListening);
			Assert.IsFalse(newListener.IsListening);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);

			newListener.enabled = true;

			yield return null;

			Assert.IsTrue(newListener.IsListening);
			Assert.IsTrue(scriptableEvent.InvokedHasSubscribers);
		}

		public IEnumerator StopListening_OnDisable<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = scriptableEvent;
			listener.StopListening = StopListenEvents.OnDisable;
			listener.enabled = false;

			yield return null;

			Assert.AreEqual(StopListenEvents.OnDisable, listener.StopListening);
			Assert.IsFalse(listener.IsListening);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
		}

		public IEnumerator StopListening_OnDestroy<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = scriptableEvent;
			listener.StopListening = StopListenEvents.OnDestroy;
			Destroy(listener);

			yield return null;

			Assert.AreEqual(StopListenEvents.OnDestroy, listener.StopListening);
			Assert.IsFalse(listener.IsListening);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
		}

		public void SetTargetEvent_NewEvent<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = scriptableEvent;
			TEvent newEvent = CreateInstance<TEvent>();
			listener.TargetEvent = newEvent;

			Assert.AreEqual(newEvent, listener.TargetEvent);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
			Assert.IsTrue(newEvent.InvokedHasSubscribers);
		}

		public void SetTargetEvent_SameEvent<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = scriptableEvent;

			Assert.AreEqual(scriptableEvent, listener.TargetEvent);
			Assert.IsTrue(scriptableEvent.InvokedHasSubscribers);
		}

		public void SetTargetEvent_Null<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = null;

			Assert.IsNull(listener.TargetEvent);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
		}

		public void OnInvoked<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();
			listener.TargetEvent = scriptableEvent;

			bool invoked = false;

			listener.OnInvoked.AddListener(i => invoked = true);

			scriptableEvent.Invoke(this);

			Assert.IsTrue(invoked);
		}

		public void OnInvoked_ChangeEvent<TListener, TEvent, TValue>() where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();
			listener.TargetEvent = scriptableEvent;

			bool invoked = false;

			listener.OnInvoked.AddListener(i => invoked = true);

			scriptableEvent.Invoke(this);

			Assert.IsTrue(invoked);

			invoked = false;

			TEvent newEvent = CreateInstance<TEvent>();
			listener.TargetEvent = newEvent;

			newEvent.Invoke(this);

			Assert.IsTrue(invoked);
		}

		public void OnInvoked_WithArgs<TListener, TEvent, TValue>(EventInvokeEvents invokeOn, TValue args) where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();
			listener.TargetEvent = scriptableEvent;
			listener.InvokeOn = invokeOn;

			Assert.AreEqual(invokeOn, listener.InvokeOn);

			switch (invokeOn)
			{
				case EventInvokeEvents.FromValue:
					listener.FromValue = default;
					Assert.AreEqual(default, listener.FromValue);
					break;
				case EventInvokeEvents.ToValue:
					listener.ToValue = args;
					Assert.AreEqual(args, listener.ToValue);
					break;
			}

			bool invoked = false;

			listener.OnInvoked.AddListener(a =>
			{
				invoked = true;
				Assert.AreEqual(args, a);
			});

			scriptableEvent.Invoke(this, args);

			Assert.IsTrue(invoked);

			// If it isn't any, make sure the invoke event isn't invoked again.
			if (invokeOn != EventInvokeEvents.Any)
			{
				invoked = false;

				switch (invokeOn)
				{
					case EventInvokeEvents.FromValue:
						scriptableEvent.Invoke(this, args);
						break;
					case EventInvokeEvents.ToValue:
						scriptableEvent.Invoke(this, default);
						break;
				}

				Assert.IsFalse(invoked);
			}
		}

		public void OnInvoked_WithArgs_ChangeEvent<TListener, TEvent, TValue>(EventInvokeEvents invokeOn, TValue args) where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();
			listener.TargetEvent = scriptableEvent;
			listener.InvokeOn = invokeOn;

			Assert.AreEqual(invokeOn, listener.InvokeOn);

			switch (invokeOn)
			{
				case EventInvokeEvents.FromValue:
					listener.FromValue = default;
					Assert.AreEqual(default, listener.FromValue);
					break;
				case EventInvokeEvents.ToValue:
					listener.ToValue = args;
					Assert.AreEqual(args, listener.ToValue);
					break;
			}

			bool invoked = false;

			listener.OnInvoked.AddListener(a =>
			{
				Assert.AreEqual(args, a);
				invoked = true;
			});

			scriptableEvent.Invoke(this, args);

			Assert.IsTrue(invoked);

			invoked = false;

			TEvent newEvent = CreateInstance<TEvent>();
			listener.TargetEvent = newEvent;

			newEvent.Invoke(this, args);

			Assert.IsTrue(invoked);

			// If it isn't any, make sure the invoke event isn't invoked again.
			if (invokeOn != EventInvokeEvents.Any)
			{
				invoked = false;

				switch (invokeOn)
				{
					case EventInvokeEvents.FromValue:
						scriptableEvent.Invoke(this, args);
						break;
					case EventInvokeEvents.ToValue:
						scriptableEvent.Invoke(this, default);
						break;
				}

				Assert.IsFalse(invoked);
			}
		}
	}
}