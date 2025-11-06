#if SCRIPTABLE_VALUES_PROPERTIES
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        public static readonly string[] bannedProperties =
        {
            nameof(ScriptableList<object>.isReadOnly),
            nameof(ScriptableList<object>.setEqualityCheck),
            nameof(ScriptableList<object>.clearOnStart),
            nameof(ScriptableList<object>.list),
            nameof(ScriptableList<object>.OnCollectionChanged)
        };

        public static readonly string[] requiredProperties =
        {
            nameof(ScriptableList<object>.Capacity),
            nameof(ScriptableList<object>.SetEqualityCheck),
            nameof(ScriptableList<object>.ClearOnStart),
            nameof(ScriptableList<object>.IsReadOnly),
            nameof(ScriptableList<object>.Count)
        };

        [Test]
        public void HasProperty([ValueSource(nameof(requiredProperties))] string property)
        {
            AssertHasProperty<TestScriptableList>(property);
        }

        [Test]
        public void DoesNotHaveProperty([ValueSource(nameof(bannedProperties))] string property)
        {
            AssertDoesNotHaveProperty<TestScriptableList>(property);
        }
    }
}
#endif