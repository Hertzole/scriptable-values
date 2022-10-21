using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class GenericEventListenerTests : BaseTest
	{
		private static readonly EventInvokeEvents[] invokeEvents = new[]
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