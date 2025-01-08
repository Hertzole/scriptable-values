using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	public class ScriptableEventTest<TType, TValue> : BaseScriptableEventTest<TType> where TType : ScriptableEvent<TValue>
	{
		private static TValue[] values;

		public static TValue[] StaticsValue { get { return TestHelper.FindValues(typeof(BaseTest), ref values); } }

		[Test]
		public void Invoke_WithoutSender()
		{
			TType instance = CreateInstance<TType>();

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) => { eventInvoked = true; };

			instance.Invoke();

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}
		
		[Test]
		public void Invoke_WithSenderAndArg([ValueSource(nameof(StaticsValue))] TValue value)
		{
			InvokeWithArgAndSender(value);
		}
		
		[Test]
		public void Invoke_WithArgOnly([ValueSource(nameof(StaticsValue))] TValue value)
		{
			InvokeWithArgsOnly(value);
		}

		private void InvokeWithArgAndSender(TValue args) 
		{
			TType instance = CreateInstance<TType>();

			GameObject sender = CreateGameObject("sender");

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) =>
			{
				Assert.AreEqual(args, eventArgs);
				Assert.AreEqual(sender, eventSender);

				eventInvoked = true;
			};

			instance.Invoke(sender, args);

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}

		private void InvokeWithArgsOnly(TValue args)
		{
			TType instance = CreateInstance<TType>();

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) =>
			{
				Assert.AreEqual(args, eventArgs);
				Assert.AreEqual(instance, eventSender);

				eventInvoked = true;
			};

			instance.Invoke(args);

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}
	}
}