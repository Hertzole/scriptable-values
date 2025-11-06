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
        public void Reverse_ReversesList()
        {
            // Arrange
            list.Add(1);
            list.Add(2);
            list.Add(3);

            // Act
            list.Reverse();

            // Assert
            Assert.AreEqual(3, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(1, list[2]);
        }

        [Test]
        public void Reverse_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int[] items = { 1, 2, 3 };
            list.AddRange(items);
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

            // Act
            list.Reverse();

            // Assert
            int[] expected = { 3, 2, 1 };
            CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
            Assert.AreEqual(3, args.NewItems.Length, "The new items length is not correct.");
            Assert.AreEqual(3, args.OldItems.Length, "The old items length is not correct.");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], list[i], $"The item at index {i} is not correct.");
                Assert.AreEqual(expected[i], args.NewItems.Span[i]);
            }

            for (int i = 0; i < items.Length; i++)
            {
                Assert.AreEqual(items[i], args.OldItems.Span[i]);
            }
        }

        [Test]
        public void Reverse_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int[] items = { 1, 2, 3 };
            list.AddRange(items);
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

            // Act
            list.Reverse();

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
            Assert.AreEqual(3, args.NewItems.Count, "The new items length is not correct.");
            Assert.AreEqual(3, args.OldItems.Count, "The old items length is not correct.");

            int[] expected = { 3, 2, 1 };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], list[i], $"The item at index {i} is not correct.");
            }

            for (int i = 0; i < items.Length; i++)
            {
                Assert.AreEqual(items[i], args.OldItems[i]);
            }

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], args.NewItems[i]);
            }
        }

        [Test]
        public void Reverse_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3 });
            list.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(list, x => x.Reverse());
        }

        [Test]
        public void Reverse_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3 });
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.Reverse(), true);
        }

        [Test]
        public void Reverse_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3 });
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.Reverse(), true);
        }

        [Test]
        public void Reverse_EmptyList_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.Reverse());
        }

        [Test]
        public void Reverse_EmptyList_DoesNotInvokeINotifyCollectionChanged()
        {
            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.Reverse());
        }

        [Test]
        public void ReverseRange_ReversesItems()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, 4, 5 });

            // Act
            list.Reverse(1, 3);

            // Assert
            int[] expected = { 1, 4, 3, 2, 5 };
            AssertArraysAreEqual(expected, list);
        }

        [Test]
        public void ReverseRange_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

            // Act
            list.Reverse(1, 3);

            // Assert
            int[] expected = { 1, 4, 3, 2, 5 };
            CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
            Assert.AreEqual(3, args.NewItems.Length, "The new items length is not correct.");
            Assert.AreEqual(3, args.OldItems.Length, "The old items length is not correct.");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], list[i], $"The item at index {i} is not correct.");
            }

            // Check if the slice is correct.
            for (int i = 0; i < args.NewItems.Length; i++)
            {
                Assert.AreEqual(expected[i + 1], args.NewItems.Span[i]);
            }

            for (int i = 0; i < args.OldItems.Length; i++)
            {
                Assert.AreEqual(items[i + 1], args.OldItems.Span[i]);
            }
        }

        [Test]
        public void ReverseRange_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            list.AddRange(items);
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

            // Act
            list.Reverse(1, 3);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
            Assert.AreEqual(3, args.NewItems.Count, "The new items length is not correct.");
            Assert.AreEqual(3, args.OldItems.Count, "The old items length is not correct.");

            int[] expected = { 1, 4, 3, 2, 5 };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], list[i], $"The item at index {i} is not correct.");
            }

            for (int i = 0; i < args.NewItems.Count; i++)
            {
                Assert.AreEqual(expected[i + 1], args.NewItems[i]);
            }

            for (int i = 0; i < args.OldItems.Count; i++)
            {
                Assert.AreEqual(items[i + 1], args.OldItems[i]);
            }
        }

        [Test]
        public void ReverseRange_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3 });
            list.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(list, x => x.Reverse(0, 3));
        }

        [Test]
        public void ReverseRange_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3 });
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.Reverse(0, 3), true);
        }

        [Test]
        public void ReverseRange_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3 });
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.Reverse(0, 3), true);
        }

        [Test]
        public void ReverseRange_InvalidIndex_ThrowsArgumentOutOfRangeException([Values(-1, 3)] int index)
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3 });

            // Act & Assert
            AssertThrows<ArgumentOutOfRangeException>(() => list.Reverse(index, 1));
        }

        [Test]
        public void ReverseRange_InvalidCount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3 });

            // Act & Assert
            AssertThrows<ArgumentOutOfRangeException>(() => list.Reverse(0, 4));
        }

        [Test]
        public void ReverseRange_EmptyList_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.Reverse(0, 0));
        }

        [Test]
        public void ReverseRange_EmptyList_DoesNotInvokeINotifyCollectionChanged()
        {
            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.Reverse(0, 0));
        }
    }
}