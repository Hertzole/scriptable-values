#nullable enable

using System;
using System.Collections.Specialized;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void RemoveAt_RemovesItem()
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);

            // Act
            list.RemoveAt(2);

            // Assert
            Assert.AreEqual(4, list.Count, "The count is not correct.");
            Assert.AreEqual(1, list[0], "The item at index 0 is not correct.");
            Assert.AreEqual(2, list[1], "The item at index 1 is not correct.");
            Assert.AreEqual(4, list[2], "The item at index 2 is not correct.");
            Assert.AreEqual(5, list[3], "The item at index 3 is not correct.");
        }

        [Test]
        public void RemoveAt_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

            // Act
            list.RemoveAt(2);

            // Assert
            CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
            Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
            Assert.AreEqual(3, args.OldItems.Span[0], "The old item is not correct.");
            Assert.AreEqual(2, args.OldIndex, "The old index is not correct.");
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
        }

        [Test]
        public void RemoveAt_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

            // Act
            list.RemoveAt(2);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
            Assert.AreEqual(1, args.OldItems.Count, "The old items length is not correct.");
            Assert.AreEqual(3, args.OldItems[0], "The old item is not correct.");
            Assert.AreEqual(2, args.OldStartingIndex, "The old index is not correct.");
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
        }

        [Test]
        public void RemoveAt_OutsideBounds_ThrowsArgumentOutOfRangeException([Values(-1, 10)] int index)
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);

            // Act & Assert
            AssertThrows<ArgumentOutOfRangeException>(() => list.RemoveAt(index));
        }

        [Test]
        public void RemoveAt_IsReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);
            list.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(list, x => x.RemoveAt(1));
        }

        [Test]
        public void RemoveAt_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, l => l.RemoveAt(2), true);
        }

        [Test]
        public void RemoveAt_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.RemoveAt(2), true);
        }
    }
}