#if SCRIPTABLE_VALUES_PROPERTIES
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptablePoolTests<TType, TValue>
    {
        public static readonly string[] bannedProperties =
        {
            nameof(ScriptablePool<TType>.activeObjects),
            nameof(ScriptablePool<TType>.pool),
            nameof(ScriptablePool<TType>.OnPoolChanged)
        };

        public static readonly string[] requiredProperties =
        {
            nameof(ScriptablePool.CountAll),
            nameof(ScriptablePool.CountActive),
            nameof(ScriptablePool.CountInactive)
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