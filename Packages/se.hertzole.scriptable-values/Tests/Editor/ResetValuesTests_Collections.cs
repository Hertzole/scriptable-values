using System.Collections.Generic;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests.Editor
{
    public partial class ResetValuesTests
    {
        [Test]
        public void Dictionary_ResetEvents([Values] EventType eventType)
        {
            // Arrange
            TestScriptableDictionary instance = CreateInstance<TestScriptableDictionary>();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(instance, eventType, instance);

            // Act
            ((IScriptableValueCallbacks) instance).OnScriptableObjectPreEnable();
            ((IScriptableValueCallbacks) instance).OnScriptableObjectEnable();

            instance.Add(0, 1);
            instance.Remove(0);
            instance.Clear();
            instance[0] = 1;

            // Assert
            Assert.IsFalse(tracker.HasBeenInvoked());
        }

        [Test]
        public void Dictionary_ResetValues([ValueSource(nameof(bools))] bool isReadOnly, [ValueSource(nameof(bools))] bool clearOnStart)
        {
            TestScriptableDictionary instance = CreateInstance<TestScriptableDictionary>();

            instance.Add(0, 1);
            instance.Add(2, 3);
            instance.Add(4, 5);

            instance.IsReadOnly = isReadOnly;
            instance.ClearOnStart = clearOnStart;

            ((IScriptableValueCallbacks) instance).OnScriptableObjectPreEnable();
            ((IScriptableValueCallbacks) instance).OnScriptableObjectEnable();

            if (!isReadOnly && clearOnStart)
            {
                Assert.AreEqual(0, instance.Count);
                Assert.AreEqual(0, instance.dictionary.Count);
                Assert.AreEqual(0, instance.values.Count);
                Assert.AreEqual(0, instance.keys.Count);
            }
            else
            {
                Assert.AreEqual(3, instance.Count);
                Assert.AreEqual(3, instance.dictionary.Count);
                Assert.AreEqual(3, instance.values.Count);
                Assert.AreEqual(3, instance.keys.Count);
            }
        }

        [Test]
        public void List_ResetEvents([Values] EventType eventType)
        {
            // Arrange
            TestScriptableList instance = CreateInstance<TestScriptableList>();
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(instance, eventType, instance);

            // Act
            ((IScriptableValueCallbacks) instance).OnScriptableObjectPreEnable();
            ((IScriptableValueCallbacks) instance).OnScriptableObjectEnable();

            instance.Add(0);
            instance[0] = 1;
            instance.Clear();
            instance.Insert(0, 42);
            instance.RemoveAt(0);

            Assert.IsFalse(tracker.HasBeenInvoked());
        }

        [Test]
        public void List_ResetValues([ValueSource(nameof(bools))] bool isReadOnly, [ValueSource(nameof(bools))] bool clearOnStart)
        {
            TestScriptableList instance = CreateInstance<TestScriptableList>();

            instance.Add(0);
            instance.Add(1);
            instance.Add(2);

            instance.IsReadOnly = isReadOnly;
            instance.ClearOnStart = clearOnStart;

            ((IScriptableValueCallbacks) instance).OnScriptableObjectPreEnable();
            ((IScriptableValueCallbacks) instance).OnScriptableObjectEnable();

            if (!isReadOnly && clearOnStart)
            {
                Assert.AreEqual(0, instance.Count);
                Assert.AreEqual(0, instance.list.Count);
            }
            else
            {
                Assert.AreEqual(3, instance.Count);
                Assert.AreEqual(3, instance.list.Count);
            }
        }
    }
}