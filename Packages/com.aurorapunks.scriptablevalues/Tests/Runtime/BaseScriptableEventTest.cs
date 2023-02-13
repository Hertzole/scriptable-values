using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public abstract class BaseScriptableEventTest<TType> : BaseRuntimeTest where TType : ScriptableEvent
	{
		[Test]
		public void Invoke()
		{
			InvokeWithoutSender();
		}

		[Test]
		public void Invoke_WithSender()
		{
			InvokeWithSender();
		}

		protected void InvokeWithoutSender()
		{
			TType instance = CreateInstance<TType>();
			instance.ResetValues();

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) => { eventInvoked = true; };

			instance.Invoke();

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}

		protected void InvokeWithSender()
		{
			TType instance = CreateInstance<TType>();
			instance.ResetValues();

			GameObject sender = CreateGameObject("sender");

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) =>
			{
				Assert.AreEqual(sender, eventSender);

				Debug.Log("On invoked");

				eventInvoked = true;
			};

			instance.Invoke(sender);

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}
	}
}