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
            nameof(ScriptableDictionary<object, object>.OnCollectionChanged)
        };

        public static readonly string[] requiredProperties =
        {
            nameof(ScriptableDictionary.SetEqualityCheck),
            nameof(ScriptableDictionary.ClearOnStart),
            nameof(ScriptableDictionary.IsReadOnly),
            nameof(ScriptableDictionary.Count)
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