﻿using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	public abstract partial class BaseScriptableEventTest<TType> : BaseRuntimeTest where TType : ScriptableEvent
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

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) => { eventInvoked = true; };

			instance.Invoke();

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}

		protected void InvokeWithSender()
		{
			TType instance = CreateInstance<TType>();

			GameObject sender = CreateGameObject("sender");

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) =>
			{
				Assert.AreEqual(sender, eventSender);

				eventInvoked = true;
			};

			instance.Invoke(sender);

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}
	}
}