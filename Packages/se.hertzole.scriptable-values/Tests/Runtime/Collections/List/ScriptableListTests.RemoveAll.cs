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
		public void RemoveAll_RemovesAllItems()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
			list.AddRange(items);

			// Act
			int removed = list.RemoveAll(x => x == 3);

			// Assert
			Assert.AreEqual(11, removed, "The amount of removed items is not correct.");
			Assert.AreEqual(4, list.Count, "The count is not correct.");
			Assert.IsFalse(list.Contains(3));
		}

		[Test]
		public void RemoveAll_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.RemoveAll(x => x == 3);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(11, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(2, args.OldIndex, "The old index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void RemoveAll_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.RemoveAll(x => x == 3);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(11, args.OldItems.Count, "The old items count is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action, "The action is not correct.");
		}

		[Test]
		public void RemoveAll_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.RemoveAll(x => x == 3));
		}

		[Test]
		public void RemoveAll_ReadOnly_DoesNotInvokeCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
			list.AddRange(items);
			list.IsReadOnly = true;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			AssertThrows<ReadOnlyException>(() => list.RemoveAll(x => x == 3));

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void RemoveAll_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
			list.AddRange(items);
			list.IsReadOnly = true;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			AssertThrows<ReadOnlyException>(() => list.RemoveAll(x => x == 3));

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void RemoveAll_NullPredicate_ThrowsArgumentNullException()
		{
			// Act & Assert
			AssertThrows<ArgumentNullException>(() => list.RemoveAll(null!));
		}

		[Test]
		public void RemoveAll_NoMatch_ReturnsZero()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);

			// Act
			int removed = list.RemoveAll(x => x == 10);

			// Assert
			Assert.AreEqual(0, removed, "The amount of removed items is not correct.");
			Assert.AreEqual(5, list.Count, "The count is not correct.");
		}
	}
}