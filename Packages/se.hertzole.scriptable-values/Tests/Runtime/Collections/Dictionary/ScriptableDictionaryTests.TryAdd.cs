#nullable enable

using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableDictionaryTests
    {
        [Test]
        public void TryAdd_Success_AddsItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();

            // Act
            bool result = dictionary.TryAdd(key, value);

            // Assert
            Assert.IsTrue(result, "The item was not added.");
            Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
            Assert.AreEqual(value, dictionary[key], "The value is not correct.");
        }

        [Test]
        public void TryAdd_Success_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            // Act
            dictionary.TryAdd(key, value);

            // Assert
            CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(1, args.NewItems.Length);
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems.Span[0]);
            Assert.AreEqual(-1, args.NewIndex);

            Assert.AreEqual(0, args.OldItems.Length);
            Assert.AreEqual(-1, args.OldIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void TryAdd_Success_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            // Act
            dictionary.TryAdd(key, value);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(1, args.NewItems.Count);
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems[0]);
            Assert.AreEqual(-1, args.NewStartingIndex);

            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void TryAdd_Fail_DoesNotAddItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.Add(key, value);

            // Act
            bool result = dictionary.TryAdd(key, value);

            // Assert
            Assert.IsFalse(result, "The item was added.");
            Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
            Assert.AreEqual(value, dictionary[key], "The value is not correct.");
        }

        [Test]
        public void TryAdd_Fail_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.Add(key, value);

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => x.TryAdd(key, value));
        }

        [Test]
        public void TryAdd_Fail_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.Add(key, value);

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => x.TryAdd(key, value));
        }

        [Test]
        public void TryAdd_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            dictionary.IsReadOnly = true;
            int key = GetRandomNumber();
            int value = GetRandomNumber();

            // Act & Assert
            AssertThrowsReadOnlyException(dictionary, x => x.TryAdd(key, value));
        }

        [Test]
        public void TryAdd_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            dictionary.IsReadOnly = true;
            int key = GetRandomNumber();
            int value = GetRandomNumber();

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => x.TryAdd(key, value), true);
        }

        [Test]
        public void TryAdd_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            dictionary.IsReadOnly = true;
            int key = GetRandomNumber();
            int value = GetRandomNumber();

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => x.TryAdd(key, value), true);
        }
    }
}