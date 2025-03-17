#nullable enable

using System;
using System.Collections.Specialized;
using System.Data;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void RemoveRange_RemovesItems()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			list.AddRange(items);

			// Act
			list.RemoveRange(2, 3);

			// Assert
			Assert.AreEqual(6, list.Count, "The count is not correct.");
			Assert.AreEqual(1, list[0], "The item at index 0 is not correct.");
			Assert.AreEqual(2, list[1], "The item at index 1 is not correct.");
			Assert.AreEqual(6, list[2], "The item at index 2 is not correct.");
			Assert.AreEqual(7, list[3], "The item at index 3 is not correct.");
			Assert.AreEqual(8, list[4], "The item at index 4 is not correct.");
			Assert.AreEqual(9, list[5], "The item at index 5 is not correct.");
		}

		[Test]
		public void RemoveRange_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.RemoveRange(2, 3);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(3, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(4, args.OldItems.Span[1], "The old item is not correct.");
			Assert.AreEqual(5, args.OldItems.Span[2], "The old item is not correct.");
			Assert.AreEqual(2, args.OldIndex, "The old index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void RemoveRange_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.RemoveRange(2, 3);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(3, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems[0], "The old item is not correct.");
			Assert.AreEqual(4, args.OldItems[1], "The old item is not correct.");
			Assert.AreEqual(5, args.OldItems[2], "The old item is not correct.");
			Assert.AreEqual(2, args.OldStartingIndex, "The old index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void RemoveRange_InvalidIndex_ThrowsArgumentOutOfRangeException([Values(-1, 10)] int index)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			list.AddRange(items);

			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.RemoveRange(index, 1));
		}

		[Test]
		public void RemoveRange_InvalidCount_ThrowsArgumentOutOfRangeException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			list.AddRange(items);

			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.RemoveRange(2, 8));
		}

		[Test]
		public void RemoveRange_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.RemoveRange(2, 3));
		}

		[Test]
		public void RemoveRange_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			list.AddRange(items);
			list.IsReadOnly = true;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			AssertThrows<ReadOnlyException>(() => list.RemoveRange(2, 3));

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void RemoveRange_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			list.AddRange(items);
			list.IsReadOnly = true;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			AssertThrows<ReadOnlyException>(() => list.RemoveRange(2, 3));

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}
	}
}