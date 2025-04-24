using System;
using System.Collections;
using System.ComponentModel;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	public partial class ScriptableEventTest<TType, TValue> : BaseScriptableEventTest<TType> where TType : ScriptableEvent<TValue>
	{
		private static TValue[] values;

		public static TValue[] StaticsValue
		{
			get { return TestHelper.FindValues(typeof(BaseTest), ref values); }
		}

		[Test]
		public void Invoke_WithSenderAndArg([ValueSource(nameof(StaticsValue))] TValue value, [Values] EventType eventType)
		{
			// Arrange
			var instance = CreateInstance<TType>();
			GameObject sender = CreateGameObject("sender");
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("sender", sender);
			context.AddArg("value", value);

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
			instance.Invoke(sender, value);

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
			instance.Invoke(sender, value);

			// Assert
			Assert.AreEqual(1, context.invokeCount, "Invoke count should still be 1.");
			return;

			void InstanceOnInvoked(object o, TValue e)
			{
				Assert.AreEqual(o, sender);
				Assert.AreEqual(e, value);
				context.invokeCount++;
			}

			static void StaticOnInvoked(object o, TValue e, InvokeCountContext c)
			{
				c.invokeCount++;
				Assert.AreEqual(o, c.GetArg<GameObject>("sender"));
				Assert.AreEqual(e, c.GetArg<TValue>("value"));
			}
		}

		[Test]
		public void Invoke_WithArgOnly([ValueSource(nameof(StaticsValue))] TValue value, [Values] EventType eventType)
		{
			// InvokeWithArgsOnly(value);

			// Arrange
			var instance = CreateInstance<TType>();
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("value", value);
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
			instance.Invoke(value);

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
			instance.Invoke(value);

			// Assert
			Assert.AreEqual(1, context.invokeCount, "Invoke count should still be 1.");
			return;

			void InstanceOnInvoked(object o, TValue e)
			{
				Assert.AreEqual(instance, o);
				Assert.AreEqual(e, value);
				context.invokeCount++;
			}

			static void StaticOnInvoked(object o, TValue e, InvokeCountContext c)
			{
				c.invokeCount++;
				Assert.AreEqual(o, c.GetArg<TType>("sender"));
				Assert.AreEqual(e, c.GetArg<TValue>("value"));
			}
		}

		public static IEnumerable PropertyChangeCases
		{
			get
			{
				yield return MakePropertyChangeTestCase<TType>(ScriptableEvent.previousArgsChanging, ScriptableEvent.previousArgsChanged,
					i =>
					{
						i.Invoke(MakeDifferentValue(i.PreviousArgs)); // Invoke twice because the first previous args is probably the default value already.
						i.Invoke(MakeDifferentValue(i.PreviousArgs));
					});
			}
		}

		[Test]
		[TestCaseSource(nameof(PropertyChangeCases))]
		public void InvokesPropertyChangeEvents(PropertyChangingEventArgs changingArgs, PropertyChangedEventArgs changedArgs, Action<TType> setValue)
		{
			AssertPropertyChangesAreInvoked(changingArgs, changedArgs, setValue);
		}

		[Test]
		public void OverrideMethod_OnBeforeInvoked_IsCalled()
		{
			// Arrange
			OverrideScriptableGenericEvent instance = CreateInstance<OverrideScriptableGenericEvent>();
			instance.shouldBlock = false;
			int targetValue = MakeDifferentValue(instance.PreviousArgs);

			// Act
			instance.Invoke(targetValue);

			// Assert
			Assert.IsTrue(instance.calledOnBeforeSetValue, "OnBeforeInvoke was not called.");
			Assert.AreEqual(targetValue, instance.beforeInvokeValue, "The value passed to OnBeforeInvoke was not the same as the one passed to Invoke.");
		}

		[Test]
		public void OverrideMethod_OnBeforeInvoked_Blocked()
		{
			// Arrange
			OverrideScriptableGenericEvent instance = CreateInstance<OverrideScriptableGenericEvent>();
			instance.shouldBlock = true;
			int oldValue = instance.PreviousArgs;
			bool invoked = false;
			instance.OnInvoked += OnInvoked;

			// Act
			instance.Invoke(MakeDifferentValue(instance.PreviousArgs));

			// Assert
			Assert.IsTrue(instance.calledOnBeforeSetValue, "OnBeforeInvoke was not called.");
			Assert.IsFalse(instance.calledOnAfterSetValue, "OnAfterInvoke was called even though it was blocked.");
			Assert.AreEqual(oldValue, instance.PreviousArgs, "The value was changed even though it was blocked.");
			Assert.IsFalse(invoked);

			void OnInvoked(object sender, int e)
			{
				invoked = true;
			}
		}

		[Test]
		public void OverrideMethod_OnAfterInvoked_IsCalled()
		{
			// Arrange
			OverrideScriptableGenericEvent instance = CreateInstance<OverrideScriptableGenericEvent>();
			int targetValue = MakeDifferentValue(instance.PreviousArgs);

			// Act
			instance.Invoke(targetValue);

			// Assert
			Assert.IsTrue(instance.calledOnAfterSetValue, "OnAfterInvoke was not called.");
			Assert.AreEqual(targetValue, instance.afterInvokeValue, "The value passed to OnAfterInvoke was not the same as the one passed to Invoke.");
		}
	}

	public class OverrideScriptableGenericEvent : ScriptableEvent<int>
	{
		public bool shouldBlock = false;

		public bool calledOnBeforeSetValue = false;
		public bool calledOnAfterSetValue = false;

		public int beforeInvokeValue;
		public int afterInvokeValue;

		/// <inheritdoc />
		protected override bool OnBeforeInvoked(object sender, int args)
		{
			calledOnBeforeSetValue = true;
			beforeInvokeValue = args;

			return !shouldBlock;
		}

		/// <inheritdoc />
		protected override void OnAfterInvoked(object sender, int args)
		{
			calledOnAfterSetValue = true;
			afterInvokeValue = args;
		}
	}
}