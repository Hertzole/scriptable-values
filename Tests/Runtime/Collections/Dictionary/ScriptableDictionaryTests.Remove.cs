#nullable enable

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
		public void Remove_RemovesItem()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act
			bool result = dictionary.Remove(key);

			// Assert
			Assert.IsTrue(result, "The item was not removed.");
			Assert.AreEqual(0, dictionary.Count, "The count is not correct.");
		}

		[Test]
		public void Remove_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

			// Act
			dictionary.Remove(key);

			// Assert
			CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

			Assert.AreEqual(0, args.NewItems.Length);
			Assert.AreEqual(-1, args.NewIndex);

			Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(-1, args.OldIndex, "The old index is not correct.");

			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void Remove_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

			// Act
			dictionary.Remove(key);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

			Assert.IsNull(args.NewItems);
			Assert.AreEqual(-1, args.NewStartingIndex);

			Assert.AreEqual(1, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems[0], "The old item is not correct.");
			Assert.AreEqual(-1, args.OldStartingIndex, "The old index is not correct.");
		}

		[Test]
		public void Remove_NonExistentKey_ReturnsFalse()
		{
			// Arrange
			int key = GetRandomNumber();

			// Act
			bool result = dictionary.Remove(key);

			// Assert
			Assert.IsFalse(result, "The item was removed when it should not have been.");
			Assert.AreEqual(0, dictionary.Count, "The count is not correct.");
		}

		[Test]
		public void Remove_NonExistentKey_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => x.Remove(GetRandomNumber()));
		}

		[Test]
		public void Remove_NonExistentKey_DoesNotInvokeINotifyCollectionChanged()
		{
			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => x.Remove(GetRandomNumber()));
		}

		[Test]
		public void Remove_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertThrowsReadOnlyException(dictionary, d => d.Remove(key));
		}

		[Test]
		public void Remove_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(dictionary, eventType, d => d.Remove(key), true);
		}

		[Test]
		public void Remove_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(dictionary, d => d.Remove(key), true);
		}

		[Test]
		public void Remove_Object_RemovesItem()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act
			((IDictionary) dictionary).Remove(key);

			// Assert
			Assert.AreEqual(0, dictionary.Count, "The count is not correct.");
		}

		[Test]
		public void Remove_Object_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

			// Act
			((IDictionary) dictionary).Remove(key);

			// Assert
			CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

			Assert.AreEqual(0, args.NewItems.Length);
			Assert.AreEqual(-1, args.NewIndex);

			Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(-1, args.OldIndex, "The old index is not correct.");

			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void Remove_Object_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

			// Act
			((IDictionary) dictionary).Remove(key);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

			Assert.IsNull(args.NewItems);
			Assert.AreEqual(-1, args.NewStartingIndex);

			Assert.AreEqual(1, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(new KeyValuePair<int, int>(key, value), args.OldItems[0], "The old item is not correct.");
			Assert.AreEqual(-1, args.OldStartingIndex, "The old index is not correct.");
		}

		[Test]
		public void Remove_Object_InvalidKey_DoesNotRemoveItem()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act
			((IDictionary) dictionary).Remove(key + 1);

			// Assert
			Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
			Assert.AreEqual(value, dictionary[key], "The value is not correct.");
		}

		[Test]
		public void Remove_Object_InvalidKey_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary) x).Remove(key + 1));
		}

		[Test]
		public void Remove_Object_InvalidKey_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary) x).Remove(key + 1));
		}

		[Test]
		public void Remove_Object_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertThrowsReadOnlyException(dictionary, x => ((IDictionary) x).Remove(key));
		}

		[Test]
		public void Remove_Object_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary) x).Remove(key), true);
		}

		[Test]
		public void Remove_Object_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary) x).Remove(key), true);
		}

		[Test]
		public void Remove_Object_NonExistentKey_DoesNotRemoveItem()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act
			((IDictionary) dictionary).Remove(key + 1);

			// Assert
			Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
			Assert.AreEqual(value, dictionary[key], "The value is not correct.");
		}

		[Test]
		public void Remove_Object_NonExistentKey_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary) x).Remove(key + 1));
		}

		[Test]
		public void Remove_Object_NonExistentKey_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary) x).Remove(key + 1));
		}

		[Test]
		public void Remove_KeyValuePair_RemovesItem()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);

			// Act
			bool result = ((IDictionary<int, int>) dictionary).Remove(pair);

			// Assert
			Assert.IsTrue(result, "The item was not removed.");
			Assert.AreEqual(0, dictionary.Count, "The count is not correct.");
		}

		[Test]
		public void Remove_KeyValuePair_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);
			CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary, eventType);

			// Act
			((IDictionary<int, int>) dictionary).Remove(pair);

			// Assert
			CollectionChangedArgs<KeyValuePair<int, int>> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

			Assert.AreEqual(0, args.NewItems.Length);
			Assert.AreEqual(-1, args.NewIndex);

			Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(pair, args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(-1, args.OldIndex, "The old index is not correct.");

			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void Remove_KeyValuePair_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);
			CollectionEventTracker<KeyValuePair<int, int>> tracker = new CollectionEventTracker<KeyValuePair<int, int>>(dictionary);

			// Act
			((IDictionary<int, int>) dictionary).Remove(pair);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

			Assert.IsNull(args.NewItems);
			Assert.AreEqual(-1, args.NewStartingIndex);

			Assert.AreEqual(1, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(pair, args.OldItems[0], "The old item is not correct.");
			Assert.AreEqual(-1, args.OldStartingIndex, "The old index is not correct.");
		}

		[Test]
		public void Remove_KeyValuePair_InvalidKey_DoesNotRemoveItem()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key + 1, value);

			// Act
			bool result = ((IDictionary<int, int>) dictionary).Remove(pair);

			// Assert
			Assert.IsFalse(result, "The item was removed when it should not have been.");
			Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
			Assert.AreEqual(value, dictionary[key], "The value is not correct.");
		}

		[Test]
		public void Remove_KeyValuePair_InvalidKey_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key + 1, value);

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary<int, int>) x).Remove(pair));
		}

		[Test]
		public void Remove_KeyValuePair_InvalidKey_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key + 1, value);

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary<int, int>) x).Remove(pair));
		}

		[Test]
		public void Remove_KeyValuePair_InvalidValue_DoesNotRemoveItem()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value + 1);

			// Act
			bool result = ((IDictionary<int, int>) dictionary).Remove(pair);

			// Assert
			Assert.IsFalse(result, "The item was removed when it should not have been.");
			Assert.AreEqual(1, dictionary.Count, "The count is not correct.");
			Assert.AreEqual(value, dictionary[key], "The value is not correct.");
		}

		[Test]
		public void Remove_KeyValuePair_InvalidValue_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value + 1);

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary<int, int>) x).Remove(pair));
		}

		[Test]
		public void Remove_KeyValuePair_InvalidValue_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value + 1);

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary<int, int>) x).Remove(pair));
		}

		[Test]
		public void Remove_KeyValuePair_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertThrowsReadOnlyException(dictionary, x => ((IDictionary<int, int>) x).Remove(pair));
		}

		[Test]
		public void Remove_KeyValuePair_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(dictionary, eventType, x => ((IDictionary<int, int>) x).Remove(pair), true);
		}

		[Test]
		public void Remove_KeyValuePair_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			KeyValuePair<int, int> pair = new KeyValuePair<int, int>(key, value);
			dictionary.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(dictionary, x => ((IDictionary<int, int>) x).Remove(pair), true);
		}
	}
}