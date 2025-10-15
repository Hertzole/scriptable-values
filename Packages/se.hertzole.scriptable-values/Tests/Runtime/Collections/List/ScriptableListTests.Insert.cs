#nullable enable

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;
using Random = UnityEngine.Random;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void Insert_InsertsItem()
        {
            // Arrange
            int value = GetRandomNumber();
            int index = Random.Range(20, 40);
            list.AddRange(Enumerable.Range(0, 50));

            // Act
            list.Insert(index, value);

            // Assert
            Assert.AreEqual(value, list[index]);
            Assert.AreEqual(51, list.Count);
        }

        [Test]
        public void Insert_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int value = GetRandomNumber();
            int index = Random.Range(20, 40);
            list.AddRange(Enumerable.Range(0, 50));
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

            // Act
            list.Insert(index, value);

            // Assert
            CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(1, args.NewItems.Length);
            Assert.AreEqual(value, args.NewItems.Span[0]);
            Assert.AreEqual(index, args.NewIndex);

            Assert.AreEqual(0, args.OldItems.Length);
            Assert.AreEqual(-1, args.OldIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Insert_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int value = GetRandomNumber();
            int index = Random.Range(20, 40);
            list.AddRange(Enumerable.Range(0, 50));
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

            // Act
            list.Insert(index, value);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsNotNull(args);
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.IsNotNull(args.NewItems);
            Assert.AreEqual(1, args.NewItems.Count);
            Assert.AreEqual(value, args.NewItems[0]);
            Assert.AreEqual(index, args.NewStartingIndex);

            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Insert_ReadOnly_ThrowsReadOnlyException()
        {
            // Act & Assert
            AssertThrowsReadOnlyException(list, x => x.Insert(5, GetRandomNumber()));
        }

        [Test]
        public void Insert_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.Insert(5, GetRandomNumber()), true);
        }

        [Test]
        public void Insert_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.Insert(5, GetRandomNumber()), true);
        }

        [Test]
        public void Insert_OutOfBounds_ThrowsArgumentOutOfRangeException([Values(-1, 1)] int index)
        {
            // Act & Assert
            AssertThrows<ArgumentOutOfRangeException>(() => list.Insert(index, GetRandomNumber()));
        }

        [Test]
        public void Insert_Object_InsertsItem()
        {
            // Arrange
            int value = GetRandomNumber();
            int index = Random.Range(20, 40);
            list.AddRange(Enumerable.Range(0, 50));

            // Act
            ((IList) list).Insert(index, value);

            // Assert
            Assert.AreEqual(51, list.Count);
            Assert.AreEqual(value, list[index]);
        }

        [Test]
        public void Insert_Object_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int value = GetRandomNumber();
            int index = Random.Range(20, 40);
            list.AddRange(Enumerable.Range(0, 50));
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

            // Act
            ((IList) list).Insert(index, value);

            // Assert
            CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(1, args.NewItems.Length);
            Assert.AreEqual(value, args.NewItems.Span[0]);
            Assert.AreEqual(index, args.NewIndex);

            Assert.AreEqual(0, args.OldItems.Length);
            Assert.AreEqual(-1, args.OldIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Insert_Object_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int value = GetRandomNumber();
            int index = Random.Range(20, 40);
            list.AddRange(Enumerable.Range(0, 50));
            using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

            // Act
            ((IList) list).Insert(index, value);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsNotNull(args);
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.IsNotNull(args.NewItems);
            Assert.AreEqual(1, args.NewItems.Count);
            Assert.AreEqual(value, args.NewItems[0]);
            Assert.AreEqual(index, args.NewStartingIndex);

            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Insert_Object_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(list, x => x.Insert(5, GetRandomNumber()));
        }

        [Test]
        public void Insert_Object_ReadOnly_DoesNotInvokeCollectinChanged([Values] EventType eventType)
        {
            // Arrange
            list.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(list, eventType, x => ((IList) x).Insert(5, GetRandomNumber()), true);
        }

        [Test]
        public void Insert_Object_OutOfBounds_ThrowsArgumentOutOfRangeException([Values(-1, 1)] int index)
        {
            // Act & Assert
            AssertThrows<ArgumentOutOfRangeException>(() => ((IList) list).Insert(index, GetRandomNumber()));
        }

        [Test]
        public void Insert_Object_InvalidType_ThrowsArgumentException()
        {
            // Arrange
            object value = new object();

            // Act & Assert
            AssertThrows<ArgumentException>(() => ((IList) list).Insert(0, value));
        }

        [Test]
        public void Insert_Object_Null_ThrowsArgumentNullException()
        {
            // Act & Assert
            AssertThrows<ArgumentNullException>(() => ((IList) list).Insert(0, null));
        }
    }
}