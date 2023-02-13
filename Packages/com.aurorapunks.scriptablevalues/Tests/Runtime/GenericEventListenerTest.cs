using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public abstract class GenericEventListenerTest<TListener, TEvent, TValue> : BaseRuntimeTest where TListener : ScriptableEventListener<TValue> where TEvent : ScriptableEvent<TValue>
	{
		private static TValue[] values;

		public static TValue[] StaticsValue { get { return TestHelper.FindValues(typeof(BaseTest), ref values); } }

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
		public void SetTargetEvent_NewEvent()
		{
			Test_SetTargetEvent_NewEvent();
		}

		[Test]
		public void SetTargetEvent_SameEvent()
		{
			Test_SetTargetEvent_SameEvent();
		}

		[Test]
		public void SetTargetEvent_Null()
		{
			Test_SetTargetEvent_Null();
		}

		[Test]
		public void OnInvoked()
		{
			Test_OnInvoked();
		}

		[Test]
		public void OnInvoked_ChangeEvent()
		{
			Test_OnInvoked_ChangeEvent();
		}

		[Test]
		public void OnInvoked_WithArgs([ValueSource(nameof(eventInvokes))] EventInvokeEvents invokeOn, [ValueSource(nameof(StaticsValue))] TValue value)
		{
			Test_OnInvoked_WithArgs(invokeOn, value);
		}

		[Test]
		public void OnInvoked_WithArgs_ChangeEvent([ValueSource(nameof(eventInvokes))] EventInvokeEvents invokeOn, [ValueSource(nameof(StaticsValue))] TValue value)
		{
			Test_OnInvoked_WithArgs_ChangeEvent(invokeOn, value);
		}

		private IEnumerator Test_StartListening_Awake() 
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

		public IEnumerator Test_StartListening_Start() 
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

		public IEnumerator Test_StartListening_OnEnable()
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

		public IEnumerator Test_StopListening_OnDisable()
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

		public IEnumerator Test_StopListening_OnDestroy()
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

		public void Test_SetTargetEvent_NewEvent() 
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

		public void Test_SetTargetEvent_SameEvent() 
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = scriptableEvent;

			Assert.AreEqual(scriptableEvent, listener.TargetEvent);
			Assert.IsTrue(scriptableEvent.InvokedHasSubscribers);
		}

		public void Test_SetTargetEvent_Null() 
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();

			listener.TargetEvent = null;

			Assert.IsNull(listener.TargetEvent);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
		}

		public void Test_OnInvoked()
		{
			TListener listener = CreateComponent<TListener>();
			TEvent scriptableEvent = CreateInstance<TEvent>();
			listener.TargetEvent = scriptableEvent;

			bool invoked = false;

			listener.OnInvoked.AddListener(i => invoked = true);

			scriptableEvent.Invoke(this);

			Assert.IsTrue(invoked);
		}

		public void Test_OnInvoked_ChangeEvent() 
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

		public void Test_OnInvoked_WithArgs(EventInvokeEvents invokeOn, TValue args) 
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
				if (EqualityComparer<TValue>.Default.Equals(default, args))
				{
					return;
				}
				
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

		public void Test_OnInvoked_WithArgs_ChangeEvent(EventInvokeEvents invokeOn, TValue args) 
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