#nullable enable

using System.Collections.Specialized;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void Clear_ClearsItems()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);

			// Act
			list.Clear();

			// Assert
			Assert.AreEqual(0, list.Count, "The count is not correct.");
		}

		[Test]
		public void Clear_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.Clear();

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

			Assert.AreEqual(0, args.NewItems.Length, "The new items length is not correct.");
			Assert.AreEqual(0, args.NewIndex, "The new index is not correct.");

			AssertArraysAreEqual(items, args.OldItems.ToArray());
			Assert.AreEqual(0, args.OldIndex, "The old index is not correct.");

			Assert.AreEqual(NotifyCollectionChangedAction.Reset, args.Action, "The action is not correct.");
		}

		[Test]
		public void Clear_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.Clear();

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");

			// Reset does not support any items.
			Assert.IsNull(args.NewItems, "There shouldn't be any new items");
			Assert.AreEqual(-1, args.NewStartingIndex, "The new index is not correct.");

			Assert.IsNull(args.OldItems, "There shouldn't be any old items");
			Assert.AreEqual(-1, args.OldStartingIndex, "The old index is not correct.");

			Assert.AreEqual(NotifyCollectionChangedAction.Reset, args.Action, "The action is not correct.");
		}

		[Test]
		public void Clear_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrowsReadOnlyException(list, x => x.Clear());
		}

		[Test]
		public void Clear_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.Clear(), true);
		}

		[Test]
		public void Clear_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.Clear(), true);
		}

		[Test]
		public void Clear_Empty_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Act & Arrange
			AssertDoesNotInvokeCollectionChanged(list, eventType, x => x.Clear());
		}

		[Test]
		public void Clear_Empty_DoesNotInvokeINotifyCollectionChanged()
		{
			// Act & Arrange
			AssertDoesNotInvokeINotifyCollectionChanged(list, x => x.Clear());
		}
	}
}