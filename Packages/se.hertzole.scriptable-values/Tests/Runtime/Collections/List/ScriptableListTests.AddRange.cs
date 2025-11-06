#nullable enable

using System;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void AddRange_AddsItems()
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };

            // Act
            list.AddRange(items);

            // Assert
            AssertArraysAreEqual(items, list);
        }

        [Test]
        public void AddRange_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

            // Act
            list.AddRange(items);

            // Assert
            CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(0, args.NewIndex);
            AssertArraysAreEqual(items, args.NewItems.ToArray());

            Assert.AreEqual(0, args.OldItems.Length);
            Assert.AreEqual(-1, args.OldIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void AddRange_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int[] items = { 1, 2, 3, 4, 5 };
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

            // Act
            list.AddRange(items);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(0, args.NewStartingIndex);
            AssertArraysAreEqual(items, args.NewItems.Cast<int>().ToArray());

            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void AddRange_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(list, x => x.AddRange(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void AddRange_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.AddRange(new[] { 1, 2, 3, 4, 5 }), true);
        }

        [Test]
        public void AddRange_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.AddRange(new[] { 1, 2, 3, 4, 5 }), true);
        }

        [Test]
        public void AddRange_Empty_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.AddRange(Array.Empty<int>()));
        }

        [Test]
        public void AddRange_Empty_DoesNotInvokeINotifyCollectionChanged()
        {
            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.AddRange(Array.Empty<int>()));
        }

        [Test]
        public void AddRange_Null_ThrowsArgumentNullException()
        {
            // Act & Assert
            AssertThrows<ArgumentNullException>(() => list.AddRange(null!));
        }
    }
}