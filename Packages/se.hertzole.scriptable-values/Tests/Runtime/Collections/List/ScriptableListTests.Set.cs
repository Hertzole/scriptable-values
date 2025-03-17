#nullable enable

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void Set_SetsItem()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);

			// Act
			list[2] = 10;

			// Assert
			Assert.AreEqual(5, list.Count, "The count is not correct.");
			Assert.AreEqual(1, list[0], "The item at index 0 is not correct.");
			Assert.AreEqual(2, list[1], "The item at index 1 is not correct.");
			Assert.AreEqual(10, list[2], "The item at index 2 is not correct.");
			Assert.AreEqual(4, list[3], "The item at index 3 is not correct.");
			Assert.AreEqual(5, list[4], "The item at index 4 is not correct.");
		}

		[Test]
		public void Set_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list[2] = 10;

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(10, args.NewItems.Span[0], "The new item is not correct.");
			Assert.AreEqual(2, args.OldIndex, "The old index is not correct.");
			Assert.AreEqual(2, args.NewIndex, "The new index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
		}

		[Test]
		public void Set_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list[2] = 10;

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(1, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems[0], "The old item is not correct.");
			Assert.AreEqual(10, args.NewItems[0], "The new item is not correct.");
			Assert.AreEqual(2, args.OldStartingIndex, "The old index is not correct.");
			Assert.AreEqual(2, args.NewStartingIndex, "The new index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
		}

		[Test]
		public void Set_OutsideBounds_ThrowsArgumentOutOfRangeException([Values(-1, 5)] int index)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);

			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list[index] = 10);
		}

		[Test]
		public void Set_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list[2] = 10);
		}

		[Test]
		public void Set_ReadOnly_DoesNotInvokeCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.IsReadOnly = true;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			AssertThrows<ReadOnlyException>(() => list[2] = 10);
		}

		[Test]
		public void Set_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.IsReadOnly = true;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			AssertThrows<ReadOnlyException>(() => list[2] = 10);
		}

		[Test]
		public void Set_SameValue_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list[2] = 3;

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Set_SameValue_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list[2] = 3;

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Set_SameValue_NoEqualityCheck_DoesInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.SetEqualityCheck = false;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list[2] = 3;

			// Assert
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
		}

		[Test]
		public void Set_SameValue_NoEqualityCheck_DoesInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.SetEqualityCheck = false;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list[2] = 3;

			// Assert
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
		}

		[Test]
		public void Set_Object_SetsItem()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			IList l = list;

			// Act
			l[2] = 10;

			// Assert
			Assert.AreEqual(5, l.Count, "The count is not correct.");
			Assert.AreEqual(1, l[0], "The item at index 0 is not correct.");
			Assert.AreEqual(2, l[1], "The item at index 1 is not correct.");
			Assert.AreEqual(10, l[2], "The item at index 2 is not correct.");
			Assert.AreEqual(4, l[3], "The item at index 3 is not correct.");
			Assert.AreEqual(5, l[4], "The item at index 4 is not correct.");
		}

		[Test]
		public void Set_Object_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);
			IList l = list;

			// Act
			l[2] = 10;

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(1, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems.Span[0], "The old item is not correct.");
			Assert.AreEqual(10, args.NewItems.Span[0], "The new item is not correct.");
			Assert.AreEqual(2, args.OldIndex, "The old index is not correct.");
			Assert.AreEqual(2, args.NewIndex, "The new index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
		}

		[Test]
		public void Set_Object_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);
			IList l = list;

			// Act
			l[2] = 10;

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(1, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(3, args.OldItems[0], "The old item is not correct.");
			Assert.AreEqual(10, args.NewItems[0], "The new item is not correct.");
			Assert.AreEqual(2, args.OldStartingIndex, "The old starting index is not correct.");
			Assert.AreEqual(2, args.NewStartingIndex, "The new starting index is not correct.");
			Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
		}

		[Test]
		public void Set_Object_InvalidType_ThrowsArgumentException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			IList l = list;

			// Act & Assert
			AssertThrows<ArgumentException>(() => l[2] = "Test");
		}
	}
}