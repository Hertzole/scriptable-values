#if SCRIPTABLE_VALUES_PROPERTIES
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptablePoolTests
	{
		public static readonly string[] bannedProperties =
		{
			nameof(ScriptablePool<object>.OnCreateObject),
			nameof(ScriptablePool<object>.OnGetObject),
			nameof(ScriptablePool<object>.OnDestroyObject),
			nameof(ScriptablePool<object>.OnReturnObject)
		};

		public static readonly string[] requiredProperties =
		{
			nameof(ScriptablePool<object>.CountAll),
			nameof(ScriptablePool<object>.CountActive),
			nameof(ScriptablePool<object>.CountInactive)
		};

		[Test]
		public void HasProperty([ValueSource(nameof(requiredProperties))] string property)
		{
			AssertHasProperty<TestScriptableObjectPool>(property);
		}

		[Test]
		public void DoesNotHaveProperty([ValueSource(nameof(bannedProperties))] string property)
		{
			AssertDoesNotHaveProperty<TestScriptableObjectPool>(property);
		}
	}
}
#endif