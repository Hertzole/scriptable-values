using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public abstract class ValueListenerTest<TListener, TType, TValue> : BaseRuntimeTest where TListener : ScriptableValueListener<TValue> where TType : ScriptableValue<TValue>
	{
		private static TValue[] values;

		public static TValue[] StaticsValue
		{
			get
			{
				TestHelper.FindValues(typeof(BaseTest), ref values);
				return values;
			}
		}

		[UnityTest]
		public IEnumerator StartListening_Awake()
		{
			yield return Test_StartListening_Awake();
		}

		[UnityTest]
		public IEnumerator StartListening_Start()
		{
			yield return Test_StartListening_Start();
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable()
		{
			yield return Test_StartListening_OnEnable();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDisable()
		{
			yield return Test_StopListening_OnDisable();
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy()
		{
			yield return Test_StopListening_OnDestroy();
		}

		[Test]
		public void SetTargetValue_NewValue()
		{
			Test_SetTargetValue_NewValue();
		}

		[Test]
		public void SetTargetValue_SameValue()
		{
			Test_SetTargetValue_SameValue();
		}

		[Test]
		public void SetTargetValue_Null()
		{
			Test_SetTargetValue_Null();
		}

		[Test]
		public void SetValue([ValueSource(nameof(invokeParameters))] InvokeParameters parameters, [ValueSource(nameof(valueInvokes))] InvokeEvents invokeOn, [ValueSource(nameof(StaticsValue))] TValue value)
		{
			Test_SetValue(invokeOn, parameters, value);
		}

		private IEnumerator Test_StartListening_Awake() 
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

		public IEnumerator Test_StartListening_Start() 
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

		public IEnumerator Test_StartListening_OnEnable() 
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

		public IEnumerator Test_StopListening_OnDisable() 
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

		public IEnumerator Test_StopListening_OnDestroy() 
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

		public void Test_SetTargetValue_NewValue() 
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

		public void Test_SetTargetValue_SameValue() 
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = scriptableValue;

			Assert.AreEqual(scriptableValue, listener.TargetValue);
			Assert.IsTrue(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsTrue(scriptableValue.ValueChangedHasSubscribers);
		}

		public void Test_SetTargetValue_Null() 
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();

			listener.TargetValue = null;

			Assert.IsNull(listener.TargetValue);
			Assert.IsFalse(scriptableValue.ValueChangingHasSubscribers);
			Assert.IsFalse(scriptableValue.ValueChangedHasSubscribers);
		}

		public void Test_SetValue(InvokeEvents invokeOn, InvokeParameters invokeParameters, TValue newValue)
		{
			TListener listener = CreateComponent<TListener>();
			TType scriptableValue = CreateInstance<TType>();
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