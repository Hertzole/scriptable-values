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
        public void Clear_ClearsDictionary()
        {
            // Arrange
            int key1 = GetRandomNumber();
            int value1 = GetRandomNumber();
            int key2 = GetRandomNumber();
            int value2 = GetRandomNumber();
            dictionary.Add(key1, value1);
            dictionary.Add(key2, value2);

            // Act
            dictionary.Clear();

            // Assert
            Assert.AreEqual(0, dictionary.Count, "The count is not correct.");
        }

        [Test]
        public void Clear_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key1 = GetRandomNumber();
            int value1 = GetRandomNumber();
            int key2 = GetRandomNumber();
            int value2 = GetRandomNumber();
            dictionary.Add(key1, value1);
            dictionary.Add(key2, value2);
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            // Act
            dictionary.Clear();

            // Assert
            CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(0, args.NewItems.Length);
            Assert.AreEqual(-1, args.NewIndex);

            Assert.AreEqual(2, args.OldItems.Length);
            Assert.AreEqual(new KeyValuePair<int, int>(key1, value1), args.OldItems.Span[0]);
            Assert.AreEqual(new KeyValuePair<int, int>(key2, value2), args.OldItems.Span[1]);
            Assert.AreEqual(-1, args.OldIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Reset, args.Action);
        }

        [Test]
        public void Clear_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key1 = GetRandomNumber();
            int value1 = GetRandomNumber();
            int key2 = GetRandomNumber();
            int value2 = GetRandomNumber();
            dictionary.Add(key1, value1);
            dictionary.Add(key2, value2);
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            // Act
            dictionary.Clear();

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.IsNull(args.NewItems, "The new items are not null.");
            Assert.AreEqual(-1, args.NewStartingIndex, "The new starting index is not -1.");

            // Old items are not supported on Reset.
            Assert.IsNull(args.OldItems, "The old items are not null.");
            Assert.AreEqual(-1, args.OldStartingIndex, "The old starting index is not -1.");

            Assert.AreEqual(NotifyCollectionChangedAction.Reset, args.Action, "The action is not correct.");
        }

        [Test]
        public void Clear_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            int key1 = GetRandomNumber();
            int value1 = GetRandomNumber();
            int key2 = GetRandomNumber();
            int value2 = GetRandomNumber();
            dictionary.Add(key1, value1);
            dictionary.Add(key2, value2);

            // Act & Assert
            AssertThrowsReadOnlyException(dictionary, x => x.Clear());
        }

        [Test]
        public void Clear_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key1 = GetRandomNumber();
            int value1 = GetRandomNumber();
            int key2 = GetRandomNumber();
            int value2 = GetRandomNumber();
            dictionary.Add(key1, value1);
            dictionary.Add(key2, value2);
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => x.Clear(), true);
        }

        [Test]
        public void Clear_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            int key1 = GetRandomNumber();
            int value1 = GetRandomNumber();
            int key2 = GetRandomNumber();
            int value2 = GetRandomNumber();
            dictionary.Add(key1, value1);
            dictionary.Add(key2, value2);
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => x.Clear(), true);
        }

        [Test]
        public void Clear_Empty_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => x.Clear());
        }

        [Test]
        public void Clear_Empty_DoesNotInvokeINotifyCollectionChanged()
        {
            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => x.Clear());
        }
    }
}