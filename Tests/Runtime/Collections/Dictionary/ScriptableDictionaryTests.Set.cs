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
        public void Set_NoExistingItem_AddsItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();

            // Act
            dictionary[key] = value;

            // Assert
            Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
            Assert.AreEqual(value, dictionary[key], "The value is not correct.");
        }

        [Test]
        public void Set_NoExistingItem_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            // Act
            dictionary[key] = value;

            // Assert
            CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Length, "The new items length is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems.Span[0], "The new item is not correct.");
            Assert.AreEqual(-1, args.NewIndex, "The new index is not correct.");

            Assert.AreEqual(0, args.OldItems.Length, "The old items length is not correct.");
            Assert.AreEqual(-1, args.OldIndex, "The old index is not correct.");

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_NoExistingItem_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            // Act
            dictionary[key] = value;

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Count, "The new items count is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems[0], "The new item is not correct.");
            Assert.AreEqual(-1, args.NewStartingIndex, "The new starting index is not correct.");

            Assert.IsNull(args.OldItems, "Old items should be null.");
            Assert.AreEqual(-1, args.OldStartingIndex, "The old starting index is not correct.");

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_ExistingItem_ReplacesItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;

            int newValue = GetRandomNumber();

            // Act
            dictionary[key] = newValue;

            // Assert
            Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
            Assert.AreEqual(newValue, dictionary[key], "The value is not correct.");
        }

        [Test]
        public void Set_ExistingItem_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            int newValue = GetRandomNumber();

            // Act
            dictionary[key] = newValue;

            // Assert
            CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Length, "The new items length is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, newValue), args.NewItems.Span[0], "The new item is not correct.");
            Assert.AreEqual(-1, args.NewIndex, "The new index is not correct.");

            Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems.Span[0], "The old item is not correct.");
            Assert.AreEqual(-1, args.OldIndex, "The old index is not correct.");

            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_ExistingItem_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            int newValue = GetRandomNumber();

            // Act
            dictionary[key] = newValue;

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Count, "The new items count is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, newValue), args.NewItems[0], "The new item is not correct.");
            Assert.AreEqual(-1, args.NewStartingIndex, "The new starting index is not correct.");

            Assert.AreEqual(1, args.OldItems.Count, "The old items count is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems[0], "The old item is not correct.");
            Assert.AreEqual(-1, args.OldStartingIndex, "The old starting index is not correct.");

            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(dictionary, x => x[key] = value);
        }

        [Test]
        public void Set_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => x[key] = value, true);
        }

        [Test]
        public void Set_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => x[key] = value, true);
        }

        [Test]
        public void Set_SameValue_NoEqualsCheck_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;
            dictionary.SetEqualityCheck = false;
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            // Act
            dictionary[key] = value;

            // Assert
            CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Length, "The new items length is not correct.");
            Assert.AreEqual(-1, args.NewIndex, "The new index is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems.Span[0]);

            Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
            Assert.AreEqual(-1, args.OldIndex, "The old index is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems.Span[0]);

            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_SameValue_NoEqualsCheck_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;
            dictionary.SetEqualityCheck = false;
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            // Act
            dictionary[key] = value;

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Count, "The new items count is not correct.");
            Assert.AreEqual(-1, args.NewStartingIndex, "The new starting index is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems[0]);

            Assert.AreEqual(1, args.OldItems.Count, "The old items count is not correct.");
            Assert.AreEqual(-1, args.OldStartingIndex, "The old starting index is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems[0]);

            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_SameValue_EqualsCheck_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;
            dictionary.SetEqualityCheck = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => x[key] = value);
        }

        [Test]
        public void Set_SameValue_EqualsCheck_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;
            dictionary.SetEqualityCheck = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => x[key] = value);
        }

        [Test]
        public void Set_SameValue_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;
            dictionary.SetEqualityCheck = true;
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(dictionary, x => x[key] = value);
        }

        [Test]
        public void Set_Object_NoExistingItem_AddsItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();

            // Act
            ((IDictionary) dictionary)[key] = value;

            // Assert
            Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
            Assert.AreEqual(value, dictionary[key], "The value is not correct.");
        }

        [Test]
        public void Set_Object_NoExistingItem_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            // Act
            ((IDictionary) dictionary)[key] = value;

            // Assert
            CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Length, "The new items length is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems.Span[0], "The new item is not correct.");
            Assert.AreEqual(-1, args.NewIndex, "The new index is not correct.");

            Assert.AreEqual(0, args.OldItems.Length, "The old items length is not correct.");
            Assert.AreEqual(-1, args.OldIndex, "The old index is not correct.");

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_Object_NoExistingItem_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            // Act
            ((IDictionary) dictionary)[key] = value;

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Count, "The new items count is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.NewItems[0], "The new item is not correct.");
            Assert.AreEqual(-1, args.NewStartingIndex, "The new starting index is not correct.");

            Assert.IsNull(args.OldItems, "Old items should be null.");
            Assert.AreEqual(-1, args.OldStartingIndex, "The old starting index is not correct.");

            Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_Object_ExistingItem_ReplacesItem()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            ((IDictionary) dictionary)[key] = value;

            int newValue = GetRandomNumber();

            // Act
            ((IDictionary) dictionary)[key] = newValue;

            // Assert
            Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
            Assert.AreEqual(newValue, dictionary[key], "The value is not correct.");
        }

        [Test]
        public void Set_Object_ExistingItem_InvokesCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            ((IDictionary) dictionary)[key] = value;
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

            int newValue = GetRandomNumber();

            // Act
            ((IDictionary) dictionary)[key] = newValue;

            // Assert
            CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Length, "The new items length is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, newValue), args.NewItems.Span[0], "The new item is not correct.");
            Assert.AreEqual(-1, args.NewIndex, "The new index is not correct.");

            Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems.Span[0], "The old item is not correct.");
            Assert.AreEqual(-1, args.OldIndex, "The old index is not correct.");

            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_Object_ExistingItem_InvokesINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            ((IDictionary) dictionary)[key] = value;
            using CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

            int newValue = GetRandomNumber();

            // Act
            ((IDictionary) dictionary)[key] = newValue;

            // Assert
            NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
            Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

            Assert.AreEqual(1, args.NewItems.Count, "The new items count is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, newValue), args.NewItems[0], "The new item is not correct.");
            Assert.AreEqual(-1, args.NewStartingIndex, "The new starting index is not correct.");

            Assert.AreEqual(1, args.OldItems.Count, "The old items count is not correct.");
            Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems[0], "The old item is not correct.");
            Assert.AreEqual(-1, args.OldStartingIndex, "The old starting index is not correct.");

            Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
        }

        [Test]
        public void Set_Object_ReadOnly_ThrowsReadOnlyException()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertThrowsReadOnlyException(dictionary, x => ((IDictionary) x)[key] = value);
        }

        [Test]
        public void Set_Object_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary) x)[key] = value, true);
        }

        [Test]
        public void Set_Object_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary.IsReadOnly = true;

            // Act & Assert
            AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary) x)[key] = value, true);
        }

        [Test]
        public void Set_Object_InvalidKey_ThrowsArgumentException()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;

            // Act & Assert
            AssertThrows<ArgumentException>(() => ((IDictionary) dictionary)[new object()] = value);
        }

        [Test]
        public void Set_Object_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;

            // Act & Assert
            AssertThrows<ArgumentNullException>(() => ((IDictionary) dictionary)[null!] = value);
        }

        [Test]
        public void Set_Object_InvalidValue_ThrowsArgumentException()
        {
            // Arrange
            int key = GetRandomNumber();
            int value = GetRandomNumber();
            dictionary[key] = value;

            // Act & Assert
            AssertThrows<ArgumentException>(() => ((IDictionary) dictionary)[key] = new object());
        }
    }
}