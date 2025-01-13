using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class BaseScriptableEventTest<TType>
	{
		public static readonly string[] bannedProperties =
		{
			nameof(ScriptableEvent.onInvoked),
			nameof(ScriptableEvent.OnInvoked),
			nameof(ScriptableEvent.InvokedHasSubscribers),
			nameof(ScriptableEvent<TType>.onInvokedWithArgs),
			nameof(ScriptableEvent<TType>.editorInvokeValue),
			nameof(ScriptableEvent<TType>.InvokedHasSubscribers),
			nameof(ScriptableEvent<TType>.OnInvoked),
#if UNITY_EDITOR
			nameof(RuntimeScriptableObject.CollectStackTraces),
			nameof(RuntimeScriptableObject.CollectStackTraces)
#endif
		};

		public static readonly string[] requiredProperties =
		{
			nameof(ScriptableEvent<TType>.PreviousArgs)
		};

		[Test]
		public void HasProperty([ValueSource(nameof(requiredProperties))] string property)
		{
			if (typeof(TType) == typeof(ScriptableEvent))
			{
				Assert.Pass("ScriptableEvent does not have properties.");
				return;
			}
			
			AssertHasProperty<TType>(property);
		}

		[Test]
		public void DoesNotHaveProperty([ValueSource(nameof(bannedProperties))] string property)
		{
			AssertDoesNotHaveProperty<TType>(property);
		}
	}
}