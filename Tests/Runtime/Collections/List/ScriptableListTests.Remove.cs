#nullable enable

using System.Collections;
using System.Collections.Specialized;
using System.Data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void Remove_RemovesObject()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);

			// Act
			bool removed = list.Remove(3);

			// Assert
			Assert.IsTrue(removed, "The item was not removed.");
			Assert.AreEqual(4, list.Count, "The count is not correct.");
			Assert.AreEqual(1, list[0], "The item at index 0 is not correct.");
			Assert.AreEqual(2, list[1], "The item at index 1 is not correct.");
			Assert.AreEqual(4, list[2], "The item at index 2 is not correct.");
			Assert.AreEqual(5, list[3], "The item at index 3 is not correct.");
		}

		[Test]
		public void Remove_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.Remove(3);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(2, args.OldIndex, "The old index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void Remove_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.Remove(3);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(1, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems[0], "The old item is not correct.");
			Assert.AreEqual(2, args.OldStartingIndex, "The old starting index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void Remove_NonExistingObject_ReturnsFalse()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);

			// Act
			bool removed = list.Remove(6);

			// Assert
			Assert.IsFalse(removed, "The item was removed.");
			Assert.AreEqual(5, list.Count, "The count is not correct.");
			for (int i = 0; i < items.Length; i++)
			{
				Assert.AreEqual(items[i], list[i], $"The item at index {i} is not correct.");
			}
		}

		[Test]
		public void Remove_NonExistingObject_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.Remove(6);

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Remove_NonExistingObject_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.Remove(6);

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Remove_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.Remove(3));
		}

		[Test]
		public void Remove_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.Remove(3));
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Remove_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);
			list.IsReadOnly = true;

			// Act
			AssertThrows<ReadOnlyException>(() => list.Remove(3));

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Remove_Object_RemovesItem()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);

			// Act
			((IList) list).Remove(3);

			// Assert
			Assert.AreEqual(4, list.Count, "The count is not correct.");
			Assert.AreEqual(1, list[0], "The item at index 0 is not correct.");
			Assert.AreEqual(2, list[1], "The item at index 1 is not correct.");
			Assert.AreEqual(4, list[2], "The item at index 2 is not correct.");
			Assert.AreEqual(5, list[3], "The item at index 3 is not correct.");
		}

		[Test]
		public void Remove_Object_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			((IList) list).Remove(3);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(2, args.OldIndex, "The old index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void Remove_Object_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			((IList) list).Remove(3);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(1, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems[0], "The old item is not correct.");
			Assert.AreEqual(2, args.OldStartingIndex, "The old starting index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void Remove_Object_NonExistingObject_DoesNothing()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);

			// Act
			((IList) list).Remove(6);

			// Assert
			Assert.AreEqual(5, list.Count, "The count is not correct.");
			for (int i = 0; i < items.Length; i++)
			{
				Assert.AreEqual(items[i], list[i], $"The item at index {i} is not correct.");
			}
		}

		[Test]
		public void Remove_Object_Invalid()
		{
			// Arrange
			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			// Act
			((IList) list).Remove("invalid");
		}
	}
}