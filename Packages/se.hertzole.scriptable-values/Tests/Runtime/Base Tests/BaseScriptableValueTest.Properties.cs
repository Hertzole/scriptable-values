#if SCRIPTABLE_VALUES_PROPERTIES
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class BaseScriptableValueTest<TType, TValue>
	{
		public static readonly string[] bannedProperties =
		{
			nameof(ScriptableValue.isReadOnly),
			nameof(ScriptableValue.resetValueOnStart),
			nameof(ScriptableValue.setEqualityCheck),
			nameof(ScriptableValue<TType>.value),
			nameof(ScriptableValue<TType>.defaultValue),
			nameof(ScriptableValue<TType>.onValueChanging),
			nameof(ScriptableValue<TType>.onValueChanged),
			nameof(ScriptableValue<TType>.OnValueChanging),
			nameof(ScriptableValue<TType>.OnValueChanged),
			nameof(ScriptableValue<TType>.ValueChangingHasSubscribers),
			nameof(ScriptableValue<TType>.ValueChangedHasSubscribers)
		};

		public static readonly string[] requiredProperties =
		{
			nameof(ScriptableValue.IsReadOnly),
			nameof(ScriptableValue.ResetValueOnStart),
			nameof(ScriptableValue.SetEqualityCheck),
			nameof(ScriptableValue<TType>.Value),
			nameof(ScriptableValue<TType>.PreviousValue),
			nameof(ScriptableValue<TType>.DefaultValue)
		};

		[Test]
		public void HasProperty([ValueSource(nameof(requiredProperties))] string property)
		{
			AssertHasProperty<TType>(property);
		}

		[Test]
		public void DoesNotHaveProperty([ValueSource(nameof(bannedProperties))] string property)
		{
			AssertDoesNotHaveProperty<TType>(property);
		}
	}
}
#endif