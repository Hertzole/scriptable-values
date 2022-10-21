using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class EventListenerTests : BaseTest
	{
		private ScriptableEventListener listener;
		private ScriptableEvent scriptableEvent;

		protected override void OnSetup()
		{
			listener = CreateComponent<ScriptableEventListener>();
			scriptableEvent = CreateInstance<ScriptableEvent>();
			listener.TargetEvent = scriptableEvent;
		}

		[UnityTest]
		public IEnumerator StartListening_Awake()
		{
			listener.StartListening = StartListenEvents.Awake;
			ScriptableEventListener newListener = Instantiate(listener);
			// Destroy the old listener to ensure that only the new listener is listening.
			Destroy(listener);

			yield return null;

			Assert.AreEqual(StartListenEvents.Awake, newListener.StartListening);
			Assert.IsTrue(newListener.IsListening);
			Assert.IsTrue(scriptableEvent.InvokedHasSubscribers);
		}

		[UnityTest]
		public IEnumerator StartListening_Start()
		{
			listener.StartListening = StartListenEvents.Start;

			ScriptableEventListener newListener = Instantiate(listener);
			// Destroy the old listener to ensure that only the new listener is listening.
			Destroy(listener);

			yield return null;

			Assert.AreEqual(StartListenEvents.Start, newListener.StartListening);
			Assert.IsTrue(newListener.IsListening);
			Assert.IsTrue(scriptableEvent.InvokedHasSubscribers);
		}

		[UnityTest]
		public IEnumerator StartListening_OnEnable()
		{
			listener.StartListening = StartListenEvents.OnEnable;
			listener.StopListening = StopListenEvents.OnDisable;

			ScriptableEventListener newListener = Instantiate(listener);
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

		[UnityTest]
		public IEnumerator StopListening_OnDisable()
		{
			listener.StopListening = StopListenEvents.OnDisable;
			listener.enabled = false;

			yield return null;

			Assert.IsFalse(listener.IsListening);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
		}

		[UnityTest]
		public IEnumerator StopListening_OnDestroy()
		{
			listener.StopListening = StopListenEvents.OnDestroy;
			Destroy(listener);

			yield return null;

			Assert.IsFalse(listener.IsListening);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
		}

		[Test]
		public void SetTargetEvent_NewEvent()
		{
			ScriptableEvent newEvent = CreateInstance<ScriptableEvent>();
			listener.TargetEvent = newEvent;

			Assert.AreEqual(newEvent, listener.TargetEvent);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
			Assert.IsTrue(newEvent.InvokedHasSubscribers);
		}

		[Test]
		public void SetTargetEvent_SameEvent()
		{
			listener.TargetEvent = scriptableEvent;

			Assert.AreEqual(scriptableEvent, listener.TargetEvent);
			Assert.IsTrue(scriptableEvent.InvokedHasSubscribers);
		}

		[Test]
		public void SetTargetEvent_Null()
		{
			listener.TargetEvent = null;

			Assert.IsNull(listener.TargetEvent);
			Assert.IsFalse(scriptableEvent.InvokedHasSubscribers);
		}

		[Test]
		public void OnInvoked()
		{
			bool invoked = false;

			listener.OnInvoked.AddListener(() => invoked = true);

			scriptableEvent.Invoke(this);

			Assert.IsTrue(invoked);
		}
		
		[Test]
		public void OnInvoked_ChangeEvent()
		{
			bool invoked = false;

			listener.OnInvoked.AddListener(() => invoked = true);

			scriptableEvent.Invoke(this);

			Assert.IsTrue(invoked);

			invoked = false;

			ScriptableEvent newEvent = CreateInstance<ScriptableEvent>();
			listener.TargetEvent = newEvent;

			newEvent.Invoke(this);

			Assert.IsTrue(invoked);
		}
	}
}