using System;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests.Events
{
	public class ScriptableEventTests : BaseScriptableEventTest<ScriptableEvent>
	{
		[Test]
		public void Invoke_NoSender([Values] EventType eventType)
		{
			// Arrange
			ScriptableEvent instance = CreateInstance<ScriptableEvent>();
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("sender", instance);

			switch (eventType)
			{
				case EventType.Event:
					instance.OnInvoked += InstanceOnInvoked;
					break;
				case EventType.Register:
					instance.RegisterInvokedListener(InstanceOnInvoked);
					break;
				case EventType.RegisterWithContext:
					instance.RegisterInvokedListener(StaticOnInvoked, context);
					break;
			}

			// Act
			instance.Invoke();

			// Assert
			Assert.AreEqual(1, context.invokeCount, "Invoke count should be 1.");

			// Arrange removal
			switch (eventType)
			{
				case EventType.Event:
					instance.OnInvoked -= InstanceOnInvoked;
					break;
				case EventType.Register:
					instance.UnregisterInvokedListener(InstanceOnInvoked);
					break;
				case EventType.RegisterWithContext:
					instance.UnregisterInvokedListener<InvokeCountContext>(StaticOnInvoked);
					break;
			}

			// Act
			instance.Invoke();

			// Assert
			Assert.AreEqual(1, context.invokeCount, "Invoke count should still be 1.");
			return;

			void InstanceOnInvoked(object sender, EventArgs e)
			{
				context.invokeCount++;
				Assert.AreEqual(sender, instance);
			}

			static void StaticOnInvoked(object sender, EventArgs e, InvokeCountContext context)
			{
				context.invokeCount++;
				Assert.AreEqual(sender, context.GetArg<ScriptableEvent>("sender"));
			}
		}
		
		[Test]
		public void Invoke_WithSender([Values] EventType eventType)
		{
			// Arrange
			ScriptableEvent instance = CreateInstance<ScriptableEvent>();
			GameObject sender = new GameObject();
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("sender", sender);

			switch (eventType)
			{
				case EventType.Event:
					instance.OnInvoked += InstanceOnInvoked;
					break;
				case EventType.Register:
					instance.RegisterInvokedListener(InstanceOnInvoked);
					break;
				case EventType.RegisterWithContext:
					instance.RegisterInvokedListener(StaticOnInvoked, context);
					break;
			}
			
			// Act
			instance.Invoke(sender);
			
			// Assert
			Assert.AreEqual(1, context.invokeCount, "Invoke count should be 1.");
			
			// Arrange removal
			switch (eventType)
			{
				case EventType.Event:
					instance.OnInvoked -= InstanceOnInvoked;
					break;
				case EventType.Register:
					instance.UnregisterInvokedListener(InstanceOnInvoked);
					break;
				case EventType.RegisterWithContext:
					instance.UnregisterInvokedListener<InvokeCountContext>(StaticOnInvoked);
					break;
			}
			
			// Act
			instance.Invoke(sender);
			
			// Assert
			Assert.AreEqual(1, context.invokeCount, "Invoke count should still be 1.");
			return;

			void InstanceOnInvoked(object o, EventArgs e)
			{
				Assert.AreEqual(o, sender);
				context.invokeCount++;
			}
			
			static void StaticOnInvoked(object o, EventArgs e, InvokeCountContext c)
			{
				c.invokeCount++;
				Assert.AreEqual(o, c.GetArg<GameObject>("sender"));
			}
		}
	}
}