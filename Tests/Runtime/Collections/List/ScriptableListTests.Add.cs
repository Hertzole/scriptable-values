#nullable enable

using System;
using System.Collections;
using System.Collections.Specialized;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void Add_AddsItem()
        {
            // Arrange
            int value = GetRandomNumber();

            // Act
            list.Add(value);

            // Assert
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(value, list[0]);
        }

        [Test]
        public void Add_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int value = GetRandomNumber();
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

            // Act
            list.Add(value);

            // Assert
            CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(1, args.NewItems.Length);
            Assert.AreEqual(value, args.NewItems.Span[0]);
            Assert.AreEqual(0, args.NewIndex);

            Assert.AreEqual(0, args.OldItems.Length);
            Assert.AreEqual(-1, args.OldIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Add_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int value = GetRandomNumber();
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

            // Act
            list.Add(value);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsNotNull(args);
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.IsNotNull(args.NewItems);
            Assert.AreEqual(1, args.NewItems.Count);
            Assert.AreEqual(value, args.NewItems[0]);
            Assert.AreEqual(0, args.NewStartingIndex);

            Assert.IsNull(args.OldItems);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Add_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(list, x => x.Add(GetRandomNumber()));
        }

        [Test]
        public void Add_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.Add(GetRandomNumber()), true);
        }

        [Test]
        public void Add_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.Add(GetRandomNumber()), true);
        }

        [Test]
        public void Add_Object_AddsItem()
        {
            // Arrange
            int value = GetRandomNumber();

            // Act
            int result = ((IList) list).Add(value);

            // Assert
            Assert.AreEqual(0, result); // Make sure the result is the index of the added item, which is 0.
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(value, list[0]);
        }

        [Test]
        public void Add_Object_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int value = GetRandomNumber();
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

            // Act
            ((IList) list).Add(value);

            // Assert
            CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(1, args.NewItems.Length);
            Assert.AreEqual(value, args.NewItems.Span[0]);
            Assert.AreEqual(0, args.NewIndex);

            Assert.AreEqual(0, args.OldItems.Length);
            Assert.AreEqual(-1, args.OldIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Add_Object_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int value = GetRandomNumber();
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

            // Act
            ((IList) list).Add(value);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.IsNotNull(args.NewItems);
            Assert.AreEqual(1, args.NewItems.Count);
            Assert.AreEqual(value, args.NewItems[0]);
            Assert.AreEqual(0, args.NewStartingIndex);

            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Add_Object_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(list, x => ((IList) x).Add(GetRandomNumber()));
        }

        [Test]
        public void Add_Object_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => ((IList) x).Add(GetRandomNumber()), true);
        }

        [Test]
        public void Add_Object_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => ((IList) x).Add(GetRandomNumber()), true);
        }

        [Test]
        public void Add_Object_InvalidType_ThrowsArgumentException()
        {
            // Act & Assert
            AssertThrows<ArgumentException>(() => ((IList) list).Add(new object()));
        }

        [Test]
        public void Add_Object_Null_ThrowsArgumentNullException()
        {
            // Assert
            AssertThrows<ArgumentNullException>(() => ((IList) list).Add(null));
        }
    }
}