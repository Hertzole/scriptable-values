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