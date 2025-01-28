using System;
using NUnit.Framework;
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

		[Test]
		public void Invoke_RegisterInvokeListener()
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			int invokeCount = 0;
			instance.RegisterInvokedListener(OnInvoked);

			// Act
			instance.Invoke();

			// Assert
			Assert.AreEqual(1, invokeCount, "Invoke count should be 1. The invoke listener has not been called.");

			// Arrange removal
			instance.UnregisterInvokedListener(OnInvoked);

			// Act
			instance.Invoke();

			// Assert
			Assert.AreEqual(1, invokeCount, "Invoke count should still be 1. The invoke listener has not been removed properly.");
			return;

			void OnInvoked(object sender, EventArgs e)
			{
				invokeCount++;
			}
		}

		[Test]
		public void Invoke_RegisterInvokeListener_WithContext()
		{
			// Arrange
			TType instance = CreateInstance<TType>();
			Context context = new Context();
			instance.RegisterInvokedListener(OnInvoked, context);

			// Act
			instance.Invoke();

			// Assert
			Assert.AreEqual(1, context.invokeCount, "Invoke count should be 1. The invoke listener has not been called.");

			// Arrange removal
			instance.UnregisterInvokedListener<Context>(OnInvoked);

			// Act
			instance.Invoke();

			// Assert
			Assert.AreEqual(1, context.invokeCount, "Invoke count should still be 1. The invoke listener has not been removed properly.");
			return;

			static void OnInvoked(object sender, EventArgs e, Context context)
			{
				context.invokeCount++;
			}
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

		private class Context
		{
			public int invokeCount = 0;
		}
	}
}