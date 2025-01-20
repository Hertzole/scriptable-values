#if SCRIPTABLE_VALUES_PROPERTIES
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableDictionaryTests
	{
		public static readonly string[] bannedProperties =
		{
			nameof(ScriptableDictionary<object, object>.isReadOnly),
			nameof(ScriptableDictionary<object, object>.setEqualityCheck),
			nameof(ScriptableDictionary<object, object>.clearOnStart),
			nameof(ScriptableDictionary<object, object>.keys),
			nameof(ScriptableDictionary<object, object>.values),
			nameof(ScriptableDictionary<object, object>.OnAdded),
			nameof(ScriptableDictionary<object, object>.OnSet),
			nameof(ScriptableDictionary<object, object>.OnRemoved),
			nameof(ScriptableDictionary<object, object>.OnCleared),
			nameof(ScriptableDictionary<object, object>.OnChanged)
		};

		public static readonly string[] requiredProperties =
		{
			nameof(ScriptableDictionary<object, object>.SetEqualityCheck),
			nameof(ScriptableDictionary<object, object>.ClearOnStart),
			nameof(ScriptableDictionary<object, object>.IsReadOnly),
			nameof(ScriptableDictionary<object, object>.Count)
		};

		[Test]
		public void HasProperty([ValueSource(nameof(requiredProperties))] string property)
		{
			AssertHasProperty<TestScriptableDictionary>(property);
		}

		[Test]
		public void DoesNotHaveProperty([ValueSource(nameof(bannedProperties))] string property)
		{
			AssertDoesNotHaveProperty<TestScriptableDictionary>(property);
		}
	}
}
#endif