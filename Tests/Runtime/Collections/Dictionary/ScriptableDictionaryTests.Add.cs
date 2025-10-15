#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableDictionaryTests
    {
        [Test]
        public void Add_AddsItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();

            // Act
            dictionary.Add(key, value);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(value, dictionary[key]);
        }

        [Test]
        public void Add_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            // Act
            dictionary.Add(key, value);

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
        public void Add_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            // Act
            dictionary.Add(key, value);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.IsNotNull(args.NewItems);
            Assert.AreEqual(1, args.NewItems.Count);
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems[0]);
            Assert.AreEqual(-1, args.NewStartingIndex);

            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Add_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(dictionary, x => x.Add(GetRandomNumber(), GetRandomNumber()));
        }

        [Test]
        public void Add_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => x.Add(GetRandomNumber(), GetRandomNumber()), true);
        }

        [Test]
        public void Add_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => x.Add(GetRandomNumber(), GetRandomNumber()), true);
        }

        [Test]
        public void Add_Object_AddsItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();

            // Act
            ((IDictionary) dictionary).Add(key, value);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(value, dictionary[key]);
        }

        [Test]
        public void Add_Object_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            // Act
            ((IDictionary) dictionary).Add(key, value);

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
        public void Add_Object_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            // Act
            ((IDictionary) dictionary).Add(key, value);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.IsNotNull(args.NewItems);
            Assert.AreEqual(1, args.NewItems.Count);
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems[0]);
            Assert.AreEqual(-1, args.NewStartingIndex);

            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Add_Object_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(dictionary, x => ((IDictionary) x).Add(GetRandomNumber(), GetRandomNumber()));
        }

        [Test]
        public void Add_Object_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary) x).Add(GetRandomNumber(), GetRandomNumber()), true);
        }

        [Test]
        public void Add_Object_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary) x).Add(GetRandomNumber(), GetRandomNumber()), true);
        }

        [Test]
        public void Add_Object_InvalidKeyType_ThrowsArgumentException()
        {
            // Arrange
            object key = new object();
            int value = GetRandomNumber();

            // Act & Assert
            AssertThrows<ArgumentException>(() => ((IDictionary) dictionary).Add(key, value));
        }

        [Test]
        public void Add_Object_InvalidValueType_ThrowsArgumentException()
        {
            // Arrange
            int key = GetRandomNumber();
            object value = new object();

            // Act & Assert
            AssertThrows<ArgumentException>(() => ((IDictionary) dictionary).Add(key, value));
        }

        [Test]
        public void Add_Object_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            int value = GetRandomNumber();

            // Act & Assert
            AssertThrows<ArgumentNullException>(() => ((IDictionary) dictionary).Add(null!, value));
        }

        [Test]
        public void Add_Object_NullValue_ThrowsArgumentNullException()
        {
            // Arrange
            int key = GetRandomNumber();

            // Act & Assert
            AssertThrows<ArgumentNullException>(() => ((IDictionary) dictionary).Add(key, null!));
        }

        [Test]
        public void Add_KeyValuePair_AddsItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);

            // Act
            ((IDictionary<int, int>) dictionary).Add(pair);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(value, dictionary[key]);
        }

        [Test]
        public void Add_KeyValuePair_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            // Act
            ((IDictionary<int, int>) dictionary).Add(pair);

            // Assert
            CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(1, args.NewItems.Length);
            Assert.AreEqual(pair, args.NewItems.Span[0]);
            Assert.AreEqual(-1, args.NewIndex);

            Assert.AreEqual(0, args.OldItems.Length);
            Assert.AreEqual(-1, args.OldIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Add_KeyValuePair_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            // Act
            ((IDictionary<int, int>) dictionary).Add(pair);

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.IsNotNull(args.NewItems);
            Assert.AreEqual(1, args.NewItems.Count);
            Assert.AreEqual(pair, args.NewItems[0]);
            Assert.AreEqual(-1, args.NewStartingIndex);

            Assert.IsNull(args.OldItems);
            Assert.AreEqual(-1, args.OldStartingIndex);

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
        }

        [Test]
        public void Add_KeyValuePair_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            dictionary.IsReadOnly = true;
            KeyValuePair<int, int> pair = new KeyValuePair<int, int>(GetRandomNumber(), GetRandomNumber());

            // Act & Assert
            AssertThrowsReadOnlyException(dictionary, x => ((IDictionary<int, int>) x).Add(pair));
        }

        [Test]
        public void Add_KeyValuePair_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            dictionary.IsReadOnly = true;
            KeyValuePair<int, int> pair = new KeyValuePair<int, int>(GetRandomNumber(), GetRandomNumber());

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary<int, int>) x).Add(pair), true);
        }

        [Test]
        public void Add_KeyValuePair_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            dictionary.IsReadOnly = true;
            KeyValuePair<int, int> pair = new KeyValuePair<int, int>(GetRandomNumber(), GetRandomNumber());

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary<int, int>) x).Add(pair), true);
        }
    }
}